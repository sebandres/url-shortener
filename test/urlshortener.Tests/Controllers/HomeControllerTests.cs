namespace urlshortener.Tests.Controllers
{
    using urlshortener.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;

    public class HomeControllerTests
    {
        private HomeController controller;

        public HomeControllerTests()
        {
            this.controller = new HomeController();
        }

        [Fact]
        public void IndexShouldReturnViewResult()
        {
            var result = this.controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ErrorReturnsViewResult()
        {
            var result = this.controller.Error();

            Assert.IsType<ViewResult>(result);
        }
    }
}