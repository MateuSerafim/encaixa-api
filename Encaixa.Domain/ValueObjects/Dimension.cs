using BaseUtils.FlowControl.ResultType;
using BaseValueObjects.Validators;
using BaseValueObjects.Validators.Extensions;
using BaseValueObjects.ValueObjects.SimpleValueObject;

namespace Encaixa.Domain.ValueObjects;
public class Dimension : ValueObject<int>, IValueObject<int, Dimension>
{
    private Dimension(int value) : base(value)
    {
    }

    public static Result<Dimension> Build(int value) 
    => Build(new Dimension(value));

    public IValueValidator<int> Validator()
    => new ValueValidator<int>(Value)
        .SetPositiveMandatory()
        .SetMinValue(1);
}
