# Console Utilities
A small collection of helpers for building CLI apps in C#. No fancy bells and whistles - just clean utilities that handle the boring stuff.

## What's inside
- Reading user input with validation and retry capability
- Hidden input reading, e.g for passwords
- Simple progress tracking for long operations  
- Formatted tables that handle any data type
- Basic color output helpers

## Example 
Reading validated input is as simple as:
```csharp
ConsoleReader.Read<int>(
    prompt: "How old are you? ",
    validationFunc: age => age is > 0 and < 150
)
.OnSuccess(age => Console.WriteLine($"You're {age} years old!"))
.OnFailure(Console.WriteLine);
```
Want a simple table? Just pass your objects and what to show:
```csharp
ConsoleTable.Show(users,
    ("Id", u => u.Id),
    ("Name", u => u.Name),
    ("Role", u => u.Role)
);
```

## Why another console library?
I got bored and wanted clean, reusable tools for my own projects. You might like it if you:
- Need simple console UI bits without pulling in a massive package
- Want code that's easy to understand and modify
- Like typing less boilerplate
