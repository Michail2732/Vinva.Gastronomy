using Ardalis.Specification;
using Vinva.Gastronomy.Identity.Domain.Entities;

namespace Vinva.Gastronomy.Identity.Persistence.Specifications
{
    public class ByAccessTokenSpec : Specification<UserTokens>
    {
        public ByAccessTokenSpec(string accessToken)
        {
            Query.Where(a => a.AccessToken == accessToken);
        }
    }
}
