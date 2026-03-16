using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Authentication.Login
{
    public readonly record struct LoginRequest : IRequest<LoginResponce>
    {
        /// <summary>
        ///     Логин.
        /// </summary>
        public required string Login { get; init; }
        /// <summary>
        ///     Пароль.
        /// </summary>
        public required string Password { get; init; }        
    }
}
