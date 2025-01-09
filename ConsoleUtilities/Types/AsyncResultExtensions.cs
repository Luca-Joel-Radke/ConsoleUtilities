namespace ConsoleUtilities.Types;

public static class AsyncResultExtensions
{
    public static AsyncResult<T> ToAsync<T>(this Result<T?> result) =>
        new(Task.FromResult(result));

    public static AsyncResult<T> ToAsync<T>(this Task<Result<T?>> result) =>
        new(result);

    public static AsyncResult<T> OnSuccess<T>(this AsyncResult<T> result, Action<T?> action) =>
        new(result.AsTask().ContinueWith(t => t.Result.OnSuccess(action)));

    public static AsyncResult<T> OnFailure<T>(this AsyncResult<T> result, Action<string> action) =>
        new(result.AsTask().ContinueWith(t => t.Result.OnFailure(action)));

    public static AsyncResult<T> OnSuccessAsync<T>(this AsyncResult<T> result, Func<T?, Task> action) =>
        new(result.AsTask().ContinueWith(async t =>
            await t.Result.OnSuccessAsync(action)).Unwrap());

    public static AsyncResult<T> OnFailureAsync<T>(this AsyncResult<T> result, Func<string, Task> action) =>
        new(result.AsTask().ContinueWith(async t =>
            await t.Result.OnFailureAsync(action)).Unwrap());
}
