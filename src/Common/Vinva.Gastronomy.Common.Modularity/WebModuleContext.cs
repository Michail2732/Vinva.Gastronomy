using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Modularity
{
    public class WebModuleContext
    {
        private readonly List<Assembly> _applicationParts;


        public IServiceCollection Services { get; }
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }     
        
        public Action<MvcOptions> MvcConfigurations { get; private set; }
        public Action<RouteOptions> RouteConfigurations { get; private set; }
        public Action<SwaggerGenOptions> SwaggerConfigurations { get; private set; }
        public Action<MediatRServiceConfiguration> MediatRConfigurations { get; private set; }
        public IReadOnlyList<Assembly> ApplicationParts => _applicationParts;

        public WebModuleContext(IServiceCollection serviceCollection,
            IConfiguration configuration, IWebHostEnvironment environment)
        {
            Services = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));

            MvcConfigurations = op => { };
            RouteConfigurations = op => { };
            SwaggerConfigurations = op => { };
            MediatRConfigurations = op => { };

            _applicationParts = new List<Assembly>();
        }

        public void ConfigureMvc(Action<MvcOptions> options)
        {
            MvcConfigurations += options;            
        }

        public void ConfigureRoute(Action<RouteOptions> options)
        {
            RouteConfigurations += options;
        }

        public void ConfigureSwagger(Action<SwaggerGenOptions> options)
        {
            SwaggerConfigurations += options;
        }

        public void ConfigureMediatR(Action<MediatRServiceConfiguration> options)
        {
            MediatRConfigurations += options;
        }   
        
        public void AddApplicationPart(Assembly assembly)
        {
            _applicationParts.Add(assembly);
        }
    }
}
