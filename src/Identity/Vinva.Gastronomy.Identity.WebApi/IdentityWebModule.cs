using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Exceptions;
using Vinva.Gastronomy.Common.Modularity;
using Vinva.Gastronomy.Identity.Application.Common;
using Vinva.Gastronomy.Identity.Application.Services;
using Vinva.Gastronomy.Identity.Domain.Services;
using Vinva.Gastronomy.Identity.Persistence;
using Vinva.Gastronomy.Identity.WebApi.Services;

namespace Vinva.Gastronomy.Identity.WebApi
{
    public class IdentityWebModule : IWebModule
    {
        public string ModuleName => "Identity";

        public int Order => 0;

        public Assembly[] Assemblies { get; } = 
        {
            typeof(Domain.Entities.User).Assembly,
            typeof(Application.Common.JwtTokenConfig).Assembly,
            typeof(Persistence.IdentityDbContext).Assembly,
        };

        public Task InitializeAsync(WebApplication app, CancellationToken ct = default)
        {            
            return Task.CompletedTask;
        }

        public void RegisterServices(WebApplicationBuilder builder, IConfiguration config)
        {
            var services = builder.Services;
            services.AddHttpContextAccessor();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<IPasswordHashService, PasswordHashService>();
            services.AddSingleton<IRegistrationService, RegistrationService>();
            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("DefaultConnectionString"));
            });


            Func<IConfiguration, IConfigurationSection> identityConfigSpec =
                builder.Environment.IsDevelopment()
                ? a => a.GetSection("IdentityModule").GetSection("Development")
                : a => a.GetSection("IdentityModule").GetSection("Production");            

            services.Configure<JwtTokenConfig>(identityConfigSpec(config).GetSection("JwtTokenConfig"));
            services.Configure<PasswordConfig>(identityConfigSpec(config).GetSection("PasswordConfig"));
            services.Configure<RegisterSmtpConfig>(identityConfigSpec(config).GetSection("RegisterSmtpConfig"));

            var jwtConfig = identityConfigSpec(config).GetSection("JwtTokenConfig").Get<JwtTokenConfig>()
                ?? throw new DomainException("Couldnt read jwt config section");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    if (builder.Environment.IsDevelopment())
                    {
                        options.IncludeErrorDetails = true;
                    }                    
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
                        ValidateIssuer = true,
                        ValidIssuer = jwtConfig.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtConfig.Audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(jwtConfig.ClockSkewMinutes),                        
                    };                    
                });           

            services.AddAuthorization();
        }
    }
}
