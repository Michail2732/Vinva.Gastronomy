using Microsoft.Extensions.Hosting;

namespace Vinva.Gastronomy.Common.Modularity
{
    public interface IWebModule
    {
        IHostApplicationBuilder Add(WebModuleContext context);

    }
}
