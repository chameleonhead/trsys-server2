using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Trsys.Frontend.Web.Services;

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
            await EaService.Instance.AddValidSecretKyeAsync("SECRETKEY", "Publisher");
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/token", new StringContent("SECRETKEY", Encoding.UTF8, "text/plain"));

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}
