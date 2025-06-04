using BaseUtils.FlowControl.ResultType;
using BaseValueObjects.Validators;
using BaseValueObjects.Validators.Extensions;
using BaseValueObjects.ValueObjects.SimpleValueObject;

namespace Encaixa.Domain.ValueObjects;
public class Name: ValueObject<string>, IValueObject<string, Name>
{
    public const int MinNameLength = 5;
    public const int MaxNameLength = 255;

    public static string ErrorNameRange
    => $"Nome deve estar entre {MinNameLength} e {MaxNameLength}!";
    
    private Name(string value) : base(value)
    {
    }

    public static Result<Name> Build(string value) 
    => Build(new Name(value));

    public IValueValidator<string> Validator()
    => new ValueValidator<string>(Value)
        .SetMinLengthText(MinNameLength, ErrorNameRange)
        .SetMaxLengthText(MaxNameLength, ErrorNameRange);
}
