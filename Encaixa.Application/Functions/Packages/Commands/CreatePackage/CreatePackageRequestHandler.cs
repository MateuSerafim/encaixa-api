using BaseUtils.FlowControl.ResultType;
using Encaixa.Application.Orquestrators.Requests;
using Encaixa.Application.Services.Packages;
using Encaixa.Domain.Packages;

namespace Encaixa.Application.Functions.Packages.Commands;
public class CreatePackageRequestHandler(IPackageBoxService packageBoxService,
    Guid userActionId, CreatePackageRequest input) :
    ICommandHandler<CreatePackageRequest, CreatePackageResponse>
{
    private readonly IPackageBoxService _packageBoxService = packageBoxService;
    private readonly Guid _userActionId = userActionId;
    public CreatePackageRequest Input { get; } = input;

    public async Task<Result<CreatePackageResponse>> HandleAsync(CancellationToken token = default)
    {
        var packageResult = PackageBox.Create(Input.BoxLabel, Input.Height,
                                            Input.Width, Input.Length, _userActionId);
        if (packageResult.IsFailure)
            return packageResult.Errors;

        var packageAddResult = await _packageBoxService.AddAsync(packageResult.GetValue(), token);
        if (packageAddResult.IsFailure)
            return packageAddResult.Errors;

        return CreatePackageResponse.CreateView(packageAddResult.GetValue());
    }
}
