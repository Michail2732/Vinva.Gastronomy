using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Vinva.Gastronomy.Common.Modularity
{
    public interface IWebModule
    {
        string ModuleName { get; }
        int Order { get; }
        Assembly[] Assemblies { get; }        
        void RegisterServices(WebApplicationBuilder webAppBuilder, IConfiguration config);
        Task InitializeAsync(WebApplication webApp, CancellationToken ct = default);
    }
}
