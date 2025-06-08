namespace Encaixa.Application.Functions.Packages.Queries;
public record SimulateRequest(int Id, List<ProductRequest> Products)
{
}
public record ProductRequest(string Id, DimensionsRequest Dimensions)
{
}
public record DimensionsRequest(int Height, int Width, int Length)
{
}