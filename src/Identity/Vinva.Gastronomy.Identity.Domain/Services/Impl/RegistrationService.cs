using System.Collections.Concurrent;
using Vinva.Gastronomy.Identity.Domain.Constants;
using Vinva.Gastronomy.Identity.Domain.Entities;
using Vinva.Gastronomy.Identity.Domain.Exceptions;

namespace Vinva.Gastronomy.Identity.Domain.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ConcurrentDictionary<Guid, RegistrationToken> _tokens;
        private readonly TimeProvider _timeProvider;
        private readonly TimeSpan _expiresDelta;

        public RegistrationService(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            _tokens = new ConcurrentDictionary<Guid, RegistrationToken>();
            _expiresDelta = TimeSpan.FromMinutes(5);            
        }

        public Task<RegistrationToken> GenerateTokenAsync(string email, string login, string passwordHash, CancellationToken ct = default)
        {
            var utcNow = _timeProvider.GetUtcNow();
            var expiresTime = utcNow + _expiresDelta;
            var regToken = new RegistrationToken(expiresTime, passwordHash, login, email);

            if (!_tokens.TryAdd(regToken.Id, regToken))
                throw new InvalidOperationException($"Could not add registration token '{regToken}'");

            return Task.FromResult(regToken);
        }        

        public Task<RegistrationToken?> PopTokenAsync(Guid id, CancellationToken ct = default)
        {
            var utcNow = _timeProvider.GetUtcNow();

            if (!_tokens.TryRemove(id, out var token))
                return Task.FromResult<RegistrationToken?>(null);            

            return Task.FromResult((RegistrationToken?)token);
        }
    }
}
