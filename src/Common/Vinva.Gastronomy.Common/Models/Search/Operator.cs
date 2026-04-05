using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Models.Search
{
    public enum Operator
    {
        /// <summary>
        /// Less
        /// </summary>
        Less,
        /// <summary>
        /// LessOrEqual
        /// </summary>
        LessOrEqual,
        /// <summary>
        /// Larger
        /// </summary>
        Larger,
        /// <summary>
        /// LargerOrEqual
        /// </summary>
        LargerOrEqual,
        /// <summary>
        /// Equals
        /// </summary>
        Equals,
        /// <summary>
        /// NotEquals
        /// </summary>
        NotEquals,
        /// <summary>
        /// StartWith
        /// </summary>
        StartWith,
        /// <summary>
        /// EndWith
        /// </summary>
        EndWith,
        /// <summary>
        /// Contains
        /// </summary>
        Contains
    }
}
