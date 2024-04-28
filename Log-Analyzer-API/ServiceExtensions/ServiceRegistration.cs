using LogAnalyzerLibrary.Abstraction;
using LogAnalyzerLibrary.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Log_Analyzer_API.ServiceExtensions
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
            
            services.AddSingleton<ILogAnalyzerUserStories, LogAnalyzerUserStories>();

            services.AddSwaggerGen();

        }


    }
}
