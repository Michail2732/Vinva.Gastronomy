using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Identity.Domain.Entities;

namespace Vinva.Gastronomy.Identity.Domain.Services
{
    public interface IRegistrationService
    {
        Task<RegistrationToken> GenerateTokenAsync(string email, string login, string passwordHash, CancellationToken ct = default);
        Task UpdateTokenAsync(RegistrationToken token, string newPasswordHash, CancellationToken ct = default);
        /// <summary>
        /// Сгенерировать временную ссылку для подтверждения регистрации
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<Uri> GenerateRegisterConfirmLinkTokenAsync(Guid tokenId, CancellationToken ct = default);        
    }
}
