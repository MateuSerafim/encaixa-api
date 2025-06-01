
using BaseUtils.FlowControl.ResultType;

namespace Encaixa.Application.Orquestrators.Requests;
public interface IRequestHandler<T, E> where T: IRequestDTO
{
    public T Input { get; }
    public Task<Result<E>> HandleAsync(CancellationToken token = default);
}
