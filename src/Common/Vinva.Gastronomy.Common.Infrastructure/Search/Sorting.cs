using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Infrastructure.Filters
{
    public class Sorting
    {
        public required string Property { get; init; }
        public SortDirection Direction { get; init; }
    }
}
