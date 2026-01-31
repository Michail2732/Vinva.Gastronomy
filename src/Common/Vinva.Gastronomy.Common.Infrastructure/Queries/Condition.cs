using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Infrastructure.Queries
{
    public class Condition
    {
        public string? Logic { get; set; }        

        public string? Field { get; set; }

        public string? Operator { get; set; }

        public object? Value { get; set; }
    }
}
