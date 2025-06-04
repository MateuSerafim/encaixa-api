using BaseUtils.FlowControl.ResultType;
using Encaixa.Domain.Packages;

namespace Encaixa.Application.Services.Packages;
public interface IPackageBoxService
{
    Task<Result<PackageBox>> AddAsync(PackageBox packageBox, CancellationToken token = default);
    Task<Result<List<PackageBox>>> GetPackagesByUserAsync(Guid userId, CancellationToken token = default);
}
