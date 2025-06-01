using BaseUtils.FlowControl.ErrorType;
using BaseUtils.FlowControl.ResultType;
using BaseValueObjects.Validators;
using BaseValueObjects.Validators.Extensions;

namespace Encaixa.Application.Services.Users;
public static class PasswordValidator
{
    public const int MinPasswordLength = 8;
    public static Result ValidatePassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            return ErrorResponse.InvalidTypeError("Senha não pode ser nula");

        var validationResult = new ValueValidator<string>(password)
            .SetMinLengthText(MinPasswordLength)
            .SetRegexValidation("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[^A-Za-z\\d]).+$");

        if (validationResult.IsValid())
            return Result.Success();
            
        return validationResult.Errors;
    }
}
