using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Models.Search
{
    public class Condition
    {
        public required Logic Logic { get; init; }        

        public required string Field { get; init; }

        public required Operator Operator { get; init; }

        public required object Value { get; init; }
    }
}
