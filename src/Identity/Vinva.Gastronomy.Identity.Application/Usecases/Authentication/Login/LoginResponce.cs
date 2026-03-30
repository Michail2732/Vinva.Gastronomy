using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Identity.Application.Common;
using Vinva.Gastronomy.Identity.Domain.Entities;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Authentication.Login
{
    public record LoginResponce : UserInfoDto
    {
        /// <summary>
        /// Токен доступа
        /// </summary>
        public required string AccessToken { get; init; }
        /// <summary>
        /// Токен обновления
        /// </summary>
        public required string RefreshToken { get; init; }        
    }
}
