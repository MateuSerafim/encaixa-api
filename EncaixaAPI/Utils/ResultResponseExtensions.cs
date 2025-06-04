using BaseUtils.FlowControl.ErrorType;
using BaseUtils.FlowControl.ResultType;

namespace EncaixaAPI.Utils;
public static class ResultResponseExtensions
{
    public static IResult SetAPIResponse(this Result result)
    {
        if (result.IsFailure)
            result.Errors.SetAPIResponse();

        return Results.NoContent();
    }

    public static IResult SetAPIResponse<T>(this Result<T> result)
    {
        if (result.IsFailure)
            return result.Errors.SetAPIResponse();

        return Results.Ok(result.GetValue());
    }

    public static IResult SetAPIResponse<T>(this Result<T> result, string resourceUrl)
    {
        if (result.IsFailure)
            return result.Errors.SetAPIResponse();

        return Results.Created(uri: resourceUrl, value: result.GetValue());
    }

    private static IResult SetAPIResponse(this List<ErrorResponse> errors)
    {
        if (errors.Any(e => e.ErrorType == ErrorTypeEnum.CriticalError))
        {
            var criticalErrorsMessage = string.Concat("Erro(s) crítico(s): ",
            string.Join("|", errors.Where(e => e.ErrorType == ErrorTypeEnum.CriticalError)
                                   .Select(s => s.ErrorMessage())));

            return Results.InternalServerError(criticalErrorsMessage);
        }

        if (errors.Any(e => e.ErrorType == ErrorTypeEnum.NoAccessError))
            return Results.Forbid();

        if (errors.Any(e => e.ErrorType == ErrorTypeEnum.NotFoundError))
            return Results.NotFound();

        var concatenateErrorsMessage = string.Concat("Erro(s): ",
            string.Join("|", errors.Select(s => s.ErrorMessage())));

        return Results.BadRequest(concatenateErrorsMessage);
    }
}
