namespace ConsoleUtilities.Types;

public class AsyncResult<T>(Task<Result<T?>> result)
{
    public Task<Result<T?>> AsTask() => result;
}
