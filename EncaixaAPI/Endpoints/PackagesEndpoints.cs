using System.Security.Claims;
using Encaixa.Application.Functions.Packages.Commands;
using Encaixa.Application.Functions.Packages.Queries;
using Encaixa.Application.Orquestrators;
using Encaixa.Application.Services.Packages;
using EncaixaAPI.Utils;
using Microsoft.AspNetCore.Mvc;

namespace EncaixaAPI.Endpoints;
public static class PackagesEndpoints
{
    public const string urlGroupV1 = "api/v1";
    public static void RegisterPackagesEndpoints(this WebApplication app)
    {
        var packageGroup = app.MapGroup(urlGroupV1).WithTags("Packages");
        packageGroup.RegisterPackageEndpoint();
        packageGroup.GetPackagesEndpoint();
    }

    public static void RegisterPackageEndpoint(this RouteGroupBuilder app)
    {
        app.MapPost("/Packages", async ([FromBody] CreatePackageRequest request,
                                                   IOrchestrator orcherstrator,
                                                   IPackageBoxService packageBoxService,
                                                   ClaimsPrincipal user) =>
        {
            var tryGetUserId = Guid.TryParse(user.FindFirst("UserId")?.Value, out var userId);
            if (!tryGetUserId)
                return Results.Unauthorized();

            var result = await orcherstrator.ExecuteCommandAsync(
                new CreatePackageRequestHandler(packageBoxService, userId, request));

            return result.SetAPIResponse(urlGroupV1 + "/{userId}");
        }).RequireAuthorization();
    }
    
    public static void GetPackagesEndpoint(this RouteGroupBuilder app)
    {
        app.MapGet("/Packages", async (IOrchestrator orcherstrator, 
                                        IPackageBoxService packageBoxService,
                                        ClaimsPrincipal user) =>
        {
            var tryGetUserId = Guid.TryParse(user.FindFirst("UserId")?.Value, out var userId);
            if (!tryGetUserId)
                return Results.Unauthorized();
            
            var result = await orcherstrator.ExecuteQueryAsync(
                new GetPackagesRequestHandler(packageBoxService, new GetPackagesRequest(userId)));

            return result.SetAPIResponse();
        }).RequireAuthorization();
    }
}
