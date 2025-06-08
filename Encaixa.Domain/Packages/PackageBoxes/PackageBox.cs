using BaseRepository.Entities.Base;
using BaseUtils.FlowControl.ErrorType;
using BaseUtils.FlowControl.ResultType;
using Encaixa.Domain.Users;
using Encaixa.Domain.ValueObjects;

namespace Encaixa.Domain.Packages;
public class PackageBox : Entity<Guid>
{
    public const string BoxLabelCannotBeNull = "Nome da caixa não pode ser nulo!";
    public const string DimensionInvalid =
        $"A caixa possui a dimensão {ErrorResponse.ReferenceToVariable} inválida!";

    public const string UserCannotBeNull = "Usuário não pode ser nulo";

    public string BoxLabel { get; }

    public Dimension Height { get; }
    public Dimension Width { get; }
    public Dimension Length { get; }

    public int Volume => Height.Value * Width.Value * Length.Value;

    public int Area => 2 * (Height.Value * Width.Value)
                     + 2 * (Width.Value * Length.Value)
                     + 2 * (Height.Value * Length.Value);

    public Guid UserId { get; }
    public virtual User User { get; }
    
    // Pro EF poder criar os objetos.
    #pragma warning disable CS8618
    protected PackageBox() : base() { }

    private PackageBox(string boxLabel, Dimension height, Dimension width,
                        Dimension length, Guid userId, EntityStatus entityStatus,
                        Guid Id = default) : base(entityStatus, Id)
    {
        BoxLabel = boxLabel;
        Height = height;
        Width = width;
        Length = length;
        UserId = userId;
    }
#pragma warning restore CS8618

    public static Result<PackageBox> Create(string boxLabel,
        int height, int width, int length, User user, Guid id = default)
    {
        if (user is null)
            return ErrorResponse.InvalidTypeError(UserCannotBeNull);

        return Create(boxLabel, height, width, length, user.Id, id);
    }

    public static Result<PackageBox> Create(string boxLabel,
    int height, int width, int length, Guid userId, Guid id = default)
    {
        List<ErrorResponse> errors = [];

        if (string.IsNullOrEmpty(boxLabel))
            errors.Add(ErrorResponse.InvalidTypeError(BoxLabelCannotBeNull));

        var heightResult = Dimension.Build(height);
        if (heightResult.IsFailure)
            errors.Add(ErrorResponse.InvalidTypeError(DimensionInvalid, "altura"));

        var widthResult = Dimension.Build(width);
        if (widthResult.IsFailure)
            errors.Add(ErrorResponse.InvalidTypeError(DimensionInvalid, "largura"));

        var lengthResult = Dimension.Build(length);
        if (lengthResult.IsFailure)
            errors.Add(ErrorResponse.InvalidTypeError(DimensionInvalid, "comprimento"));

        if (errors.Count > 0)
            return errors;

        return new PackageBox(boxLabel, heightResult.GetValue(), widthResult.GetValue(),
                              lengthResult.GetValue(), userId, EntityStatus.Activated, id);
    }
}
