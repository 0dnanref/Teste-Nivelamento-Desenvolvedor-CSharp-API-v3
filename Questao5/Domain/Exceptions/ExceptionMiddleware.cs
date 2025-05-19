using Questao5.Domain.Entities;
using Questao5.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace Questao5.Infrastructure.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BusinessException ex)
            {
                // Trata erro de negócio com 400
                await EscreverRespostaAsync(context, HttpStatusCode.BadRequest, ex.Message, ex.Tipo.ToString());
            }
            catch (Exception ex)
            {                
                _logger.LogError(ex, "Erro interno inesperado");
                await EscreverRespostaAsync(context, HttpStatusCode.InternalServerError, "Erro interno no servidor.");
            }
        }

        private static async Task EscreverRespostaAsync(HttpContext context, HttpStatusCode statusCode, string mensagem, string? tipoErro = null)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var resultado = new CustomResult<object>(
                message: mensagem,
                success: false,
                data: null,
                errorType: tipoErro
            );

            var json = JsonSerializer.Serialize(resultado);
            await context.Response.WriteAsync(json);
        }
    }
}
