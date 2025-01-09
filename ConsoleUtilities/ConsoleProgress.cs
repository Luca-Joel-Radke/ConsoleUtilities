namespace ConsoleUtilities;

public class ConsoleProgress
{
    public static async Task Show(
        Func<IProgress<double>, Task> operation,
        string message = "Processing",
        int barWidth = 20,
        char fillChar = '#',
        char emptyChar = '-'
    )
    {
        var progress = new Progress<double>(percent =>
        {
            var filled = (int)Math.Round(barWidth * percent / 100);
            if (barWidth > 0)
            {
                var bar = new string(fillChar, filled) + new string(emptyChar, barWidth - filled);
                Console.Write($"\r{message}: [{bar}] {percent:F1}% ");
            }

            if (percent >= 100)
            {
                Console.WriteLine();
            }
        });

        await operation(progress);
    }
}
