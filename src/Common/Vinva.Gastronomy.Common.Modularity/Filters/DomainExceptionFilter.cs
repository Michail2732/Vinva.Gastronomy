using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Exceptions;
using Vinva.Gastronomy.Common.Exceptions;

namespace Vinva.Gastronomy.Common.Modularity.Filters
{
    public class DomainExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<DomainExceptionFilter> _logger;

        public DomainExceptionFilter(ILogger<DomainExceptionFilter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async void OnException(ExceptionContext context)
        {
            if (context.Exception is DomainException domainEx)
            {
                _logger.LogError(domainEx, domainEx.Message);
                context.HttpContext.Response.StatusCode = (int)(domainEx switch
                {
                    BadRequestException => HttpStatusCode.BadRequest,
                    ConflictException => HttpStatusCode.Conflict,
                    ForbiddenException => HttpStatusCode.Forbidden,
                    NotFoundException => HttpStatusCode.NotFound,
                    UnauthorizedException => HttpStatusCode.Unauthorized,
                    UnprocessableContentException => HttpStatusCode.UnprocessableContent,
                    _ => HttpStatusCode.InternalServerError,
                });
                var problemDetails = new ProblemDetails
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = context.HttpContext.Response.StatusCode,
                    Detail = domainEx.Message,
                };

                await context.HttpContext.Response.WriteAsJsonAsync(problemDetails).ConfigureAwait(false);
            }            
        }
    }
}
