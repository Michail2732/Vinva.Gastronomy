using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Vinva.Gastronomy.Identity.Domain.Entities;

namespace Vinva.Gastronomy.Identity.Persistence.Specifications
{
    public class ByRefreshTokenSpec : Specification<UserTokens>
    {
        public ByRefreshTokenSpec(string refreshToken)
        {
            Query.Where(a => a.RefreshToken == refreshToken);
        }
    }
}
