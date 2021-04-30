using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using OrganizationService.Infrastructure.Queries;

namespace OrganizationService.Application
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
            services.AddLogging();
            services.AddMediatR();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrganizationService", Version = "v1" });
            });
            services.AddScoped<IQueries>(serviceProvider => new Queries(ApplicationExtensions.GetSqlServerConnStr()));
            services.AddPersistence(ApplicationExtensions.GetSqlServerConnStr());
            services.AddControllers(options => options.Filters.Add(typeof(HttpGlobalExceptionFilter)))
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                    .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrganizationService v1");
            });
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}