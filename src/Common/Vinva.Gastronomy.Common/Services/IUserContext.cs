using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Entities;

namespace Vinva.Gastronomy.Common.Services
{
    /// <summary>
    /// Контекст текущего пользователя
    /// </summary>
    public interface IUserContext
    {
        Guid GetId();
        UserState GetState();
        UserRole[] GetRoles();
        string GetLogin();
        string GetEmail();
        UserInfo GetInfo();
        bool IsAuthenticated { get; }
    }
}
