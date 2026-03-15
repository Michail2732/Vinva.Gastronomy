using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Services;
using Vinva.Gastronomy.Identity.Application.Common;
using Vinva.Gastronomy.Identity.Domain.Entities;
using Vinva.Gastronomy.Identity.Domain.Services;

namespace Vinva.Gastronomy.Identity.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtTokenConfig _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly GuidProvider _guidProvider;
        private readonly TimeProvider _timeProvider;

        public TokenService(IOptions<JwtTokenConfig> jwtConfig, TimeProvider timeProvider, GuidProvider guidProvider)
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
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            _guidProvider = guidProvider ?? throw new ArgumentNullException(nameof(guidProvider));
        }

        public Task<string> GenerateAccessTokenAsync(User user, CancellationToken ct = default)
        {

            var claims = UserClaims.CreateClaims(user);            
            claims.Add(new(JwtRegisteredClaimNames.Jti, _guidProvider.Generate().ToString()));
            claims.Add(new(JwtRegisteredClaimNames.Iat, _timeProvider.GetUtcNow().ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));
            

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
            var refreshToken = _guidProvider.Generate().ToString("N") + _guidProvider.Generate().ToString("N");
            return Task.FromResult(refreshToken);
        }

        public Task<UserClaims?> ValidateTokenAsync(string token, CancellationToken ct = default)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out _);
                var userTokenPrincipal = new UserClaims(principal);
                return Task.FromResult<UserClaims?>(userTokenPrincipal);
            }
            catch
            {
                return Task.FromResult<UserClaims?>(null);
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
