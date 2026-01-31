using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Infrastructure.Queries
{
    public class SearchQuery
    {
        public int Take { get; set; }
        public long Skip { get; set; }
        public OrderCriteria? Order { get; set; }
        public List<Condition> Conditions { get; set; }
    }
}
