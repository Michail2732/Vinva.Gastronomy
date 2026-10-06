using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Vinva.Gastronomy.Common.Modularity
{
    public class ModuleServiceContext
    {
        public IServiceCollection Services { get; }
        public IConfiguration Configuration { get; }
        public IHostEnvironment Environment { get; }

        public ModuleServiceContext(
            IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment environment)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }
    }
}