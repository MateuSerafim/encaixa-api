using Encaixa.Application.Functions.Users.Commands;
using Encaixa.Application.Functions.Users.Queries;
using Encaixa.Application.Orquestrators;
using Encaixa.Application.Services.Users;
using Encaixa.Infrastructure.UserIdentity;
using EncaixaAPI.Utils;
using Microsoft.AspNetCore.Mvc;

namespace EncaixaAPI.Endpoints;
public static class UsersEndpoints
{
    public const string urlGroupV1 = "api/v1";

    public static void RegisterUsersEndpoints(this WebApplication app)
    {
        var user = app.MapGroup(urlGroupV1).WithTags("Users");

        user.RegisterUserEndpoint();
        user.LoginEndpoint();

        user.GetUserByIdEndpoint();
    }

    public static void RegisterUserEndpoint(this RouteGroupBuilder app)
    {
        app.MapPost("/users", async ([FromBody] CreateUserRequest request,
                                                IOrchestrator orcherstrator,
                                                IUserService usuarioService) =>
        {
            var result = await orcherstrator.ExecuteCommandAsync(
                new CreateUserRequestHandler(usuarioService, request));

            return result.SetAPIResponse(urlGroupV1 + "/users/{userId}");
        });
    }

    public static void LoginEndpoint(this RouteGroupBuilder app)
    {
        app.MapPost("/users/login", async ([FromBody] UserLoginRequest request,
                                                      IOrchestrator orcherstrator,
                                                      IUserService usuarioService) =>
        {
            var result = await orcherstrator.ExecuteCommandAsync(
                new UserLoginRequestHandler(usuarioService, request));

            return result.SetAPIResponse();
        });
    }

    public static void GetUserByIdEndpoint(this RouteGroupBuilder app)
    {
        app.MapGet("/users/self", async (IOrchestrator orcherstrator,
                                         IUserService usuarioService,
                                         UserReference currentUser) =>
        {

            if (currentUser is null)
                return Results.Unauthorized();

            var result = await orcherstrator.ExecuteQueryAsync(
                new GetUserRequestHandler(usuarioService, new GetUserRequest(currentUser.UserId)));
            
            return result.SetAPIResponse();
        }).RequireAuthorization();
    }
}
