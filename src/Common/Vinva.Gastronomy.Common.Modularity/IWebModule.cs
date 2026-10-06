namespace Vinva.Gastronomy.Common.Modularity
{
    public interface IModule
    {
        string ModuleName { get; }
        IReadOnlyCollection<Type> DependsOn { get; }
        void RegisterServices(ModuleServiceContext context);
        Task InitializeAsync(ModuleInitializationContext context, CancellationToken ct = default);
    }

    public interface IWebModule : IModule
    {
        void RegisterWeb(ModuleWebContext context);
    }
}
