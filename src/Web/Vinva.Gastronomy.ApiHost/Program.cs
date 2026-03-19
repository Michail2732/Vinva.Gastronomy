using Microsoft.OpenApi.Models;
using Vinva.Gastronomy.Common.Modularity;
using Vinva.Gastronomy.Common.Modularity.MediatR;
using Vinva.Gastronomy.Common.Services;
using Vinva.Gastronomy.Identity.WebApi;
using Vinva.Gastronomy.Media.WebApi;
using Vinva.Gastronomy.Recipes.WebApi;

var builder = WebApplication.CreateBuilder(args);
var moduleContext = new WebModuleContext(
    builder.Services,
    builder.Configuration,
    builder.Environment);

var moduleLoader = new WebModulesLoader(new List<IWebModule>
{
    new IdentityWebModule(),
    new RecipeWebModule(),
    new MediaWebModule()
});
moduleContext.ConfigureMediatR(cfg =>
{
    cfg.AddOpenBehavior(typeof(LoggingMediatRBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidationMediatRBehavior<,>));
});
moduleContext.ConfigureSwagger(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gastronomy",
        Version = "v1",
        Description = "Gastronomy API"
    });
    opt.CustomOperationIds(apiDesc =>
    {
        // Берем имя метода (GetRecipes, Login, Logout)
        var actionName = apiDesc.ActionDescriptor.RouteValues["action"];
        var controllerName = apiDesc.ActionDescriptor.RouteValues["controller"];

        // Приводим к camelCase (getRecipes, login, logout)
        return controllerName + actionName;
    });
});

moduleLoader.RegisterServices(moduleContext);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton(GuidProvider.Instance);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await moduleLoader.InitializeAsync(app);

app.Run();
