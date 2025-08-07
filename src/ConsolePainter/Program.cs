// Example 1: Print colored text
ConsolePainter.WriteLine("Hello, world!", ConsoleColor.Green);
ConsolePainter.WriteLine("Warning: Low disk space!", ConsoleColor.Yellow);
ConsolePainter.WriteLine("Error: File not found!", ConsoleColor.Red);

Console.WriteLine();

// Example 2: Display simple table with default colors (white)
var fruits = new List<Fruit>
{
     new Fruit { Name = "Apple", Color = "Red", Quantity = 10 },
     new Fruit { Name = "Banana", Color = "Yellow", Quantity = 5 },
     new Fruit { Name = "Grapes", Color = "Green", Quantity = 15 }
};
ConsolePainter.WriteTable(fruits);

Console.WriteLine();

// Example 3: Display table with custom colors for headers and rows
ConsolePainter.WriteTable(fruits, ConsoleColor.Cyan, ConsoleColor.DarkGray);

Console.WriteLine();

// Example 4: Display table for simple type list (string)
var colors = new List<string> { "Red", "Green", "Blue" };
ConsolePainter.WriteTable(colors, ConsoleColor.Magenta, ConsoleColor.Gray);

Console.WriteLine();

// Example 5: Display table with inheritance (base and derived class)
var employees = new List<Employee>
{
     new Employee { Name = "John", Age = 40, Department = "HR" },
     new Employee { Name = "Alice", Age = 30, Department = "IT" }
};
ConsolePainter.WriteTable(employees, ConsoleColor.Yellow, ConsoleColor.DarkBlue);


class Fruit
{
    public string Name { get; set; }
    public string Color { get; set; }
    public int Quantity { get; set; }
}

class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

class Employee : Person
{
    public string Department { get; set; }
}