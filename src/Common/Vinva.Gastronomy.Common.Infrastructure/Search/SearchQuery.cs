using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Infrastructure.Filters
{
    public class SearchQuery
    {
        public int Take { get; init; }
        public long Skip { get; init; }
        public Sorting? Sort { get; init; }
        public List<Condition>? Conditions { get; init; }
    }
}
