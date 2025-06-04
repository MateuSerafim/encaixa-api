using BaseUtils.FlowControl.ResultType;
using Encaixa.Application.Orquestrators.Requests;
using Encaixa.Application.Services.Users;

namespace Encaixa.Application.Functions.Users.Commands;
public class UserLoginRequestHandler(IUserService userService,
                                     UserLoginRequest input) :
    ICommandHandler<UserLoginRequest, UserLoginResponse>
{
    private readonly IUserService _userService = userService;
    public UserLoginRequest Input { get; } = input;

    public async Task<Result<UserLoginResponse>> HandleAsync(CancellationToken token = default)
    {
        var loginResult = await _userService.Login(Input.Email, Input.Password);
        if (loginResult.IsFailure)
            return loginResult.Errors;

        return new UserLoginResponse(loginResult.GetValue());
    }
}