using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using Vinva.Gastronomy.Common.Modularity.Filters;
using Vinva.Gastronomy.Common.Modularity.MediatR;

namespace Vinva.Gastronomy.Common.Modularity
{
    public class WebModulesLoader
    {
        private readonly IReadOnlyList<IWebModule> _modules;

        public WebModulesLoader(IEnumerable<IWebModule> modules)
        {
            var moduleList = modules?.ToList() ?? throw new ArgumentNullException(nameof(modules));
            ValidateModules(moduleList);
            _modules = OrderModules(moduleList);
        }

        public void RegisterServices(ModuleServiceContext serviceContext, ModuleWebContext webContext)
        {
            ArgumentNullException.ThrowIfNull(serviceContext);
            ArgumentNullException.ThrowIfNull(webContext);

            serviceContext.Services.AddSingleton<DomainExceptionFilter>();
            webContext.ConfigureMvc(options => options.Filters.Add<DomainExceptionFilter>());

            foreach (var module in _modules)
            {
                module.RegisterServices(serviceContext);
                module.RegisterWeb(webContext);
            }

            var mvcBuilder = serviceContext.Services.AddControllers(webContext.MvcConfigurations)
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            serviceContext.Services.AddRouting(webContext.RouteConfigurations);
            serviceContext.Services.AddSwaggerGen(webContext.SwaggerConfigurations);
            serviceContext.Services.AddMediatR(webContext.MediatRConfigurations);

            foreach (var applicationPart in webContext.ApplicationParts)
                mvcBuilder.AddApplicationPart(applicationPart);
        }

        public async Task InitializeAsync(ModuleInitializationContext context, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(context);
            foreach (var module in _modules)
                await module.InitializeAsync(context, ct);
        }

        private static void ValidateModules(IReadOnlyCollection<IWebModule> modules)
        {
            var duplicate = modules
                .GroupBy(module => module.ModuleName, StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault(group => group.Count() > 1);
            if (duplicate is not null)
                throw new InvalidOperationException($"Duplicate module name '{duplicate.Key}'.");

            var moduleTypes = modules.Select(module => module.GetType()).ToHashSet();
            foreach (var module in modules)
            {
                foreach (var dependency in module.DependsOn)
                {
                    if (!moduleTypes.Contains(dependency))
                        throw new InvalidOperationException($"Module '{module.ModuleName}' depends on '{dependency.Name}', but it is not registered.");
                }
            }
        }

        private static IReadOnlyList<IWebModule> OrderModules(IReadOnlyCollection<IWebModule> modules)
        {
            var remaining = modules.ToDictionary(module => module.GetType());
            var ordered = new List<IWebModule>(modules.Count);

            while (remaining.Count > 0)
            {
                var ready = remaining.Values
                    .Where(module => module.DependsOn.All(dependency => ordered.Any(item => item.GetType() == dependency)))
                    .ToList();

                if (ready.Count == 0)
                    throw new InvalidOperationException("Circular module dependency detected.");

                foreach (var module in ready.OrderBy(module => module.ModuleName, StringComparer.OrdinalIgnoreCase))
                {
                    ordered.Add(module);
                    remaining.Remove(module.GetType());
                }
            }

            return ordered;
        }
    }
}
