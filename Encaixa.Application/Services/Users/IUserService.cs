using BaseUtils.FlowControl.ResultType;
using Encaixa.Domain.Users;

namespace Encaixa.Application.Services.Users;
public interface IUserService
{
    Task<Result<User>> AddUser(User user, string password, CancellationToken token = default);
}
