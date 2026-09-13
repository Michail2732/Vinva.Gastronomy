using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vinva.Gastronomy.Identity.Domain.Entities;

namespace Vinva.Gastronomy.Identity.Domain.Services
{
    public interface ISessionService
    {        
        UserSession GetSession(Guid userId);
        UserSession AddOrUpdateSession(UserSession session);
        void RemoveSession(Guid sessionId);
    }
}