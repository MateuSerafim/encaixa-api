
namespace Encaixa.Application.Orquestrators.Requests;
public interface ICommandHandler<T, E> : IRequestHandler<T, E> where T: IRequestDTO
{
}
