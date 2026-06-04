using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CitizenRegistry.API.Middlewares
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Uma exceção não tratada ocorreu: {Message}", exception.Message);

            httpContext.Response.ContentType = "application/json";
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "Ocorreu um erro interno no servidor. Tente novamente mais tarde.";


            if (exception is DbUpdateException || exception.InnerException is SqlException)
            {
                statusCode = (int)HttpStatusCode.ServiceUnavailable;
                message = "Serviço temporariamente indisponível. Falha na comunicação com o banco de dados.";
            }

            httpContext.Response.StatusCode = statusCode;

            var response = new
            {
                error = message,
            };

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return true;
        }
    }
}
