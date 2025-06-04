using Encaixa.Application.Orquestrators.Requests;
using Encaixa.Domain.Packages;

namespace Encaixa.Application.Functions.Packages.Queries;
public record GetPackagesRequest(Guid UserId) : IRequestDTO
{
}
public record GetPackagesResponse(Guid Id, string BoxLabel, int Height, int Width, int Length, Guid UserId)
{
    public static GetPackagesResponse CreateView(PackageBox packageBox)
    => new(packageBox.Id, packageBox.BoxLabel, packageBox.Height.Value,
    packageBox.Width.Value, packageBox.Length.Value, packageBox.UserId);
}
