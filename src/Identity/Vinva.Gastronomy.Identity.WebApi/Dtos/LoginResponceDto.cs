using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Entities;

namespace Vinva.Gastronomy.Identity.WebApi.Dtos
{
    public record LoginResponceDto
    {
        /// <summary>
        /// Токен доступа
        /// </summary>
        public required string AccessToken { get; init; }        
        
        /// <summary>
        /// Время истечения токена
        /// </summary>
        public required DateTimeOffset ExpiresAt { get; init; }
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public required string Login { get; init; }
        /// <summary>
        /// Роль пользователя
        /// </summary>
        public required UserRole[] Roles { get; init; }
    }
}
