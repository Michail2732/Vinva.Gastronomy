using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Models.Search
{
    public enum Operator
    {
        Less,
        LessOrEqual, 
        Larger,
        LargerOrEqual,
        Equals,
        NotEquals,
        StartWith,
        EndWith,
        Contains
    }
}
