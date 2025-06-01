using BaseRepository.Repositories;
using BaseRepository.UnitWorkBase;
using BaseUtils.FlowControl.ErrorType;
using BaseUtils.FlowControl.ResultType;
using Encaixa.Domain.Users;
using Encaixa.Infrastructure.UserIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Encaixa.Application.Services.Users;
public class UserService(IUnitOfWork unitOfWork,
                         UserManager<UserApplication> userManager) : IUserService
{
    private readonly IWriteRepository<User, Guid> _userRepository =
          unitOfWork.WriteRepository<User, Guid>();

    private readonly UserManager<UserApplication> _userManager = userManager;

    public const string EmailAlreadInUse = $"Email {ErrorResponse.ReferenceToVariable} já cadastrado.";

    public async Task<Result<User>> AddUser(User user, string password,
        CancellationToken token = default)
    {
        var validateResult = PasswordValidator.ValidatePassword(password);
        if (validateResult.IsFailure)
            return validateResult.Errors;

        if (await _userRepository.Query(i => i.Email == user.Email).AnyAsync(token))
            return ErrorResponse.InvalidOperationError(EmailAlreadInUse, user.Email.Value);

        var result = await _userRepository.AddAsync(user, token);
        if (result.IsFailure)
            return result.Errors;

        var userApp = new UserApplication(user);

        var resultManager = await _userManager.CreateAsync(userApp, password);
        if (!resultManager.Succeeded)
            return ErrorResponse.CriticalError("Erro ao adicionar usuário!");

        return user;
    }
}
