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
                case ProductNotFoundException:
                    _logger.LogWarning(exception, "Producto no encontrado: {Message}", exception.Message);
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    httpContext.Response.ContentType = "application/json";
                    var response = new
                    {
                        error = "Producto no encontrado",
                        message = exception.Message,
                        timestamp = DateTime.UtcNow
                    };
                    await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
                    return true;

                case ProductInvalidIdException:
                    _logger.LogWarning(exception, "Producto inválido: {Message}", exception.Message);
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    httpContext.Response.ContentType = "application/json";
                    var badResponse = new
                    {
                        error = "Producto inválido",
                        message = exception.Message,
                        timestamp = DateTime.UtcNow
                    };
                    await httpContext.Response.WriteAsJsonAsync(badResponse, cancellationToken);
                    return true;
            }

            return false;
        }
    }
}
