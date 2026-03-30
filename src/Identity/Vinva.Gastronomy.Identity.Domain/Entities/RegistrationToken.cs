using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Identity.Domain.Constants;
using Vinva.Gastronomy.Identity.Domain.Exceptions;

namespace Vinva.Gastronomy.Identity.Domain.Entities
{
    public class RegistrationToken : EntityOfT<Guid>
    {        
        public DateTimeOffset ExpiresAt { get; private set; }
        public string PasswordHash { get; private set; }        
        public string Login { get; private set; }
        public string Email { get; private set; }
        
        public DateTimeOffset? LetterSentTimestap { get; private set; }
        public bool IsConfirmLetterSent => LetterSentTimestap.HasValue;

        public DateTimeOffset? ConfirmCompleateTimestap { get; private set; }
        public bool IsConfirmCompleate => ConfirmCompleateTimestap.HasValue;

        public RegistrationToken(DateTimeOffset expiresAt,
            string passwordHash, string login, string email, 
            DateTimeOffset? letterSentTimestap = null,
            DateTimeOffset? confirmCompleateTimestap = null)
        {
            ExpiresAt = expiresAt;
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            Login = login ?? throw new ArgumentNullException(nameof(login));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            LetterSentTimestap = letterSentTimestap;
            ConfirmCompleateTimestap = confirmCompleateTimestap;            
        }

        public bool IsExpires(TimeProvider timeProvider)
        {
            return timeProvider.GetUtcNow() > ExpiresAt;
        }


        public void LetterSent(TimeProvider timeProvider)
        {
            if (IsConfirmLetterSent)
                throw new IdentityDomainException(IdentityErrorMessages.ConfirmMessageAlreadySent(Id));
            var utcNow = timeProvider.GetUtcNow();
            LetterSentTimestap = utcNow;            
        }


        public void KeepAlive(string passwordHash, TimeSpan expiresDelta, TimeProvider timeProvider )
        {
            var utcNow = timeProvider.GetUtcNow();

            if (IsConfirmCompleate)
                throw new IdentityDomainException(IdentityErrorMessages.RegistrationTokenAlreadyComplete(Id));
            if (expiresDelta.TotalSeconds <= 0)
                throw new ArgumentException($"'{nameof(expiresDelta)}' must be positive.", nameof(passwordHash));
            if (string.IsNullOrEmpty(passwordHash))
            {
                throw new ArgumentException($"'{nameof(passwordHash)}' cannot be null or empty.", nameof(passwordHash));
            }            
            
            PasswordHash = passwordHash;
            ExpiresAt = utcNow + expiresDelta;            
        }

        public void Confirm(TimeProvider timeProvider)
        {
            var utcNow = timeProvider.GetUtcNow();
            ConfirmCompleateTimestap = utcNow;            
        }
    }
}
