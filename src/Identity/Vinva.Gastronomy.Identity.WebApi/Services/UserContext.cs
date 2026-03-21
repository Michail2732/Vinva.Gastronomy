using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Common.Exceptions;
using Vinva.Gastronomy.Common.Services;
using Vinva.Gastronomy.Identity.Domain.Entities;
using Vinva.Gastronomy.Identity.Domain.Services;

namespace Vinva.Gastronomy.Identity.WebApi.Services
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccess;
        private readonly UserClaimsParcer _parser;

        public bool IsAuthenticated => throw new NotImplementedException();

        public UserContext(IHttpContextAccessor httpContextAccess)
        {
            _httpContextAccess = httpContextAccess ?? throw new ArgumentNullException(nameof(httpContextAccess));
            _parser = new UserClaimsParcer(); 
        }                

        public Guid GetId()
        {
            var claims = GetClaimsPrincipal();
            return _parser.ParseId(claims);
        }

        public UserState GetState()
        {
            var claims = GetClaimsPrincipal();
            return _parser.ParseState(claims);
        }

        public UserRole[] GetRoles()
        {
            var claims = GetClaimsPrincipal();
            return _parser.ParseRoles(claims);
        }

        public string GetLogin()
        {
            var claims = GetClaimsPrincipal();
            return _parser.ParseLogin(claims);
        }

        public string GetEmail()
        {
            var claims = GetClaimsPrincipal();
            return _parser.ParseEmail(claims);
        }

        public UserInfo GetInfo()
        {
            var claims = GetClaimsPrincipal();
            return _parser.Parse(claims);
        }

        private ClaimsPrincipal GetClaimsPrincipal()
        {
            var context = _httpContextAccess.HttpContext 
                ?? throw new Exception("http context doesn't set");
            if (context.User.Identity?.IsAuthenticated != true)
                throw new UnauthorizedException();
            return context.User;
        }
    }
}
