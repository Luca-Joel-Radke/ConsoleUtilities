namespace ConsoleUtilities;

public static class ConsoleFormat
{
    public static void WriteColored(string text, ConsoleColor color)
    {
        var original = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = original;
    }

    public static void WriteSuccess(string message) =>
        WriteColored($"\u2713 {message}\n", ConsoleColor.Green);

    public static void WriteError(string message) =>
        WriteColored($"\u0078 {message}\n", ConsoleColor.Red);

    public static void WriteWarning(string message) =>
        WriteColored($"\u0021 {message}\n", ConsoleColor.Yellow);
}
