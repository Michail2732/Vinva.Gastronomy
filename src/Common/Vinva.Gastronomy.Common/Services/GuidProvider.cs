using System;
using UUIDNext;

namespace Vinva.Gastronomy.Common.Services
{
    public class GuidProvider 
    {
        public static GuidProvider _instance = new GuidProvider();
        public static GuidProvider Instance => _instance;

        public virtual Guid Generate()
        {
            return Uuid.NewSequential();
        }

        public static void SetProvider(GuidProvider provider)
        {
            _instance = provider;
        }
    }

}
