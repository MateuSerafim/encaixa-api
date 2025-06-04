using Encaixa.Application.Orquestrators.Requests;
using Encaixa.Domain.Users;

namespace Encaixa.Application.Functions.Users.Commands;

public record CreateUserRequest(string FirstName,
                                string Surname,
                                string Email,
                                string Password) : IRequestDTO
{
}

public record CreateUserResponse
{
    public string Name { get; }
    public string? Surname { get; }
    public string Email { get; }
    public CreateUserResponse(User user)
    {
        Name = user.FirstName.Value;
        Surname = user.Surname?.Value;
        Email = user.Email.Value;
    }
}
