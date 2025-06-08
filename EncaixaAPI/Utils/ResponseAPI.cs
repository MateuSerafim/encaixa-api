namespace EncaixaAPI.Utils;

public record ResponseAPI<T>
{
    public T? Result { get; }
    public List<string> Errors { get; }
    public DateTime FinishDate { get; }

    public ResponseAPI(T result)
    {
        Result = result;
        Errors = [];
        FinishDate = DateTime.UtcNow;
    }

    public ResponseAPI(List<string> errors)
    {
        Errors = errors;
        FinishDate = DateTime.UtcNow;
    }
}

