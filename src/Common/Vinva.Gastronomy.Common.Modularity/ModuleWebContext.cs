using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Vinva.Gastronomy.Common.Modularity
{
    public sealed class ModuleWebContext
    {
        private readonly List<Assembly> _applicationParts = new();

        public Action<MvcOptions> MvcConfigurations { get; private set; } = _ => { };
        public Action<RouteOptions> RouteConfigurations { get; private set; } = _ => { };
        public Action<SwaggerGenOptions> SwaggerConfigurations { get; private set; } = _ => { };
        public Action<MediatRServiceConfiguration> MediatRConfigurations { get; private set; } = _ => { };
        public IReadOnlyList<Assembly> ApplicationParts => _applicationParts;

        public void ConfigureMvc(Action<MvcOptions> options) => MvcConfigurations += options;

        public void ConfigureRoute(Action<RouteOptions> options) => RouteConfigurations += options;

        public void ConfigureSwagger(Action<SwaggerGenOptions> options) => SwaggerConfigurations += options;

        public void ConfigureMediatR(Action<MediatRServiceConfiguration> options) => MediatRConfigurations += options;

        public void AddApplicationPart(Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(assembly);
            if (!_applicationParts.Contains(assembly))
                _applicationParts.Add(assembly);
        }
    }
}