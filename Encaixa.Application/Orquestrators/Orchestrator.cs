using BaseRepository.UnitWorkBase;
using BaseUtils.FlowControl.ResultType;
using Encaixa.Application.Orquestrators.Requests;

namespace Encaixa.Application.Orquestrators;
public class Orchestrator(IUnitOfWork unitOfWork) : IOrchestrator
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> ExecuteCommandAsync<T>(
        ICommandHandler<T, int> commandHandler,
        CancellationToken token = default) where T: IRequestDTO
    {
        var commandResult = await commandHandler.HandleAsync(token);
        if (commandResult.IsFailure)
        {
            _unitOfWork.InvalidateState();
            return commandResult.Errors;
        }

        var commitResult = await _unitOfWork.CommitAsync(token);
        if (commitResult.IsFailure)
            return commandResult.Errors;

        return Result.Success();
    }

    public async Task<Result<E>> ExecuteCommandAsync<T, E>(
        ICommandHandler<T, E> commandHandler,
        CancellationToken token = default) where T: IRequestDTO
    {
        var commandResult = await commandHandler.HandleAsync(token);
        if (commandResult.IsFailure)
        {
            _unitOfWork.InvalidateState();
            return commandResult.Errors;
        }

        var commitResult = await _unitOfWork.CommitAsync(token);
        if (commitResult.IsFailure)
            return commandResult.Errors;

        return commandResult;
    }
    
    public async Task<Result<E>> ExecuteQueryAsync<T, E>(
        IQueryHandler<T, E> queryHandler,
        CancellationToken token = default) where T: IRequestDTO
    {
        return await queryHandler.HandleAsync(token);
    }
}
