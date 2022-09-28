using Microsoft.AspNetCore.Mvc;
using Parser.WebApp.Models;
using System.Diagnostics;

namespace Parser.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Parser.Web.Models.Settings _settings;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _settings = configuration.GetSection(nameof(Parser.Web.Models.Settings)).Get<Parser.Web.Models.Settings>();
        }

        public IActionResult Index()
        {
            ViewData["WebAPIEndPoint"] = _settings.WebAPIEndPoint;
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}