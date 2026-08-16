using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Vinva.Gastronomy.ApiHost.Swagger
{
    // заменяем литеральные определения enum'ов на ссылочные 
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            
            if (context.Type.IsEnum)
            {
                
                string enumName = context.Type.Name;

                
                if (!context.SchemaRepository.Schemas.ContainsKey(enumName))
                {
                    var enumSchema = new OpenApiSchema
                    {
                        Type = "string",
                        Enum = new System.Collections.Generic.List<IOpenApiAny>()
                    };

                    
                    foreach (var name in Enum.GetNames(context.Type))
                    {
                        enumSchema.Enum.Add(new OpenApiString(name));
                    }

                    context.SchemaRepository.Schemas.Add(enumName, enumSchema);
                }


                
                schema.Reference = new OpenApiReference
                {
                    Id = context.Type.Name,
                    Type = ReferenceType.Schema
                };
            }
        }
    }
}
