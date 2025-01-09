namespace ConsoleUtilities.Types;

public static class ResultExtensions
{
    public static Result<T?> OnSuccess<T>(this Result<T?> result, Action<T?> action)
    {
        if (result.IsSuccess)
        {
            action(result.Value);
        }

        return result;
    }

    public static async Task<Result<T?>> OnSuccessAsync<T>(
        this Result<T?> result,
        Func<T?, Task> action
    )
    {
        if (result.IsSuccess)
        {
            await action(result.Value);
        }

        return result;
    }

    public static Result<T?> OnFailure<T>(this Result<T?> result, Action<string> action)
    {
        if (!result.IsSuccess)
        {
            action(result.Error!);
        }

        return result;
    }

    public static async Task<Result<T?>> OnFailureAsync<T>(this Result<T?> result, Func<string, Task> action)
    {
        if (!result.IsSuccess)
        {
            await action(result.Error!);
        }

        return result;
    }

    public static Result<TResult?> Map<T, TResult>(this Result<T?> result, Func<T?, TResult?> transform)
    {
        if (result.IsSuccess)
        {
            return Result<TResult?>.Success(transform(result.Value));
        }

        return Result<TResult?>.Failure(result.Error!);
    }

    public static async Task<Result<TResult?>> MapAsync<T, TResult>(
        this Result<T?> result,
        Func<T?, Task<TResult?>> transform
    )
    {
        if (result.IsSuccess)
        {
            return Result<TResult?>.Success(await transform(result.Value));
        }

        return Result<TResult?>.Failure(result.Error!);
    }

    public static Result<TResult?> Bind<T, TResult>(this Result<T?> result, Func<T?, Result<TResult?>> operation)
    {
        if (result.IsSuccess)
        {
            return operation(result.Value);
        }

        return Result<TResult?>.Failure(result.Error!);
    }

    public static async Task<Result<TResult?>> BindAsync<T, TResult>(
        this Result<T?> result,
        Func<T?, Task<Result<TResult?>>> operation
    )
    {
        if (result.IsSuccess)
        {
            return await operation(result.Value);
        }
        return Result<TResult?>.Failure(result.Error!);
    }

    public static Result<(T1?, T2?)> Combine<T1, T2>(this Result<T1?> result1, Result<T2?> result2)
    {
        if (!result1.IsSuccess)
        {
            return Result<(T1?, T2?)>.Failure(result1.Error!);
        }

        if (!result2.IsSuccess)
        {
            return Result<(T1?, T2?)>.Failure(result2.Error!);
        }

        return Result<(T1?, T2?)>.Success((result1.Value, result2.Value));
    }

    public static async Task<Result<(T1?, T2?)>> CombineAsync<T1, T2>(
        this Task<Result<T1?>> result1,
        Task<Result<T2?>> result2
    )
    {
        var (r1, r2) = (await result1, await result2);
        return r1.Combine(r2);
    }

    public static async Task<Result<T?>> OnSuccess<T>(
        this Task<Result<T?>> resultTask,
        Action<T?> action
    )
    {
        var result = await resultTask;
        return result.OnSuccess(action);
    }

    public static async Task<Result<T?>> OnSuccessAsync<T>(
        this Task<Result<T?>> resultTask,
        Func<T?, Task> action
    )
    {
        var result = await resultTask;
        return await result.OnSuccessAsync(action);
    }

    public static async Task<Result<T?>> OnFailure<T>(
        this Task<Result<T?>> resultTask,
        Action<string> action
    )
    {
        var result = await resultTask;
        return result.OnFailure(action);
    }

    public static async Task<Result<T?>> OnFailureAsync<T>(
        this Task<Result<T?>> resultTask,
        Func<string, Task> action
    )
    {
        var result = await resultTask;
        return await result.OnFailureAsync(action);
    }
}
