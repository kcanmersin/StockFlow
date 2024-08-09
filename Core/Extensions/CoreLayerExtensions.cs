using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Core.Data;
using Core.Service.JWT;
using MediatR;
using System.Reflection;
using FluentValidation;

namespace Core.Extensions
{
    public static class CoreLayerExtensions
    {
        public static IServiceCollection LoadCoreLayerExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IJwtService, JwtService>();
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            services.AddSingleton(jwtSettings);
            
            //add mediatr
            services.AddMediatR(Assembly.GetExecutingAssembly());
            //add currency
            // Add CurrencyConversionService
            services.AddHttpClient<CurrencyConversionService>();
            //add validators from assembly
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());



            return services;
        }
    }
}
