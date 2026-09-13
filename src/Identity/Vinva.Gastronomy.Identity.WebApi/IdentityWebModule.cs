using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Exceptions;
using Vinva.Gastronomy.Common.Modularity;
using Vinva.Gastronomy.Common.Services;
using Vinva.Gastronomy.Identity.Application.Common;
using Vinva.Gastronomy.Identity.Application.Services;
using Vinva.Gastronomy.Identity.Domain.Services;
using Vinva.Gastronomy.Identity.Persistence;
using Vinva.Gastronomy.Identity.WebApi.Controllers;
using Vinva.Gastronomy.Identity.WebApi.Filters;
using Vinva.Gastronomy.Identity.WebApi.Services;

namespace Vinva.Gastronomy.Identity.WebApi
{
    public class IdentityWebModule : IWebModule
    {
        public string ModuleName => "Identity";

        public int Order => 0;        

        public async Task InitializeAsync(WebApplication webApp, CancellationToken ct = default)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<IdentityDbContext>()
                    ?? throw new ArgumentNullException($"Not found {nameof(IdentityDbContext)}");
                await context!.Database.MigrateAsync(ct);

                var sessionService = scope.ServiceProvider.GetRequiredService<ISessionService>();
                var sessions = await context.Sessions.AsNoTracking().ToListAsync(ct);
                foreach (var session in sessions)
                    sessionService.AddOrUpdateSession(session);
            }
        }

        public void RegisterServices(WebModuleContext context)
        {
            context.ConfigureMediatR(opt =>
            {
                opt.RegisterServicesFromAssemblies(typeof(JwtTokenConfig).Assembly);
            });

            context.ConfigureMvc(opt =>
            {
                opt.Filters.Add<UserStateFilter>();
            });
            context.AddApplicationPart(GetType().Assembly);
            var services = context.Services;

            services.AddValidatorsFromAssembly(typeof(JwtTokenConfig).Assembly);
            services.AddHttpContextAccessor();
            services.AddSingleton<IUserContext, UserContext>();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<IHashService, HashService>();
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddSingleton<ISessionService, SessionService>();
            services.AddSingleton(TimeProvider.System);
            services.AddSingleton<IRegistrationService, RegistrationService>();
            services.AddSingleton<AuthCookieOptionsFactory>();            
            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnectionString"));
            });


            Func<IConfiguration, IConfigurationSection> identityConfigSpec =
                context.Environment.IsDevelopment()
                ? a => a.GetSection("IdentityModule").GetSection("Development")
                : a => a.GetSection("IdentityModule").GetSection("Production");            

            services.Configure<JwtTokenConfig>(identityConfigSpec(context.Configuration).GetSection("JwtTokenConfig"));
            services.Configure<PasswordConfig>(identityConfigSpec(context.Configuration).GetSection("PasswordConfig"));
            services.Configure<RegisterSmtpConfig>(identityConfigSpec(context.Configuration).GetSection("RegisterSmtpConfig"));

            var jwtConfig = identityConfigSpec(context.Configuration).GetSection("JwtTokenConfig").Get<JwtTokenConfig>()
                ?? throw new DomainException("Couldnt read jwt config section");
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    JwtBearerConfiguration.Configure(
                        options,
                        jwtConfig,
                        context.Environment.IsDevelopment());
                });

            context.ConfigureSwagger(opt =>
            {
                var sequrityScheme = new OpenApiSecurityScheme
                {
                    Description = "Cookie based configuration",
                    Name = AuthenticationController.ACCESS_TOKEN_KEY,
                    In = ParameterLocation.Cookie,
                    Type = SecuritySchemeType.ApiKey                    
                };                
                opt.AddSecurityDefinition("cookieAuth", sequrityScheme);

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "cookieAuth"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            
            services.AddAuthorization();
        }
    }
}
