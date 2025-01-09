using ConsoleUtilities;
using ConsoleUtilities.Types;

namespace SampleProject;

public class Program
{
    private record User(int Id, string Name, string Password, string Role);

    private record WorkItem(int Id, string Description)
    {
        public bool IsCompleted { get; set; } = false;
    }

    private enum MainOption
    {
        ViewUsers,
        AddUser,
        ManageWorkItems,
        ProcessData,
        Exit
    }

    public static async Task Main()
    {
        var users = new List<User>
        {
            new(1, "Alice Smith", "pass123", "Admin"),
            new(2, "Bob Johnson", "pass456", "User"),
            new(3, "Carol White", "pass789", "User")
        };

        var tasks = new List<WorkItem>
        {
            new(1, "Review documentation"),
            new(2, "Update system"),
            new(3, "Backup database"),
            new(4, "Send reports")
        };

        while (true)
        {
            Console.Clear();
            ConsoleFormat.WriteSuccess("System Management Console");
            Console.WriteLine();

            var menuResult = ConsoleMenu.Options([
                ("View Users", MainOption.ViewUsers),
                ("Add User", MainOption.AddUser),
                ("Manage Tasks", MainOption.ManageWorkItems),
                ("Process Data", MainOption.ProcessData),
                ("Exit", MainOption.Exit)
            ], "Main Menu");

            await menuResult
                .OnSuccessAsync(async option =>
                {
                    Console.Clear();

                    switch (option)
                    {
                        case MainOption.ViewUsers:
                            ViewUsers(users);
                            break;

                        case MainOption.AddUser:
                            AddUser(users);
                            break;

                        case MainOption.ManageWorkItems:
                            ManageWorkItems(tasks);
                            break;

                        case MainOption.ProcessData:
                            await ProcessData(users);
                            break;

                        case MainOption.Exit:
                            Environment.Exit(0);
                            break;
                    }
                })
                .OnFailure(ConsoleFormat.WriteError);

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    private static void ViewUsers(List<User> users)
    {
        ConsoleFormat.WriteSuccess("User Management");
        Console.WriteLine();

        var password = ConsoleReader.ReadHidden(
            "Enter admin password: ",
            pwd => pwd == "admin123",
            "Invalid admin password"
        );

        password.OnSuccess(_ =>
        {
            ConsoleTable.Show(
                users,
                ("ID", u => u.Id),
                ("Name", u => u.Name),
                ("Role", u => u.Role)
            );
        });
    }

    private static void AddUser(List<User> users)
    {
        ConsoleFormat.WriteSuccess("Add New User");
        Console.WriteLine();

        var name = ConsoleReader.Read<string>(
            "Enter name: ",
            n => !string.IsNullOrWhiteSpace(n),
            "Name cannot be empty"
        );

        var password = ConsoleReader.ReadHidden(
            "Enter password: ",
            p => p?.Length >= 6,
            "Password must be at least 6 characters"
        );

        var roles = ConsoleMenu.Options([
            ("Admin", "Admin"),
            ("User", "User")
        ], "Select role:");

        name.Combine(password)
            .Combine(roles)
            .OnSuccess(t =>
            {
                var ((username, pwd), role) = t;
                var id = users.Max(u => u.Id) + 1;
                users.Add(new User(id, username!, pwd!, role!));
                ConsoleFormat.WriteSuccess("User added successfully!");
            });
    }

    private static void ManageWorkItems(List<WorkItem> workItems)
    {
        ConsoleFormat.WriteSuccess("Work Item Management");
        Console.WriteLine();

        ConsoleMenu.Select(
                workItems.Select(item => (
                        Label: $"{item.Description} [{(item.IsCompleted ? "Done" : "Pending")}]",
                        Value: item
                    ))
            )!
            .OnSuccess(selected =>
            {
                foreach (var item in selected!)
                {
                    item.IsCompleted = true;
                }
                ConsoleFormat.WriteSuccess($"Marked {selected.Count} items as complete!");

                Console.WriteLine("\nCurrent Status:");
                ConsoleTable.Show(workItems,
                    ("ID", i => i.Id),
                    ("Description", i => i.Description),
                    ("Status", i => i.IsCompleted ? "Done" : "Pending")
                );
            });
    }

    private static async Task ProcessData(List<User> users)
    {
        ConsoleFormat.WriteWarning("Processing User Data");
        Console.WriteLine();

        await ConsoleProgress.Show(async progress =>
        {
            var total = users.Count;
            for (var i = 0; i < total; i++)
            {
                await Task.Delay(500);
                progress.Report((double)(i + 1) / total * 100);
            }
        }, "Analyzing user data");

        ConsoleFormat.WriteSuccess("Processing complete!");
    }
}
