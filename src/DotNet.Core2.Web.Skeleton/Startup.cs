
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Newtonsoft.Json.Serialization;
using System.Runtime;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using DotNet.Core2.Web.Skeleton.Code;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DotNet.Core2.Web.Skeleton
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(options =>
            {
                options.LogoutPath = "/Account/Logout";
                options.LogoutPath = "/Account/Login";
            });

		    services.AddDataProtection(opts =>
           {
             opts.ApplicationDiscriminator = "MyAppName.web";
           });

            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
			
			
            services.AddMvcCore().
                     AddDataAnnotations().
                     AddJsonFormatters();

            services.Configure<GzipCompressionProviderOptions>
                  (options => options.Level = CompressionLevel.Fastest);
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.AddMvc((options) =>
            {
                options.CacheProfiles.Add("default", new CacheProfile()
                {
                    Duration = 0,
                    Location = ResponseCacheLocation.None
                });
                options.CacheProfiles.Add("MyCache", new CacheProfile()
                {
                    Duration = 0,
                    Location = ResponseCacheLocation.None
                });
            }).AddJsonOptions(
o =>
{
    CamelCasePropertyNamesContractResolver resorver = new CamelCasePropertyNamesContractResolver();
    resorver.NamingStrategy = null;
    o.SerializerSettings.ContractResolver = resorver;
    o.SerializerSettings.Converters.Add(new StringEnumConverter());
    o.SerializerSettings.Formatting = Formatting.None;
    o.SerializerSettings.NullValueHandling = NullValueHandling.Include;
    o.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
    o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Error;
});

            // https://github.com/PaulMiami/reCAPTCHA

            services.AddRecaptcha(new RecaptchaOptions
            {
                SiteKey = "6Ldi3yYUAAAAABxsWPuc73WuxoGGN7DC-y0AOFCG",//Configuration["Recaptcha:SiteKey"],
                SecretKey = "6Ldi3yYUAAAAAE4pflIk94pb0scUlz_-BcXqojUq",//Configuration["Recaptcha:SecretKey"],
                ValidationMessage = "Are you a robot?"
            });

        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {


            app.UseAuthentication();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var logger = loggerFactory.CreateLogger("DotNet.Core2.Web.Skeleton ");
            MyLogger.Logger = logger;

            MyLogger.LogInformation($"- Application started  ");


            string strGName = "";
            if (GCSettings.IsServerGC == true)
                strGName = "server";
            else
                strGName = "workstation";
            MyLogger.LogInformation($"The {strGName} garbage collector is running.");

            app.UseResponseCompression();
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
