using ConsoleUtilities.Types;
namespace ConsoleUtilities;

public class ConsoleMenu
{
    public static Result<T?> Options<T>(
        IEnumerable<(string Label, T Value)> options,
        string prompt = "Select an option:"
    )
    {
        var list = options.ToList();

        Console.WriteLine(prompt);
        for (var i = 0; i < list.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {list[i].Label}");
        }

        return ConsoleReader
            .Read<int>("Enter number: ",
                n => n > 0 && n <= list.Count,
                "Invalid selection.")
            .Map(n => list[n - 1].Value);
    }

    public static Result<List<T>> Select<T>(
        IEnumerable<(string Label, T Value)> options,
        string prompt = "Select items (Space to toggle, Enter to confirm):")
    {
        var items = options.ToList();
        var selected = new bool[items.Count];
        var currentIndex = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine(prompt + "\n");

            for (var i = 0; i < items.Count; i++)
            {
                if (i == currentIndex)
                {
                    Console.Write("> ");
                }
                else
                {
                    Console.Write("  ");
                }

                Console.Write(selected[i] ? "[x] " : "[ ] ");
                Console.WriteLine(items[i].Label);
            }

            var key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    currentIndex = (currentIndex - 1 + items.Count) % items.Count;
                    break;

                case ConsoleKey.DownArrow:
                    currentIndex = (currentIndex + 1) % items.Count;
                    break;

                case ConsoleKey.Spacebar:
                    selected[currentIndex] = !selected[currentIndex];
                    break;

                case ConsoleKey.Enter:
                    return Result<List<T>>.Success(
                        items.Where((_, i) => selected[i])
                            .Select(x => x.Value)
                            .ToList()
                    );

                default:
                    break;
            }
        }
    }
}
