using BaseRepository.Entities.Base;
using BaseRepository.Repositories;
using BaseRepository.UnitWorkBase;
using BaseUtils.FlowControl.ErrorType;
using BaseUtils.FlowControl.ResultType;
using Encaixa.Domain.Packages;
using Encaixa.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Encaixa.Application.Services.Packages;
public class PackageBoxService(IUnitOfWork unitOfWork) : IPackageBoxService
{
    private readonly IWriteRepository<User, Guid> _userRepository =
            unitOfWork.WriteRepository<User, Guid>();

    private readonly IWriteRepository<PackageBox, Guid> _packageBoxRepository =
            unitOfWork.WriteRepository<PackageBox, Guid>();
    private readonly IReadRepository<PackageBox, Guid> _packageBoxReadRepository =
            unitOfWork.ReadOnlyRepository<PackageBox, Guid>();


    public async Task<Result<PackageBox>> AddAsync(PackageBox packageBox, CancellationToken token = default)
    {
        if (packageBox == null)
            return ErrorResponse.InvalidOperationError("Caixa inválida!");

        var userMaybe = await _userRepository.GetByIdAsync(packageBox.UserId, EntityStatus.Removed, token);
        if (userMaybe.IsFailure)
            return userMaybe.Errors;

        if (userMaybe.GetValue().EntityStatus != EntityStatus.Activated)
            return ErrorResponse.InvalidOperationError("Usuário não está ativo!");

        var hasDuplicate = await
            _packageBoxReadRepository.Query(p => p.UserId == packageBox.UserId
                                              && p.Height == packageBox.Height
                                              && p.Length == packageBox.Length
                                              && p.Width == packageBox.Width)
                                      .AnyAsync(token);
        if (hasDuplicate)
            return ErrorResponse.InvalidOperationError("Caixa duplicada!");

        return await _packageBoxRepository.AddAsync(packageBox, token);
    }

    public async Task<Result<List<PackageBox>>> GetPackagesByUserAsync(
        Guid userId, CancellationToken token = default)
    {
        if (!await _userRepository.ExistAsync(userId, token))
            return ErrorResponse.NotFoundError("Usuário não encontrado!");

        return await _packageBoxReadRepository.Query(p => p.UserId == userId).ToListAsync(token);
    }

}
