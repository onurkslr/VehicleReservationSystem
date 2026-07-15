using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace VehicleReservationSystem.API.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Beklenmeyen bir hata oluştu: {Message}", exception.Message);

            var (statusCode, title) = exception switch
            {
                InvalidOperationException => (StatusCodes.Status409Conflict, "İş kuralı ihlali"),
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Kayıt bulunamadı"),
                ArgumentException => (StatusCodes.Status400BadRequest, "Geçersiz istek"),
                _ => (StatusCodes.Status500InternalServerError, "Sunucu hatası")
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message
            };

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}