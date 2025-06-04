using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BaseRepository.Repositories;
using BaseRepository.UnitWorkBase;
using BaseUtils.FlowControl.ErrorType;
using BaseUtils.FlowControl.ResultType;
using Encaixa.Domain.Users;
using Encaixa.Infrastructure.UserIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Encaixa.Application.Services.Users;
public class UserService(IUnitOfWork unitOfWork,
                         UserManager<UserApplication> userManager,
                         IConfiguration configuration) : IUserService
{
    private readonly IWriteRepository<User, Guid> _userRepository =
            unitOfWork.WriteRepository<User, Guid>();

    private readonly IReadRepository<User, Guid> _userReadRepository =
            unitOfWork.ReadOnlyRepository<User, Guid>();

    private readonly UserManager<UserApplication> _userManager = userManager;
    private readonly IConfiguration _configuration = configuration;

    public const string EmailAlreadInUse = $"Email {ErrorResponse.ReferenceToVariable} já cadastrado.";

    public IReadRepository<User, Guid> GetReadRepository() => _userReadRepository;

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

    public async Task<Result<string>> Login(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return ErrorResponse.NotFoundError("usuário não encontrado.");

        var validPassword = await _userManager.CheckPasswordAsync(user, password);
        if (!validPassword)
            return ErrorResponse.NoAccessError("Usuário ou senha incorretos.");

        var token = GenerateJwtToken(user);
        if (string.IsNullOrEmpty(token))
            return ErrorResponse.CriticalError("Problema ao gerar o token de acesso");

        return token;
    }

    private string GenerateJwtToken(UserApplication user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"]!));

        var claims = new List<Claim>
        {
            new("UserId", user.UserId.ToString()),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.NameIdentifier, user.Id), // este é o padrão
        };

        var token = new JwtSecurityToken(issuer: issuer, audience: audience,
            claims: claims, expires: expires, signingCredentials:
            new SigningCredentials(new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
