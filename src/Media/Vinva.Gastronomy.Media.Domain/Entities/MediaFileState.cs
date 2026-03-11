using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Media.Domain.Entities
{
    public enum MediaFileState
    {
        None,
        Uploading,
        Failed,
        Ready
    }
}
