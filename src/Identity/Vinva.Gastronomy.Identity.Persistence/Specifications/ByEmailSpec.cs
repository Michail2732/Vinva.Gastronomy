using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Vinva.Gastronomy.Identity.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Vinva.Gastronomy.Identity.Persistence.Specifications
{
    public class ByEmailSpec : Specification<User>
    {
        public ByEmailSpec(string email)
        {
            Query.Where(a => a.Email == email);
        }

    }
}
