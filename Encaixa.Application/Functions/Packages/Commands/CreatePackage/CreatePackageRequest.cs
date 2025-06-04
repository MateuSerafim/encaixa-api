using Encaixa.Application.Orquestrators.Requests;
using Encaixa.Domain.Packages;

namespace Encaixa.Application.Functions.Packages.Commands;
public record CreatePackageRequest(
    string BoxLabel, int Height, int Width, int Length) : IRequestDTO
{

}

public record CreatePackageResponse(Guid Id, string BoxLabel, int Height, int Width, int Length, Guid UserId)
{
    public static CreatePackageResponse CreateView(PackageBox packageBox)
    => new(packageBox.Id, packageBox.BoxLabel, packageBox.Height.Value,
    packageBox.Width.Value, packageBox.Length.Value, packageBox.UserId);
}

