namespace Urlshortener.Controllers.Api
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Urlshortener.Models;
    using Urlshortener.Functions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore;
    using System.Net;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.Extensions.Configuration;
    using Microsoft.WindowsAzure.Storage.Table;

    [Route("api/shorten")]
    public class ShortenController : Controller
    {
        private readonly ShortenUrlFunctions.GetConnectionStringDelegate GetConnectionString;
        private readonly ShortenUrlFunctions.SaveShortUrlDelegate SaveShortUrl;
        private readonly ShortenUrlFunctions.ShortenUrlDelegate ShortenUrl;
        private readonly ShortenUrlFunctions.RetrieveShortUrlDelegate RetrieveShortUrl;

        public ShortenController(
            ShortenUrlFunctions.GetConnectionStringDelegate getConnectionString, 
            ShortenUrlFunctions.SaveShortUrlDelegate saveShortUrl, 
            ShortenUrlFunctions.ShortenUrlDelegate shortenUrl,
            ShortenUrlFunctions.RetrieveShortUrlDelegate retrieveShortUrl)
        {
            this.GetConnectionString = getConnectionString;
            this.ShortenUrl = shortenUrl;
            this.SaveShortUrl = saveShortUrl;
            this.RetrieveShortUrl = retrieveShortUrl;
        }

        public IActionResult Index()
        {
            return new NotFoundResult();
        }

        [HttpGet("{hash}")]
        public async Task<IActionResult> Index(string hash)
        {
            var shortUrl = await ShortenUrlFunctions
                .RetrieveShortUrl(
                    this.GetConnectionString,
                    hash);

            if (shortUrl == null)
            {
                return new RedirectResult("~/error");
            }
            else
            {
                await ShortenUrlFunctions
                    .RecordShortUrlHit(
                        this.GetConnectionString,
                        hash);
                return new RedirectResult(shortUrl);
            }
        }

        [HttpPost]
        public IActionResult Index([FromBody]ShortUrlRequest request)
        {
            if (request == null)
            {
                return new BadRequestResult();
            }
            return new ShortUrlResponse(this.SaveShortUrl, this.ShortenUrl, this.RetrieveShortUrl, request);
        }

        [Route("error")]
        public IActionResult Error()
        {
            return new BadRequestResult();
        }
    }
}
