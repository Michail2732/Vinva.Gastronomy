using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Services;

namespace Vinva.Gastronomy.Common.Modularity.MediatR
{
    public class LoggingMediatRBehavior<TRequest, TResponce> 
        : IPipelineBehavior<TRequest, TResponce>        
        where TRequest : IRequest<TResponce>
    {
        private readonly ILogger<LoggingMediatRBehavior<TRequest, TResponce>> _logger;
        private readonly GuidProvider _guidProvider;

        public LoggingMediatRBehavior(ILogger<LoggingMediatRBehavior<TRequest, TResponce>> logger, 
            GuidProvider guidProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _guidProvider = guidProvider ?? throw new ArgumentNullException(nameof(guidProvider));
        }

        public async Task<TResponce> Handle(TRequest request, RequestHandlerDelegate<TResponce> next, CancellationToken cancellationToken)
        {
            var id = _guidProvider.Generate();
            var requestJson = SerializeForLog(request);
            _logger.LogInformation($"[{id}] {requestJson}");
            TResponce responce = default!;
            try
            {
                responce = await next();                                
            }
            catch
            {
                throw;
            }   
            finally
            {
                if (responce != null)
                {
                    var responceJson = SerializeForLog(responce);
                    _logger.LogInformation($"[{id}] {responceJson}");
                }
            }
            return responce;
        }

        private static string SerializeForLog(object value)
        {
            var node = JsonSerializer.SerializeToNode(value);
            RedactSensitiveProperties(node);
            return node?.ToJsonString() ?? "null";
        }

        private static void RedactSensitiveProperties(JsonNode? node)
        {
            if (node is JsonObject jsonObject)
            {
                foreach (var property in jsonObject.ToList())
                {
                    if (property.Key.Equals("Password", StringComparison.OrdinalIgnoreCase) ||
                        property.Key.Equals("PasswordHash", StringComparison.OrdinalIgnoreCase) ||
                        property.Key.Equals("AccessToken", StringComparison.OrdinalIgnoreCase) ||
                        property.Key.Equals("RefreshToken", StringComparison.OrdinalIgnoreCase))
                    {
                        jsonObject[property.Key] = "[REDACTED]";
                    }
                    else
                    {
                        RedactSensitiveProperties(property.Value);
                    }
                }
            }
            else if (node is JsonArray jsonArray)
            {
                foreach (var item in jsonArray)
                    RedactSensitiveProperties(item);
            }
        }
    }
}
