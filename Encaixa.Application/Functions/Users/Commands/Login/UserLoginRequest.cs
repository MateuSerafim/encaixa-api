using Encaixa.Application.Orquestrators.Requests;

namespace Encaixa.Application.Functions.Users.Commands;

public record UserLoginRequest(string Email, string Password) : IRequestDTO
{

}
public record UserLoginResponse(string Token);
