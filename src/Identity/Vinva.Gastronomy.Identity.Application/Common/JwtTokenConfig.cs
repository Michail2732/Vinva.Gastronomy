using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Identity.Application.Common
{
    public sealed class JwtTokenConfig
    {
        public string SecretKey { get; init; } = string.Empty;
        public int AccessTokenExpirationHours { get; init; } = 24;
        public int RefreshTokenExpirationDays { get; init; } = 30;
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public int ClockSkewMinutes { get; init; } = 5;
    }
}
