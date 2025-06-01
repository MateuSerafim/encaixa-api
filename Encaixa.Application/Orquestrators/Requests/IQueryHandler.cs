namespace Encaixa.Application.Orquestrators.Requests;
public interface IQueryHandler<T, E> : IRequestHandler<T, E> where T: IRequestDTO
{
}
