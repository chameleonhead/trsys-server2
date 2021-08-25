using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}
