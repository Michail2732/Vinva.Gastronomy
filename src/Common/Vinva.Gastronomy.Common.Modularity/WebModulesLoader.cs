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
using Vinva.Gastronomy.Common.Modularity.Filters;
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


        public void RegisterServices(WebModuleContext context)
        {
            context.Services.AddSingleton<DomainExceptionFilter>();
            context.ConfigureMvc(opt =>
            {
                opt.Filters.Add<DomainExceptionFilter>();
            });
            foreach (var module in _modules.OrderBy(a => a.Order))
            {
                module.RegisterServices(context);                
            }            
            var mvcBuilder = context.Services.AddControllers(context.MvcConfigurations);
            context.Services.AddRouting(context.RouteConfigurations);            
            context.Services.AddSwaggerGen(context.SwaggerConfigurations);
            context.Services.AddMediatR(context.MediatRConfigurations);
            foreach (var applicationPart in context.ApplicationParts)
            {
                mvcBuilder.AddApplicationPart(applicationPart);
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
