using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Identity.Domain.Entities;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Authentication.Login
{
    public readonly record struct LoginResponce
    {
        /// <summary>
        /// Токен доступа
        /// </summary>
        public required string AccessToken { get; init; }
        /// <summary>
        /// Токен обновления
        /// </summary>
        public required string RefreshToken { get; init; }
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
        public required UserRole Role { get; init; }
    }
}
