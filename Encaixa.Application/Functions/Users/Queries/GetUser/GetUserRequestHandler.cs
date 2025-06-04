using BaseUtils.FlowControl.ResultType;
using Encaixa.Application.Orquestrators.Requests;
using Encaixa.Application.Services.Users;

namespace Encaixa.Application.Functions.Users.Queries;
public class GetUserRequestHandler(IUserService userService,
                                   GetUserRequest input) :
            IQueryHandler<GetUserRequest, GetUserResponse>
{
    private readonly IUserService _userService = userService;
    public GetUserRequest Input { get; } = input;
    public async Task<Result<GetUserResponse>> HandleAsync(CancellationToken token = default)
    {
        var userResult = await _userService.GetReadRepository()
                                           .GetByIdAsync(Input.UserId, token);
        if (userResult.IsFailure)
            return userResult.Errors;

        return GetUserResponse.ConvertByUser(userResult.GetValue());
    }
}
