namespace EncaixaAPI.Middleware;
public class AuthMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var tryGetId = Guid.TryParse(context.User.FindFirst("email")?.Value ?? "", out var id);
            Console.WriteLine($"[Middleware] Usuário {id} autenticado.");
        }
        else
            Console.WriteLine("[Middleware] Requisição anônima ou sem autenticação válida.");

        await _next(context);
    }
}
