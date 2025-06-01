
using BaseUtils.FlowControl.ResultType;
using Encaixa.Application.Orquestrators.Requests;

namespace Encaixa.Application.Orquestrators;
public interface IOrchestrator
{
    Task<Result> ExecuteCommandAsync<T>(
        ICommandHandler<T, int> commandHandler,
        CancellationToken token = default) where T: IRequestDTO;
    Task<Result<E>> ExecuteCommandAsync<T, E>(
        ICommandHandler<T, E> commandHandler,
        CancellationToken token = default) where T: IRequestDTO;
    Task<Result<E>> ExecuteQueryAsync<T, E>(
        IQueryHandler<T, E> queryHandler,
        CancellationToken token = default) where T: IRequestDTO;
}
