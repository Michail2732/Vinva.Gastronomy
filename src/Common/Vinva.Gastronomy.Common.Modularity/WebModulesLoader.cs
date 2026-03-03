using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Modularity.MediatR;

namespace Vinva.Gastronomy.Common.Modularity
{
    public class WebModulesLoader
    {        
        private readonly IList<IWebModule> _modules;

        public WebModulesLoader(IEnumerable<IWebModule> modules)
        {            
            _modules = modules?.OrderBy(a => a.Order).ToList() ?? throw new ArgumentNullException(nameof(modules));
        }


        public void RegisterServices(WebApplicationBuilder webAppBuilder, IConfiguration config)
        {
            var collection = webAppBuilder.Services;

            Assembly[] assemblies = _modules.SelectMany(a => a.Assemblies).ToArray();

            collection.AddMediatR(cfg =>
            {                
                cfg.RegisterServicesFromAssemblies(assemblies);
                cfg.AddOpenBehavior(typeof(LoggingMediatRBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidationMediatRBehavior<,>));
            });

            collection.AddValidatorsFromAssemblies(assemblies);

            var mvcBuilder = collection.AddControllers();

            collection.AddRouting();            

            foreach (var module in _modules)
            {
                module.RegisterServices(webAppBuilder, config);
                foreach (var moduleAssembly in module.Assemblies)
                {
                    mvcBuilder.AddApplicationPart(moduleAssembly);
                }
            }
        }

        public async Task InitializeAsync(WebApplication webApp, CancellationToken ct = default)
        {
            foreach (var module in _modules)
            {
                await module.InitializeAsync(webApp, ct);                
            }
        }

    }
}
