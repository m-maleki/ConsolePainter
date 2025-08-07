using System.Collections;
using System.Reflection;

public static class ConsolePainter
{
    public static void Write(string text, ConsoleColor? foreground = null, ConsoleColor? background = null)
    {
        var originalForeground = Console.ForegroundColor;
        var originalBackground = Console.BackgroundColor;

        if (foreground.HasValue)
            Console.ForegroundColor = foreground.Value;
        if (background.HasValue)
            Console.BackgroundColor = background.Value;

        Console.Write(text);

        Console.ForegroundColor = originalForeground;
        Console.BackgroundColor = originalBackground;
    }

    public static void WriteLine(string text, ConsoleColor? foreground = null, ConsoleColor? background = null)
    {
        Write(text, foreground, background);
        Console.WriteLine();
    }

    public static void WriteTable(IEnumerable items, ConsoleColor? headerColor = null, ConsoleColor? rowColor = null)
    {
        var headerClr = headerColor ?? ConsoleColor.White;
        var rowClr = rowColor ?? ConsoleColor.White;

        var itemList = items.Cast<object?>().Where(x => x != null).ToList();

        if (!itemList.Any())
        {
            Console.WriteLine("(no data)");
            return;
        }

        var firstNonNull = itemList.First();
        var itemType = firstNonNull.GetType();

        WriteLine($"Class: {itemType.Name}");

        if (IsSimpleType(itemType))
        {
            string header = "Value";
            int maxLen = Math.Max(header.Length, itemList.Max(x => x?.ToString()?.Length ?? 0));
            string divider = "+" + new string('-', maxLen + 2) + "+";

            WriteLine(divider, headerClr);
            WriteLine("| " + header.PadRight(maxLen) + " |", headerClr);
            WriteLine(divider, headerClr);
            foreach (var item in itemList)
            {
                WriteLine("| " + (item?.ToString() ?? "").PadRight(maxLen) + " |", rowClr);
                WriteLine(divider, headerClr);
            }
            return;
        }

        var props = GetOrderedPropertiesByInheritance(itemType);
        var headers = props.Select(p => p.Name).ToArray();

        var rows = itemList.Select(item =>
        {
            return props.Select(p =>
            {
                try
                {
                    var val = p.GetValue(item);
                    return val?.ToString() ?? "";
                }
                catch
                {
                    return "";
                }
            }).ToArray();
        }).ToList();

        int colCount = headers.Length;
        int[] maxWidths = new int[colCount];

        for (int i = 0; i < colCount; i++)
        {
            int headerWidth = headers[i].Length;
            int maxCell = rows.Select(r => r[i]?.Length ?? 0).DefaultIfEmpty(0).Max();
            maxWidths[i] = Math.Max(headerWidth, maxCell);
        }

        string tableDivider = "+" + string.Join("+", maxWidths.Select(w => new string('-', w + 2))) + "+";

        void WriteRow(IEnumerable<string> cols, ConsoleColor? fg)
        {
            var cells = cols.Select((col, idx) => " " + col.PadRight(maxWidths[idx]) + " ");
            Write("|" + string.Join("|", cells) + "|", fg);
            Console.WriteLine();
        }

        WriteLine(tableDivider, headerClr);
        WriteRow(headers, headerClr);
        WriteLine(tableDivider, headerClr);
        foreach (var row in rows)
        {
            WriteRow(row, rowClr);
            WriteLine(tableDivider, headerClr);
        }
    }

    private static bool IsSimpleType(Type type)
    {
        return type.IsPrimitive || type.IsEnum ||
               type == typeof(string) ||
               type == typeof(DateTime) ||
               type == typeof(decimal);
    }

    private static List<PropertyInfo> GetOrderedPropertiesByInheritance(Type type)
    {
        var props = new List<PropertyInfo>();
        var typeStack = new Stack<Type>();

        while (type != null && type != typeof(object))
        {
            typeStack.Push(type);
            type = type.BaseType!;
        }

        while (typeStack.Count > 0)
        {
            var t = typeStack.Pop();
            props.AddRange(t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
        }

        return props;
    }
}
