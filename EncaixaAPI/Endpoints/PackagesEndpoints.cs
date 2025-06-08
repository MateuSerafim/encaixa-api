using Encaixa.Application.Functions.Packages.Commands;
using Encaixa.Application.Functions.Packages.Queries;
using Encaixa.Application.Orquestrators;
using Encaixa.Application.Services.Packages;
using Encaixa.Domain.Users;
using Encaixa.Infrastructure.UserIdentity;
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
        app.MapPost("/packages", async ([FromBody] CreatePackageRequest request,
                                                   IOrchestrator orcherstrator,
                                                   IPackageBoxService packageBoxService,
                                                   UserReference currentUser) =>
        {

            var result = await orcherstrator.ExecuteCommandAsync(
                new CreatePackageRequestHandler(packageBoxService, currentUser.UserId, request));

            return result.SetAPIResponse(
                urlGroupV1 + $"/{(result.IsSuccess ? result.GetValue().Id.ToString() : Guid.Empty)}");
        }).RequireAuthorization();
    }

    public static void GetPackagesEndpoint(this RouteGroupBuilder app)
    {
        app.MapGet("/packages", async (IOrchestrator orcherstrator,
                                        IPackageBoxService packageBoxService,
                                        UserReference currentUser) =>
        {
            var result = await orcherstrator.ExecuteQueryAsync(
                new GetPackagesRequestHandler(packageBoxService, new GetPackagesRequest(currentUser.UserId)));

            return result.SetAPIResponse();
        }).RequireAuthorization();
    }
    
    public static void SimulateOrganizationPackagesEndpoint(this RouteGroupBuilder app)
    {
        app.MapPatch("/packages/simulate", async (IOrchestrator orcherstrator, 
                                                  IPackageBoxService packageBoxService,
                                                  UserReference currentUser) =>
        {   
            var result = await orcherstrator.ExecuteQueryAsync(
                new GetPackagesRequestHandler(packageBoxService, new GetPackagesRequest(currentUser.UserId)));

            return result.SetAPIResponse();
        }).RequireAuthorization();
    }
}
