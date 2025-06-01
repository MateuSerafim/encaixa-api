using Encaixa.Application.Functions.Users.Commands;
using Encaixa.Application.Orquestrators;
using Encaixa.Application.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace EncaixaAPI.Endpoints;
public static class UsersEndpoints
{
    public static void RegisterUsersEndpoints(this WebApplication app)
    {
        app.RegisterUserEndpoint();
    }

    public static void RegisterUserEndpoint(this WebApplication app)
    {
        app.MapPost("/users", async ([FromBody] CreateUserRequest request,
                                                IOrchestrator orcherstrator,
                                                IUserService usuarioService) =>
        {
            var result = await orcherstrator.ExecuteCommandAsync(
                new CreateUserRequestHandler(usuarioService, request));

            if (result.IsFailure)
                return Results.Problem(result.Errors.ToString());
            
            return Results.Created(uri:"teste", value: result.GetValue());
        });
    }
}
