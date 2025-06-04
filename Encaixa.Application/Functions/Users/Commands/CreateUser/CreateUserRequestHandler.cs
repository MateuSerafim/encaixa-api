using BaseUtils.FlowControl.ResultType;
using Encaixa.Application.Orquestrators.Requests;
using Encaixa.Application.Services.Users;
using Encaixa.Domain.Users;

namespace Encaixa.Application.Functions.Users.Commands;
public class CreateUserRequestHandler(IUserService userService,
                                      CreateUserRequest input) : 
    ICommandHandler<CreateUserRequest, CreateUserResponse>
{
    private readonly IUserService _userService = userService;
    public CreateUserRequest Input { get; } = input;

    public async Task<Result<CreateUserResponse>> HandleAsync(CancellationToken token = default)
    {
        var userResult = User.Create(Input.Email, Input.FirstName, Input.Surname);
        if (userResult.IsFailure)
            return userResult.Errors;

        var addResult = await _userService.AddUser(userResult.GetValue(), Input.Password, token);
        if (addResult.IsFailure)
            return addResult.Errors;

        return new CreateUserResponse(userResult.GetValue());
    }
}
