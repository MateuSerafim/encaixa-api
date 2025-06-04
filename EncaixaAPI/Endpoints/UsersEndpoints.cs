using System.Security.Claims;
using Encaixa.Application.Functions.Users.Commands;
using Encaixa.Application.Functions.Users.Queries;
using Encaixa.Application.Orquestrators;
using Encaixa.Application.Services.Users;
using EncaixaAPI.Utils;
using Microsoft.AspNetCore.Authorization;
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

            return result.SetAPIResponse(urlGroupV1 + "/{userId}");
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
                                         ClaimsPrincipal user) =>
        {
            var tryGetUserId = Guid.TryParse(user.FindFirst("UserId")?.Value, out var userId);
            if (!tryGetUserId)
                return Results.Unauthorized();

            var result = await orcherstrator.ExecuteQueryAsync(
                new GetUserRequestHandler(usuarioService, new GetUserRequest(userId)));
            
            return result.SetAPIResponse();
        }).RequireAuthorization();
    }
}
