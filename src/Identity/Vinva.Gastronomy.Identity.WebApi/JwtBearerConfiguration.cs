using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Vinva.Gastronomy.Common.Services;
using Vinva.Gastronomy.Identity.Application.Common;
using Vinva.Gastronomy.Identity.Application.Services;
using Vinva.Gastronomy.Identity.Domain.Services;
using Vinva.Gastronomy.Identity.WebApi.Controllers;

namespace Vinva.Gastronomy.Identity.WebApi
{
    public static class JwtBearerConfiguration
    {
        public static void Configure(
            JwtBearerOptions options,
            JwtTokenConfig jwtConfig,
            bool includeErrorDetails)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (jwtConfig == null)
                throw new ArgumentNullException(nameof(jwtConfig));

            options.IncludeErrorDetails = includeErrorDetails;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
                ValidateIssuer = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtConfig.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(jwtConfig.ClockSkewMinutes)
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Cookies[AuthenticationController.ACCESS_TOKEN_KEY];
                    if (!string.IsNullOrEmpty(accessToken))
                        context.Token = accessToken;

                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    var userIdValue = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (!Guid.TryParse(userIdValue, out var userId))
                    {
                        context.Fail("User identifier is missing.");
                        return Task.CompletedTask;
                    }

                    var sessionIdValue = context.Principal?
                        .FindFirst(UserClaimsParcer.SessionIdType)?.Value;
                    if (!Guid.TryParse(sessionIdValue, out var sessionId))
                    {
                        context.Fail("Session identifier is missing.");
                        return Task.CompletedTask;
                    }

                    var sessionService = context.HttpContext.RequestServices
                        .GetRequiredService<ISessionService>();

                    try
                    {
                        var session = sessionService.GetSession(userId);
                        if (session.Id != sessionId || !session.IsActive(DateTimeOffset.UtcNow))
                            context.Fail("Session is revoked or expired.");
                    }
                    catch (KeyNotFoundException)
                    {
                        context.Fail("Session is missing.");
                    }

                    return Task.CompletedTask;
                }
            };
        }
    }
}