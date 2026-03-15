using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Common.Entities;

namespace Vinva.Gastronomy.Identity.Domain.Entities
{
    public class UserTokens : Entity, IEquatable<UserTokens?>
    {
        public Guid UserId { get; private set; }
        public string? AccessToken { get; private set; }
        public string? RefreshToken { get; private set; }
        // todo: Походу ненужно 
        public DateTimeOffset? ExpiresAt { get; private set; }


        public UserTokens(Guid userId, string accessToken, string refreshToken, DateTimeOffset? expiresAt)
        {
            UserId = userId;
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            RefreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
            ExpiresAt = expiresAt;
        }

        public void SetNewToken(string accessToken, string refreshToken, DateTimeOffset expiresAt)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpiresAt = expiresAt;
        }

        public void ResetToken()
        {
            AccessToken = null;
            RefreshToken = null;
            ExpiresAt = null;
        }


        public override bool Equals(object? obj)
        {
            return Equals(obj as UserTokens);
        }

        public bool Equals(UserTokens? other)
        {
            return other is not null &&
                   UserId.Equals(other.UserId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserId);
        }

        public override bool Equals(IEntity? other)
        {
            return Equals(other as UserTokens);
        }
    }
}
