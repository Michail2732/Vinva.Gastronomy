using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Modularity;
using Vinva.Gastronomy.Media.Application.Common;
using Vinva.Gastronomy.Media.Application.Services;
using Vinva.Gastronomy.Media.Application.Usecases.CreateImage;
using Vinva.Gastronomy.Media.Domain.Services;
using Vinva.Gastronomy.Media.Persistence;
using Vinva.Gastronomy.Media.WebApi.Services;

namespace Vinva.Gastronomy.Media.WebApi
{
    public class MediaWebModule : IWebModule
    {
        public string ModuleName => "Media";

        public IReadOnlyCollection<Type> DependsOn => Array.Empty<Type>();

        public async Task InitializeAsync(ModuleInitializationContext context, CancellationToken ct = default)
        {
            using (var scope = context.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<MediaDbContext>()
                    ?? throw new ArgumentNullException($"Not found {nameof(MediaDbContext)}");
                await dbContext.Database.MigrateAsync(ct);
            }                        
        }

        public void RegisterServices(ModuleServiceContext context)
        {
            // todo: вынести в отдельный класс механизм регистрации конфигурации 
            Func<IConfiguration, IConfigurationSection> identityConfigSpec =
                context.Environment.IsDevelopment()
                ? a => a.GetSection("MediaModule").GetSection("Development")
                : a => a.GetSection("MediaModule").GetSection("Production");
            context.Services.Configure<ImagesFileStorageConfig>(identityConfigSpec(context.Configuration).GetSection("ImagesFileStorageConfig"));

            context.Services.AddSingleton<IImagesStorage, ImagesFileStorage>();
            context.Services.AddSingleton<IImageFormatService, ImageFormatService>();
            context.Services.AddValidatorsFromAssembly(typeof(CreateImageCommand).Assembly);
            context.Services.AddDbContext<MediaDbContext>(options =>
            {
                options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnectionString"));
            });
        }

        public void RegisterWeb(ModuleWebContext context)
        {
            context.ConfigureMediatR(opt =>
            {
                opt.RegisterServicesFromAssemblies(typeof(CreateImageCommand).Assembly);
            });
            context.AddApplicationPart(GetType().Assembly);
        }
    }
}
