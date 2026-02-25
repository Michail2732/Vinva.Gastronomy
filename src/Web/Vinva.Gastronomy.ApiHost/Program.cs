using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Modularity;
using Vinva.Gastronomy.Identity.Persistence;
using Vinva.Gastronomy.Identity.WebApi;
using Vinva.Gastronomy.Recipes.Application.Common;
using Vinva.Gastronomy.Recipes.Persistence;
using Vinva.Gastronomy.Recipes.WebApi;

var builder = WebApplication.CreateBuilder(args);

var moduleLoader = new WebModulesLoader(new List<IWebModule>
{
    new IdentityWebModule(),
    new RecipeWebModule()
});
moduleLoader.RegisterServices(builder, builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await moduleLoader.InitializeAsync(app);

app.Run();
