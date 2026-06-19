using Microsoft.AspNetCore.Diagnostics;

namespace WebApplicationPractica.Exceptions
{
    public class ProductExceptionHandler : IExceptionHandler
    {
        public readonly ILogger<ProductExceptionHandler> _logger;
        public ProductExceptionHandler(ILogger<ProductExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            switch(exception)
            {
                case NotFoundException:
                    _logger.LogWarning(exception, "Entidad no encontrado: {Message}", exception.Message);
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    httpContext.Response.ContentType = "application/json";
                    var response = new
                    {
                        error = "Entidad no encontrado",
                        message = exception.Message,
                        timestamp = DateTime.UtcNow
                    };
                    await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
                    return true;
                case InvalidValueException:
                    _logger.LogWarning(exception, "Valor inválida: {Message}", exception.Message);
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    httpContext.Response.ContentType = "application/json";
                    var responseInvalidValue = new
                    {
                        error = "Categoría inválida",
                        message = exception.Message,
                        timestamp = DateTime.UtcNow
                    };
                    await httpContext.Response.WriteAsJsonAsync(responseInvalidValue, cancellationToken);
                    return true;
                

            }

            return false;
        }
    }
}
