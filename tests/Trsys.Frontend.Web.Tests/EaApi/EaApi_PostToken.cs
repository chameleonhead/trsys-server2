using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Threading.Tasks;

namespace Trsys.Frontend.Web.Tests.EaApi
{
    [TestClass]
    public class EaApi_PostToken
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
        public async Task ValidSecretKey_ReturnValidToken()
        {
            // Arrange
            var client = _factory.CreateClient();
            await client.RegisterSecretKeyAsync("ValidSecretKey_ReturnValidToken", "Publisher");

            // Act
            var response = await client.PostAsync("/api/token", "ValidSecretKey_ReturnValidToken", "Publisher", content: "ValidSecretKey_ReturnValidToken");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [TestMethod]
        public async Task InvalidSecretKey_ReturnBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/token", "INVALID_KEY", "Publisher", content: "INVALID_KEY");

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.AreEqual("InvalidSecretKey", await response.Content.ReadAsStringAsync());
        }

        [TestMethod]
        public async Task SecretKeyInUse_ReturnBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            await client.RegisterSecretKeyAsync("SecretKeyInUse_ReturnBadRequest", "Publisher");
            await client.GenerateTokenAsync("SecretKeyInUse_ReturnBadRequest", "Publisher");

            // Act
            var response = await client.PostAsync("/api/token", "SecretKeyInUse_ReturnBadRequest", "Publisher", content: "SecretKeyInUse_ReturnBadRequest");

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.AreEqual("SecretKeyInUse", await response.Content.ReadAsStringAsync());
        }
    }
}
