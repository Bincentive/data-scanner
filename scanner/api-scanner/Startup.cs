using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using service_scanner.Helper;
using service_scanner.Helper.Interface;
using service_scanner.Service;
using service_scanner.Service.Interface;

namespace api_scanner
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var version = Configuration.GetSection("version");
            services.AddOpenApiDocument(document =>
            {
                document.DocumentName = "openapi";
                document.PostProcess = document =>
                {
                    document.Info.Version = "v"+ version.Value;
                    document.Info.Title = "Scanner";
                    document.Info.Description = "";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                    };
                };
            });
            services.AddHttpClient();
            services.AddScoped<IWeb3Service, Web3Service>();
            services.AddScoped<IWeb3Helper>(options => new Web3Helper(Configuration["RPC:ServerUrl"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
