using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using TrisvagoHotels.Api.Configuration;

namespace TrisvagoHotels.Host {
    public class Startup {
        private IConfiguration Configuration { get; }

        private IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment) {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) =>
            ApiConfiguration.ConfigureServices(services, Environment, Configuration)
                .AddCustomServices()
                .AddOpenApi()
                .AddCors(options => options.AddPolicy("CorsApi",
                    builder => builder.WithOrigins(
                            "http://localhost:4200",
                            "http://localhost:3000",
                            "http://localhost:8080")
                        .AllowAnyHeader().AllowAnyMethod()))
                .AddEntityFrameworkCore(Configuration)
                .AddMediatr()
                .AddCustomHealthChecks(Configuration)
                /*.AddHealthChecksUI(setup => { setup.SetEvaluationTimeInSeconds(10); })
                .AddSqliteStorage(Configuration.GetConnectionString("HealthChecksDatabase"))*/
            ;

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            ApiConfiguration.Configure(app, host => host
                    .UseIf(env.IsDevelopment(), appBuilder => appBuilder.UseDeveloperExceptionPage())
                    .UseSwagger()
                    .UseIf(env.IsDevelopment(), appBuilder => appBuilder.UseSwaggerUI(setup => {
                        setup.SwaggerEndpoint("/swagger/v1/swagger.json", nameof(TrisvagoHotels));
                    }))
                    .UseHttpsRedirection()
                    /*.UseStaticFiles(new StaticFileOptions {
                        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
                        RequestPath = new PathString("/Resources")
                    })*/
                    .UseCustomHealthchecks()
                    .UseRouting()
                    .UseCors("CorsApi")
                    .UseHeaderDiagnostics()
                    .UseHealthChecksUI()
            );
        }
    }
}