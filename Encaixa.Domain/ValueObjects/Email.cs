using BaseUtils.FlowControl.ResultType;
using BaseValueObjects.Validators;
using BaseValueObjects.Validators.Extensions;
using BaseValueObjects.ValueObjects.SimpleValueObject;

namespace Encaixa.Domain.ValueObjects;
public class Email: ValueObject<string>, IValueObject<string, Email>
{
    public const int MinEmailLength = 10;
    public const int MaxEmailLength = 255;

    public static string ErrorEmailRange
    => $"Email deve ter comprimento entre {MinEmailLength} e {MaxEmailLength}!";
    public static string InvalidEmailRegex 
    => "Email está fora do padrão nome@provedor.ref!";
    
    private Email(string value) : base(value)
    {
    }

    public static Result<Email> Build(string value) 
    => Build(new Email(value));

    public IValueValidator<string> Validator()
    => new ValueValidator<string>(Value)
        .SetMinLengthText(MinEmailLength, ErrorEmailRange)
        .SetMaxLengthText(MaxEmailLength, ErrorEmailRange)
        .SetRegexValidation(RegexCommonExpressions.GenericEmail, InvalidEmailRegex);
}
