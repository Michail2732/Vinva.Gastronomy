using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Vinva.Gastronomy.Identity.Domain.Entities;

namespace Vinva.Gastronomy.Identity.Persistence.Specifications
{
    public class ByLoginAndPasswordHashSpec: Specification<User>
    {
        public ByLoginAndPasswordHashSpec(string login)
        {
            Query.Where(a => a.Login == login);
        }
    }
}
