using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Net;

namespace MaisLocacoes.WebApi.Exceptions.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ExceptionMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError("Log Error: {@Message}", ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync("Erro no servidor");
            }
        }
    }
}