namespace ConsoleUtilities;

public class ConsoleTable
{
    public static void Show<T>(
        IEnumerable<T> items,
        params (string Header, Func<T, object> ValueSelector)[] columns
    )
    {
        // Column widths
        var widths = columns.Select(col =>
            Math.Max(
                col.Header.Length,
                items.Max(item => col.ValueSelector(item).ToString()?.Length ?? 0)
            )
        ).ToList();

        // Headers
        for (var i = 0; i < columns.Length; i++)
        {
            Console.Write(columns[i].Header.PadRight(widths[i] + 2));
        }
        Console.WriteLine();

        // Separator
        Console.WriteLine(string.Join("", widths.Select(w => new string('-', w + 2))));

        // Rows
        foreach (var item in items)
        {
            for (var i = 0; i < columns.Length; i++)
            {
                var value = columns[i].ValueSelector(item).ToString() ?? "";
                Console.Write(value.PadRight(widths[i] + 2));
            }
            Console.WriteLine();
        }
    }
}
