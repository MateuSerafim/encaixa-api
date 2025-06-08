using Encaixa.Infrastructure.UserIdentity;
using Microsoft.AspNetCore.Identity;

namespace EncaixaAPI.Middleware;
public class AuthMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userEmail = context.User.Identity.Name ?? "";

            var userManagerService = context.RequestServices.GetRequiredService<UserManager<UserApplication>>();
            var userRequest = context.RequestServices.GetRequiredService<UserReference>();

            var user = await userManagerService.FindByEmailAsync(userEmail);
            if (user is null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Usuário inválido.");
                userRequest.UserId = Guid.Empty;
                return;   
            }

            userRequest.UserId = user.UserId;       

            Console.WriteLine($"[Middleware] Usuário {userEmail} autenticado.");
        }
        else
            Console.WriteLine("[Middleware] Requisição anônima ou sem autenticação válida.");

        await _next(context);
    }
}
