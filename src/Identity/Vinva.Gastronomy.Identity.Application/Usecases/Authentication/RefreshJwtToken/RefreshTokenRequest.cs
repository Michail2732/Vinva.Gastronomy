using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Authentication.RefreshJwtToken
{
    public readonly record struct RefreshTokenRequest : IRequest<RefreshTokenResponce>
    {
        /// <summary>
        /// Токен обновления
        /// </summary>
        public string RefreshToken { get; init; }
    }
}
