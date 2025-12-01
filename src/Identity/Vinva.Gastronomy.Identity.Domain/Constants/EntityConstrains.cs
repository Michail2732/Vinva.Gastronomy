using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Identity.Domain.Constants
{
    public class EntityConstrains
    {
        public static int MaxLoginLength { get; } = 100;
        public static int MinLoginLength { get; } = 5;
    }
}
