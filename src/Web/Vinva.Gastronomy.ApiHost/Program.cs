using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using Vinva.Gastronomy.ApiHost.Swagger;
using Vinva.Gastronomy.Common.Modularity;
using Vinva.Gastronomy.Common.Modularity.MediatR;
using Vinva.Gastronomy.Common.Services;
using Vinva.Gastronomy.Identity.WebApi;
using Vinva.Gastronomy.Media.WebApi;
using Vinva.Gastronomy.Recipes.WebApi;

//todo: добавить RateLimit для EndPoint'ов и контроллеров
//todo: проверить генерируемые EF запросы
var builder = WebApplication.CreateBuilder(args);
var moduleContext = new WebModuleContext(
    builder.Services,
    builder.Configuration,
    builder.Environment);
moduleContext.ConfigureSwagger(options =>
{
    options.UseInlineDefinitionsForEnums();
});
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
        var actionName = apiDesc.ActionDescriptor.RouteValues["action"];
        var controllerName = apiDesc.ActionDescriptor.RouteValues["controller"];
        return controllerName + actionName;
    });
    opt.SchemaFilter<EnumSchemaFilter>();
});

moduleLoader.RegisterServices(moduleContext);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton(GuidProvider.Instance);

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(policy =>
    {
        var allowedOrigins = app.Configuration.GetSection("AllowedOrigins").Get<string[]>()
        ?? throw new Exception("Not found section 'AllowedOrigins' in appsettings.json");
        policy.WithOrigins(allowedOrigins)
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();

    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await moduleLoader.InitializeAsync(app);

app.Run();
