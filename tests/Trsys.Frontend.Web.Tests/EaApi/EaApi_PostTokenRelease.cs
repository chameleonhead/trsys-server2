using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Threading.Tasks;

namespace Trsys.Frontend.Web.Tests.EaApi
{
    [TestClass]
    public class EaApi_PostTokenRelease
    {
        private WebApplicationFactory<Startup> _factory;

        [TestInitialize]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Startup>();
        }

        [TestCleanup]
        public void Teardown()
        {
            _factory.Dispose();
        }

        [TestMethod]
        public async Task ValidToken_ReturnSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();
            await client.RegisterSecretKeyAsync("ValidToken_ReturnSuccess", "Publisher");
            var token = await client.GenerateTokenAsync("ValidToken_ReturnSuccess", "Publisher");

            // Act
            var response = await client.PostAsync($"/api/token/{token}/release", "ValidToken_ReturnSuccess", "Publisher");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestMethod]
        public async Task InvalidToken_ReturnBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync($"/api/token/INVALID_TOKEN/release", "ValidToken_ReturnSuccess", "Publisher");

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
