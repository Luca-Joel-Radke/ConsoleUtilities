using System.ComponentModel;
using System.Text;
namespace ConsoleUtilities;

public class ConsoleReader
{
    public static Result<T?> Read<T>(
        string? prompt = null,
        Func<T?, bool>? validationFunc = null, string? validationErrorMessage = null,
        int maxRetries = 0
    )
    {
        var retries = 0;

        while (true)
        {
            if (maxRetries > 0 && retries >= maxRetries)
            {
                return Result<T?>.Failure("Max retries reached.");
            }

            Console.Write(prompt ?? $"Please enter {typeof(T).Name}: ");

            var line = Console.ReadLine();

            try
            {
                var parsed = ParseInput<T>(line);

                if (validationFunc is null || validationFunc(parsed))
                {
                    return Result<T?>.Success(parsed);
                }
                else
                {
                    Console.WriteLine(validationErrorMessage ?? "Validation failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Input error: {ex.Message}");
            }

            retries++;
        }
    }

    public static Result<string?> ReadHidden(
        string? prompt = null,
        Func<string?, bool>? validationFunc = null, string? validationErrorMessage = null,
        int maxRetries = 0
    )
    {
        var retries = 0;

        while (true)
        {
            if (maxRetries > 0 && retries >= maxRetries)
            {
                return Result<string?>.Failure("Max retries reached.");
            }

            Console.Write(prompt ?? "Please enter value: ");

            var input = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }

                if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Length--;
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    input.Append(key.KeyChar);
                    Console.Write("*");
                }
            }
            Console.WriteLine();

            var line = input.ToString();

            if (validationFunc is null || validationFunc(line))
            {
                return Result<string?>.Success(line);
            }

            Console.WriteLine(validationErrorMessage ?? "Validation failed.");
            retries++;
        }
    }

    private static T? ParseInput<T>(string? input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentNullException(nameof(input), "Input cannot be null or empty.");
        }

        var targetType = typeof(T);

        // Just a few common types to make things faster.
        // Seeing that TypeDescriptor supports Invariant parsing, it can handle numbers for us
        if (targetType == typeof(string)) return (T?)(object)input;
        if (targetType == typeof(bool)) return (T?)(object)bool.Parse(input);

        // Fallback to TypeDescriptor for other types
        var converter = TypeDescriptor.GetConverter(targetType);
        return (T?)converter.ConvertFromInvariantString(input);
    }
}
