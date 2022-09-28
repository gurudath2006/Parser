using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Configuration;
using Parser.API.Models;
using System.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Web;

namespace Parser.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadUrlController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        
        private readonly Parser.API.Models.Settings _settings;
        public LoadUrlController(IConfiguration configuration, ILogger<LoadUrlController> logger)
        {
            _logger = logger;
            _configuration = configuration;
        }
        
        [HttpGet]
        public async Task<Details> LoadUrl(string url)
        {            
            var pageDetails = new Details();
            try
            {
                var _settings = _configuration.GetSection(nameof(Parser.API.Models.Settings)).Get<Parser.API.Models.Settings>();
                var requestUrl = $"{_settings.ProxySiteUrl}{url}";
                var host = new Uri(url).Host;
                var scheme = new Uri(url).Scheme;
                // Request URL
                var client = new HttpClient();
                var response = await client.GetAsync(requestUrl);
                var pageContents = await response.Content.ReadAsStringAsync();
                ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(pageContents);
                
                if (apiResponse == null)
                    return pageDetails;

                HtmlDocument pageDocument = new HtmlDocument();
                pageDocument.LoadHtml(HttpUtility.HtmlDecode( apiResponse.Contents));

                var imageSrcs = new List<string>();
                if (pageDocument.DocumentNode.SelectNodes("//img") != null)
                {
                    imageSrcs = pageDocument.DocumentNode.SelectNodes("//img").Where(x => !string.IsNullOrWhiteSpace(x.Attributes?["src"]?.Value)).Select(x => x.Attributes["src"].Value).Distinct().ToList();
                }
                var backgroundUrls = pageDocument.DocumentNode.SelectNodes("//div").Where(x=> !string.IsNullOrWhiteSpace(Regex.Match(x.GetAttributeValue("style", ""), @"(?<=url\()(.*)(?=\))").Groups[1].Value)).Select(x => Regex.Match(x.GetAttributeValue("style", ""), @"(?<=url\()(.*)(?=\))").Groups[1].Value).Distinct().ToList();
                imageSrcs.AddRange(backgroundUrls); 
                pageDetails.Images = imageSrcs.Select(x => {
                    Uri uriResult;
                    bool result = Uri.TryCreate(x, UriKind.Absolute, out uriResult)
                         && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                    if (result == true)
                    {
                        return x;
                    }
                    return $"{scheme}://{host}{x}";
                    
                    });
                

                // Words and total count
                var allWords = new List<string>();

                char[] delimiter = new char[] { ' ' };
                int totalWordCount = 0;
                foreach (string text in pageDocument.DocumentNode
                    .SelectNodes("//body//text()[not(parent::script)][not(parent::style)]")
                    .Select(node => node.InnerText))
                {
                    var words = text.Split(delimiter, StringSplitOptions.RemoveEmptyEntries)
                        .Where(s => Char.IsLetter(s[0]));
                    allWords.AddRange(words);
                    int wordCount = words.Count();
                    if (wordCount > 0)
                    {
                        totalWordCount += wordCount;
                    }
                }
                pageDetails.Words = allWords.GroupBy(w => w).Select(grp=> new Tuple<string,int>(grp.Key,grp.Count())).OrderByDescending(x=>x.Item2).Take(10);
                pageDetails.TotalCount = totalWordCount;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting details", this);
                pageDetails.IsSuccess = false;
            }
            return pageDetails;
        }  
    }
}
