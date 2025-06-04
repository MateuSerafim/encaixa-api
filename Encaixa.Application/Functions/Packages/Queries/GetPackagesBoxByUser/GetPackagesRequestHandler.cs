using BaseUtils.FlowControl.ResultType;
using Encaixa.Application.Orquestrators.Requests;
using Encaixa.Application.Services.Packages;

namespace Encaixa.Application.Functions.Packages.Queries;
public class GetPackagesRequestHandler(
    IPackageBoxService packageBoxService, GetPackagesRequest input) :
    IQueryHandler<GetPackagesRequest, List<GetPackagesResponse>>
{
    private readonly IPackageBoxService _packageBoxService = packageBoxService;
    public GetPackagesRequest Input { get; } = input;

    public async Task<Result<List<GetPackagesResponse>>> HandleAsync(CancellationToken token = default)
    {
        var queryResult = await _packageBoxService.GetPackagesByUserAsync(Input.UserId, token);
        if (queryResult.IsFailure)
            return queryResult.Errors;

        return queryResult.GetValue().Select(GetPackagesResponse.CreateView).ToList();
    }
}
