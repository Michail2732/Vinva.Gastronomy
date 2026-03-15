using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Identity.Domain.Entities;

namespace Vinva.Gastronomy.Identity.WebApi.Filters
{
    public class UserStateFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if ((user.Identity?.IsAuthenticated) != true)
                return;

            var state = user.FindFirst(UserClaims.StateType)?.Value;

            if (state == UserState.Blocked.ToString())
            {
                context.Result = new ObjectResult(new
                {
                    error = "Account Blocked",
                    message = "Ваш аккаунт заблокирован. Доступ запрещен."                    
                })
                {
                    StatusCode = 403
                };
            }
        }
    }
}
