using BaseRepository.Entities.Base;
using BaseUtils.FlowControl.ErrorType;
using BaseUtils.FlowControl.ResultType;
using Encaixa.Domain.Packages;
using Encaixa.Domain.ValueObjects;

namespace Encaixa.Domain.Users;
public class User : Entity<Guid>
{
    public Email Email { get; }
    public Name FirstName { get; private set; }
    public Name? Surname { get; private set; }

    public virtual List<PackageBox> PackageBoxes { get; private set; } = [];

    protected User() : base() { }

    private User(Email email, Name firstName,
                 EntityStatus entityStatus, Guid id = default) :
                 base(entityStatus, id)
    {
        Email = email;
        FirstName = firstName;
    }
    
    public static Result<User> Create(string email, string firstName,
    string surname, Guid id = default)
    {
        List<ErrorResponse> errors = [];

        var emailResult = Email.Build(email);
        if (emailResult.IsFailure)
            errors.AddRange(emailResult.Errors);

        var firstNameResult = Name.Build(firstName);
        if (firstNameResult.IsFailure)
            errors.AddRange(firstNameResult.Errors);

        if (errors.Count > 0)
            return errors;

        var newUser = new User(emailResult.GetValue(),
                            firstNameResult.GetValue(),
                            EntityStatus.Activated, id);

        var surnameResult = newUser.SetSurname(surname);
        if (surnameResult.IsFailure)
            return surnameResult.Errors;

        newUser.AddInitialPackageBoxes();

        return newUser;
    }

    private void AddInitialPackageBoxes()
    {
        PackageBoxes.Add(PackageBox.Create("Caixa 1", 30, 40, 80, Id).GetValue());
        PackageBoxes.Add(PackageBox.Create("Caixa 2", 80, 50, 40, Id).GetValue());
        PackageBoxes.Add(PackageBox.Create("Caixa 3", 50, 80, 60, Id).GetValue());
    }

    public override Result Remove()
    {
        foreach (var box in PackageBoxes)
        {
            box.Remove();
        }

        return base.Remove();
    }

    public Result SetSurname(string surname)
    {
        if (string.IsNullOrEmpty(surname))
        {
            Surname = null;
            return Result.Success();
        }

        var surnameResult = Name.Build(surname);
        if (surnameResult.IsFailure)
            return surnameResult.Errors;

        Surname = surnameResult.GetValue();
        return Result.Success();
    }
}
