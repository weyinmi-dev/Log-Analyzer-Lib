using Log_Analyzer_API.Data.AppDbContext;
using Log_Analyzer_API.Implementation;
using LogAnalyzerLibrary.Abstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Log_Analyzer_API.ServiceExtensions
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LogDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("default")));

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
            
            services.AddScoped<IUserStoriesServices, UserStoriesServices>();
        }

       
    }
}
