using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalleriesServer.Models;
using GalleriesServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

namespace GalleriesServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1); 
            services.AddCors();

            services.AddDbContext<GalleriesDbContext>(options =>
                            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<OwnerService>();
            services.AddTransient<MediaContainerService>();
            services.AddTransient<MediaItemService>();
            services.AddScoped<IImageStore, ImageStore>();

            services.AddScoped<IMediaStorage>(factory =>
            {
                var settings = new AzureBlobSettings(
                    storageAccount: Configuration["Blob_StorageAccount"],
                    storageKey: Configuration["Blob_StorageKey"]);
                var storageAccount = new CloudStorageAccount(new StorageCredentials(settings.StorageAccount, settings.StorageKey), false);
                return new AzureBlobStorage(storageAccount, settings);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
                builder.WithOrigins("http://localhost:8080").AllowAnyMethod().AllowAnyHeader());
            app.UseCors(builder =>
                builder.WithOrigins("https://galleries247b.azurewebsites.net"));

            app.UseMvc();
            app.UseDefaultFiles();
            app.UseStaticFiles();

        }
    }
}
