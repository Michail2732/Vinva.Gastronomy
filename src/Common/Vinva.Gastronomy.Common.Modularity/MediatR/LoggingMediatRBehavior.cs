using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
            var requestJson = JsonSerializer.Serialize(request);
            _logger.LogInformation($"[{id}] {requestJson}");
            TResponce responce = default!;
            try
            {
                responce = await next();                                
            }
            catch (Exception ex)
            {
                throw;
            }   
            finally
            {
                if (responce != null)
                {
                    var responceJson = JsonSerializer.Serialize(responce);
                    _logger.LogInformation($"[{id}] {responceJson}");
                }
            }
            return responce;
        }
    }
}
