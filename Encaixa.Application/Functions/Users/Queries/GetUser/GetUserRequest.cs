using Encaixa.Application.Orquestrators.Requests;
using Encaixa.Domain.Users;

namespace Encaixa.Application.Functions.Users.Queries;
public record GetUserRequest(Guid UserId) : IRequestDTO
{
}

public record GetUserResponse(Guid Id, string Name, string? Surname, string Email)
{
    public static GetUserResponse ConvertByUser(User user)
        => new(user.Id, user.FirstName.Value, user.Surname?.Value, user.Email.Value);
}
