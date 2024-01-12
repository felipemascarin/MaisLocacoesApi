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
                _logger.LogError("Log Error: {@Message}", ex.Message + ex.InnerException.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //string[] mensagem = ex.Message.Split('\n');
                //await context.Response.WriteAsync("Erro no servidor: " + mensagem[0]);
                await context.Response.WriteAsync("Erro no servidor: " + ex.Message);
            }
        }
    }
}