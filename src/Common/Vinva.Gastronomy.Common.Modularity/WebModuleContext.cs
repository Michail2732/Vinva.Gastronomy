using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Modularity
{
    public class WebModuleContext
    {
        public IMvcBuilder MvcBuilder { get; init; }
        public MediatRServiceConfiguration MediatrConfig { get; init; }
        public IHostApplicationBuilder HostBuilder { get; init; }
    }
}
