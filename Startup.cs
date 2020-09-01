using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using VidlyBackend.Models;
using VidlyBackend.Profiles;
using VidlyBackend.Services;
using Newtonsoft.Json.Serialization;

namespace VidlyBackend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        string MyAllowSpecificOrigins = "MyAllowSpecificOrigins";
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("https://localhost")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod()
                                            .AllowAnyOrigin();
                    });
            });

            services.Configure<VidlyDatabaseSettings>(Configuration.GetSection("MongoDB"));
            services.AddSingleton<IVidlyDatabaseSettings>(sp => sp.GetRequiredService<IOptions<VidlyDatabaseSettings>>().Value);

            services.AddControllers().AddNewtonsoftJson(s =>
            {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSingleton<IDatabaseContext<Movie>, MongoCRUD<Movie>>();
            services.AddSingleton<IDatabaseContext<Genre>, MongoCRUD<Genre>>();
            services.AddSingleton<IDatabaseContext<Rental>, MongoCRUD<Rental>>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
