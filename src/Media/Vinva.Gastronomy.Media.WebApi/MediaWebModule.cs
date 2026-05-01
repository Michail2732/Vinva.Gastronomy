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

        public int Order => 3;

        public async Task InitializeAsync(WebApplication webApp, CancellationToken ct = default)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<MediaDbContext>()
                    ?? throw new ArgumentNullException($"Not found {nameof(MediaDbContext)}");
                await context!.Database.MigrateAsync();
            }                        
        }

        public void RegisterServices(WebModuleContext context)
        {
            context.ConfigureMediatR(opt =>
            {
                opt.RegisterServicesFromAssemblies(typeof(CreateImageCommand).Assembly);
            });
            context.AddApplicationPart(GetType().Assembly);
            context.Services.AddValidatorsFromAssembly(typeof(CreateImageCommand).Assembly);

            // todo: вынести в отдельный класс механизм регистрации конфигурации 
            Func<IConfiguration, IConfigurationSection> identityConfigSpec =
                context.Environment.IsDevelopment()
                ? a => a.GetSection("MediaModule").GetSection("Development")
                : a => a.GetSection("MediaModule").GetSection("Production");
            context.Services.Configure<ImagesFileStorageConfig>(identityConfigSpec(context.Configuration).GetSection("ImagesFileStorageConfig"));

            context.Services.AddSingleton<IImagesStorage, ImagesFileStorage>();
            context.Services.AddSingleton<IImageFormatService, ImageFormatService>();
            context.Services.AddDbContext<MediaDbContext>(options =>
            {
                options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnectionString"));
            });
        }
    }
}
