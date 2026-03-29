using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Collections.Concurrent;
using Vinva.Gastronomy.Common.Exceptions;
using Vinva.Gastronomy.Identity.Domain.Constants;
using Vinva.Gastronomy.Identity.Domain.Entities;
using Vinva.Gastronomy.Identity.Domain.Exceptions;
using Vinva.Gastronomy.Identity.Domain.Services;
using Vinva.Gastronomy.Identity.WebApi.Controllers;

namespace Vinva.Gastronomy.Identity.WebApi.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccess;        
        private readonly TimeProvider _timeProvider;
        private readonly TimeSpan _expiresDelta;

        public RegistrationService(TimeProvider timeProvider, LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccess)
        {
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            _linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
            _httpContextAccess = httpContextAccess ?? throw new ArgumentNullException(nameof(httpContextAccess));            
            _expiresDelta = TimeSpan.FromMinutes(5);            
        }

        public async Task<Uri> GenerateRegisterConfirmLinkTokenAsync(Guid tokenId, CancellationToken ct = default)
        {
            var context = _httpContextAccess.HttpContext ?? throw new ArgumentException($"{nameof(IHttpContextAccessor.HttpContext)} is null");
            var uriStr = _linkGenerator.GetUriByAction(context, nameof(RegistrationController.RegisterConfirm),
                "Registration", new { tokenId = tokenId }) ?? throw new IdentityDomainException("Не удалось сформировать ссылку регистрации");
            return new Uri(uriStr);
        }

        public Task<RegistrationToken> GenerateTokenAsync(string email, string login, string passwordHash, CancellationToken ct = default)
        {
            var utcNow = _timeProvider.GetUtcNow();
            var expiresTime = utcNow + _expiresDelta;
            var regToken = new RegistrationToken(expiresTime, passwordHash, login, email);            
            return Task.FromResult(regToken);
        }

        public Task UpdateTokenAsync(RegistrationToken token, string newPasswordHash, CancellationToken ct = default)
        {
            var utcNow = _timeProvider.GetUtcNow();
            var expiresTime = utcNow + _expiresDelta;
            token.UpdateToken(newPasswordHash, expiresTime);
            return Task.CompletedTask;
        }
    }
}
