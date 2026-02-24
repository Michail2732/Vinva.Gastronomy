using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Authentication.Logout
{
    public readonly record struct LogoutRequest : IRequest<Result>
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; init; }

    }
}
