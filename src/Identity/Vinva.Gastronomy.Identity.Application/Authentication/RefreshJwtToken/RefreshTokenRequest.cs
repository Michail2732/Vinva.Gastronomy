using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Identity.Application.Authentication.RefreshJwtToken
{
    public readonly record struct RefreshTokenRequest : IRequest<Result<RefreshTokenResponce>>
    {
        /// <summary>
        /// Токен обновления
        /// </summary>
        public string RefreshToken { get; init; }
    }
}
