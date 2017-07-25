namespace Urlshortener.Tests.Controllers
{
    using System.Threading.Tasks;
    using Urlshortener.Controllers.Api;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;

    public class ShortenControllerTests
    {
        private ShortenController controller;

        public ShortenControllerTests()
        {
            this.controller = new ShortenController(() => string.Empty, s => Task.Run(() => true), s => s, (h) => Task.Run(() => h));
        }

        [Fact]
        public void DefaultIndexShouldReturnNotFoundResult()
        {
            var result = this.controller.Index();

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ErrorReturnsBadRequestResult()
        {
            var result = this.controller.Error();

            Assert.IsType<BadRequestResult>(result);
        }
    }
}