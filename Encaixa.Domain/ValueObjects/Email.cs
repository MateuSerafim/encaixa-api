using BaseUtils.FlowControl.ResultType;
using BaseValueObjects.Validators;
using BaseValueObjects.Validators.Extensions;
using BaseValueObjects.ValueObjects.SimpleValueObject;

namespace Encaixa.Domain.ValueObjects;
public class Email: ValueObject<string>, IValueObject<string, Email>
{
    public const int MinEmailLength = 10;
    public const int MaxEmailLength = 255;
    
    private Email(string value) : base(value)
    {
    }

    public static Result<Email> Build(string value) 
    => Build(new Email(value));

    public IValueValidator<string> Validator()
    => new ValueValidator<string>(Value)
        .SetMinLengthText(MinEmailLength)
        .SetMaxLengthText(MaxEmailLength)
        .SetRegexValidation(RegexCommonExpressions.GenericEmail);
}
