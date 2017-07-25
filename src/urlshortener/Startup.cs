namespace Urlshortener
{
    using System;
    using System.Collections.Generic;
    using System.IO.Compression;
    using System.Linq;
    using System.Threading.Tasks;
    using Urlshortener.Controllers.Api;
    using Urlshortener.Functions;
    using Urlshortener.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables();

            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddSingleton<IConfiguration>(this.Configuration);
            services.AddSingleton<ShortenUrlFunctions.GetConnectionStringDelegate>(() => 
                this.Configuration.GetSection("ConnectionStrings")
                    .GetValue<string>("StorageConnection", string.Empty));
            services.AddSingleton<ShortenUrlFunctions.ShortenUrlDelegate>(originalUrl =>
                ShortenUrlFunctions.ShortenUrl(originalUrl));
            services.AddSingleton<ShortenUrlFunctions.RetrieveShortUrlDelegate>((hash) =>
                ShortenUrlFunctions.RetrieveShortUrl(
                    () => this.Configuration.GetSection("ConnectionStrings")
                        .GetValue<string>("StorageConnection", string.Empty), 
                    hash)
                );
            services.AddSingleton<ShortenUrlFunctions.SaveShortUrlDelegate>((shortUrl) =>
                ShortenUrlFunctions.SaveShortUrl(
                    () => this.Configuration.GetSection("ConnectionStrings")
                        .GetValue<string>("StorageConnection", string.Empty),
                    shortUrl)
                );
            services.AddMvc();
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));

            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
