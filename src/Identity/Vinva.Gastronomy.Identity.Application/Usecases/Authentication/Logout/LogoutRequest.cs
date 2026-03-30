using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Authentication.Logout
{
    public readonly record struct LogoutRequest : IRequest
    {
        /// <summary>
        /// Access токен пользователя
        /// </summary>
        public string AccessToken { get; init; }
        /// <summary>
        /// Refresh токен пользователя
        /// </summary>
        public string RefreshToken { get; init; }

    }
}
