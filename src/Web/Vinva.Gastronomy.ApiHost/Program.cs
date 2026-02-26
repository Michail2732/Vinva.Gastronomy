using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gastronomy",
        Version = "v1",
        Description = "Gastronomy API"
    });

    var sequrityScheme = new OpenApiSecurityScheme
    {
        Description = "Введите 'Bearer' [пробел] и затем ваш JWT токен в поле ниже.\r\n\r\nПример: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    };
    opt.AddSecurityDefinition("Bearer", sequrityScheme);

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    Console.WriteLine();
    await next();
});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await moduleLoader.InitializeAsync(app);

app.Run();
