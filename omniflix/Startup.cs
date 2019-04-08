using System.Net.Http;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TMDbLib.Client;

namespace omniflix
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            this._Configuration = configuration;
            this._Environment = environment;
        }

        public IConfiguration _Configuration { get; }
        public IHostingEnvironment _Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.AddHttpClient();
            services.AddTransient<Common.Parsers.Movies.MovieParser>();
            services.AddTransient<Common.Parsers.GoogleDrive.VideoInfoParser>();
            // Add Google Drive
            var gdriveKey = this._Configuration.GetValue<string>("GoogleDriveSA");
            services.AddSingleton<DriveService>(new DriveService(
                new BaseClientService.Initializer()
                {
                    HttpClientInitializer = GoogleCredential.FromJson(gdriveKey)
                        .CreateScoped(new string[1] { DriveService.Scope.DriveReadonly })
                        .UnderlyingCredential as ServiceAccountCredential,
                    ApplicationName = "omniflix",
                })
            );
            services.AddSingleton<Common.TokenManager>();
            services.AddSingleton<Common.Parsers.GoogleDrive.QualityMapper>();
            // TMDB/TVDB/OMDB Clients
            services.AddTransient<TMDbClient>();
            services.AddTransient<Common.Services.OmdbService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}