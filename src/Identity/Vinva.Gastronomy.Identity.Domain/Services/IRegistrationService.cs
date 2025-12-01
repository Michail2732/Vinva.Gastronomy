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
        Task<RegistrationToken> PopTokenAsync(Guid id, CancellationToken ct = default);
    }
}
