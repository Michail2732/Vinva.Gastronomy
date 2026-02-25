using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Identity.Application.Common;
using Vinva.Gastronomy.Identity.Domain.Entities;
using Vinva.Gastronomy.Identity.Domain.Services;

namespace Vinva.Gastronomy.Identity.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtTokenConfig _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public TokenService(IOptions<JwtTokenConfig> jwtConfig)
        {
            _jwtSettings = jwtConfig.Value
                        ?? throw new ArgumentNullException(nameof(jwtConfig));

            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(_jwtSettings.ClockSkewMinutes)
            };
        }

        public Task<string> GenerateAccessTokenAsync(User user, CancellationToken ct = default)
        {

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Name, user.Login),
                new("user_state", user.State.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),            
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.AccessTokenExpirationHours),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Task.FromResult(tokenHandler.WriteToken(token));
        }

        //todo: repair stuff
        public Task<string> GenerateRefreshTokenAsync(User user, CancellationToken ct = default)
        {            
            var refreshToken = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
            return Task.FromResult(refreshToken);
        }

        public Task<UserTokenPrincipals?> ValidateTokenAsync(string token, CancellationToken ct = default)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out _);
                var userTokenPrincipal = new UserTokenPrincipals(principal);
                return Task.FromResult<UserTokenPrincipals?>(userTokenPrincipal);
            }
            catch
            {
                return Task.FromResult<UserTokenPrincipals?>(null);
            }
        }

        public Task<DateTimeOffset> GetTokenExpirationAsync(string token, CancellationToken ct = default)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jsonToken = tokenHandler.ReadJwtToken(token);
                return Task.FromResult(new DateTimeOffset(jsonToken.ValidTo));
            }
            catch
            {
                return Task.FromResult(DateTimeOffset.MinValue);
            }
        }        
    }
}
