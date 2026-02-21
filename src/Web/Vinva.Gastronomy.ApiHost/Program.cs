using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Identity.Persistence;
using Vinva.Gastronomy.Identity.WebApi;
using Vinva.Gastronomy.Recipes.Application.Common;
using Vinva.Gastronomy.Recipes.Persistence;
using Vinva.Gastronomy.Recipes.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cnfg =>
{
    cnfg.RegisterServicesFromAssembly(typeof(RecipeDto).Assembly);
});
builder.Services.AddControllers()
    .AddApplicationPart(typeof(RecipeWebModule).Assembly)
    .AddApplicationPart(typeof(IdentityWebModule).Assembly);
builder.Services.AddDbContext<RecipeDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString"));
});
builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString"));
});

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

app.Run();
