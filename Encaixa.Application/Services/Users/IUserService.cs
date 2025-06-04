using BaseRepository.Repositories;
using BaseUtils.FlowControl.ResultType;
using Encaixa.Domain.Users;

namespace Encaixa.Application.Services.Users;
public interface IUserService
{
    Task<Result<User>> AddUser(User user, string password, CancellationToken token = default);
    Task<Result<string>> Login(string email, string password);
    IReadRepository<User, Guid> GetReadRepository();
}
