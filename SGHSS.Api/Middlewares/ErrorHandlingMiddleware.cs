using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Serilog;

namespace SGHSS.Api.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex) // Erros do FluentValidation
        {
            Log.Warning("Erro de validação: {Errors}", ex.Errors);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var response = new
            {
                context.Response.StatusCode,
                Message = "Erro de validação nos dados enviados.",
                Errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (UnauthorizedAccessException ex) // Falta de autenticação
        {
            Log.Warning("Acesso não autorizado: {Message}", ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            var response = new
            {
                context.Response.StatusCode,
                Message = "Usuário não autenticado. É necessário login."
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (AuthenticationFailureException ex) // Token inválido
        {
            Log.Warning("Falha de autenticação: {Message}", ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

            var response = new
            {
                context.Response.StatusCode,
                Message = "Falha na autenticação. Token inválido ou expirado."
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (Exception ex) // Qualquer outro erro
        {
            Log.Error(ex, "Erro inesperado ocorreu: {Message}", ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                context.Response.StatusCode,
                Message = "Ocorreu um erro inesperado. Tente novamente mais tarde.",
                Details = ex.Message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
