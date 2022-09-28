using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Parser.API.Controllers;
using Parser.API.Models;
using System.Configuration;

namespace Parser.API.Test
{
    public class LoadUrlControllerTests
    {
         [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void LoadUrlControllerTests_LoadUrl_Validate_Result_Type()
        {
            var configuration = new Mock<IConfiguration>();
            var logger = new Mock<ILogger<LoadUrlController>>();
            var mockController = new LoadUrlController(configuration.Object, logger.Object);
            var actResult = mockController.LoadUrl(String.Empty);

            Assert.That(actResult.Result, Is.TypeOf<Details>());
            Assert.Pass("API result type validated");
        }
    }
}