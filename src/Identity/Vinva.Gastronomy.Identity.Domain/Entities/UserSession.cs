using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common;

namespace Vinva.Gastronomy.Identity.Domain.Entities
{
    public class UserSession : EntityOfT<Guid>
    {
        public Guid UserId { get; private set; }
        public string? DeviceId { get; set; }
        public DateTimeOffset ExpiresAt { get; init; }
        public DateTimeOffset? RevokedAt { get; private set; }          
        public string? RefreshTokenHash { get; set; }

#pragma warning disable CS8618
        private UserSession() : base(GetDefaultGuid()) { }
#pragma warning restore CS8618 

        public UserSession(Guid id, Guid userId, string? deviceId = null) : base(id)
        {
            UserId = userId;
            DeviceId = deviceId;
        }

        public UserSession(Guid userId, string? deviceId = null) : base(GenerateGuid())
        {
            UserId = userId;
            DeviceId = deviceId;
        }

        public bool IsActive(DateTimeOffset now)
        {
            return RevokedAt is null && ExpiresAt > now;
        }

        public void Revoke(DateTimeOffset now)
        {
            RevokedAt = now;
        }
    }
}