using System;
using System.Collections.Generic;
using System.Linq;

public class ResponsiveUI
{
    private const int MIN_WIDTH = 40;
    private const int MIN_HEIGHT = 10;
    private const int SMALL_SCREEN = 60;
    private const int MEDIUM_SCREEN = 100;
    private const int LARGE_SCREEN = 140;
    
    // Screen size categories
    public enum ScreenSize
    {
        Small,    // < 60 chars
        Medium,   // 60-100 chars
        Large,    // 100-140 chars
        ExtraLarge // > 140 chars
    }
    
    // Layout breakpoints
    public static ScreenSize GetScreenSize()
    {
        int width = GetSafeConsoleWidth();
        
        if (width < SMALL_SCREEN) return ScreenSize.Small;
        if (width < MEDIUM_SCREEN) return ScreenSize.Medium;
        if (width < LARGE_SCREEN) return ScreenSize.Large;
        return ScreenSize.ExtraLarge;
    }
    
    // Safe console width getter with fallback
    public static int GetSafeConsoleWidth()
    {
        try
        {
            return Math.Max(Console.WindowWidth, MIN_WIDTH);
        }
        catch
        {
            return MIN_WIDTH; // Fallback for environments where WindowWidth isn't available
        }
    }
    
    // Safe console height getter with fallback
    public static int GetSafeConsoleHeight()
    {
        try
        {
            return Math.Max(Console.WindowHeight, MIN_HEIGHT);
        }
        catch
        {
            return MIN_HEIGHT; // Fallback for environments where WindowHeight isn't available
        }
    }
    
    public static void EnsureMinimumSize()
    {
        try
        {
            if (Console.WindowWidth < MIN_WIDTH || Console.WindowHeight < MIN_HEIGHT)
            {
                Console.SetWindowSize(Math.Max(Console.WindowWidth, MIN_WIDTH), 
                                    Math.Max(Console.WindowHeight, MIN_HEIGHT));
            }
        }
        catch
        {
            // Ignore if console size cannot be set
        }
    }
    
    public static string TruncateText(string text, int maxWidth)
    {
        if (string.IsNullOrEmpty(text)) return "";
        if (maxWidth <= 3) return "...";
        if (text.Length <= maxWidth) return text;
        return text.Substring(0, maxWidth - 3) + "...";
    }
    
    // Smart text wrapping that preserves word boundaries
    public static List<string> WrapText(string text, int maxWidth)
    {
        var lines = new List<string>();
        if (string.IsNullOrEmpty(text)) return lines;
        
        var words = text.Split(' ');
        var currentLine = "";
        
        foreach (var word in words)
        {
            if (string.IsNullOrEmpty(currentLine))
            {
                currentLine = word;
            }
            else if (currentLine.Length + 1 + word.Length <= maxWidth)
            {
                currentLine += " " + word;
            }
            else
            {
                lines.Add(currentLine);
                currentLine = word;
            }
        }
        
        if (!string.IsNullOrEmpty(currentLine))
        {
            lines.Add(currentLine);
        }
        
        return lines;
    }
    
    // Adaptive spacing based on screen size
    public static int GetAdaptiveSpacing()
    {
        return GetScreenSize() switch
        {
            ScreenSize.Small => 1,
            ScreenSize.Medium => 2,
            ScreenSize.Large => 3,
            ScreenSize.ExtraLarge => 4,
            _ => 2
        };
    }
    
    // Get optimal column count for layout
    public static int GetOptimalColumnCount(int itemCount, int minItemWidth = 20)
    {
        int width = GetSafeConsoleWidth();
        int maxCols = Math.Max(1, width / minItemWidth);
        return Math.Min(itemCount, maxCols);
    }
    
    // Enhanced responsive table with better formatting
    public static void PrintResponsiveTable(string[] headers, string[][] data)
    {
        int width = GetSafeConsoleWidth();
        int colCount = headers.Length;
        ScreenSize screenSize = GetScreenSize();
        
        // Adjust table layout based on screen size
        switch (screenSize)
        {
            case ScreenSize.Small:
                PrintVerticalTable(headers, data);
                break;
            case ScreenSize.Medium:
                PrintCompactTable(headers, data, width);
                break;
            default:
                PrintFullTable(headers, data, width);
                break;
        }
    }
    
    private static void PrintVerticalTable(string[] headers, string[][] data)
    {
        int width = GetSafeConsoleWidth() - 4;
        
        foreach (var row in data)
        {
            Console.WriteLine("┌" + new string('─', width) + "┐");
            for (int i = 0; i < Math.Min(row.Length, headers.Length); i++)
            {
                string header = TruncateText(headers[i], width / 3);
                string value = TruncateText(row[i] ?? "", width - header.Length - 3);
                Console.WriteLine($"│ {header}: {value}".PadRight(width + 1) + "│");
            }
            Console.WriteLine("└" + new string('─', width) + "┘");
            Console.WriteLine();
        }
    }
    
    private static void PrintCompactTable(string[] headers, string[][] data, int width)
    {
        int colCount = headers.Length;
        int colWidth = Math.Max(8, (width - colCount - 1) / colCount);
        
        // Print headers
        Console.Write("┌");
        for (int i = 0; i < colCount; i++)
        {
            Console.Write(new string('─', colWidth));
            if (i < colCount - 1) Console.Write("┬");
        }
        Console.WriteLine("┐");
        
        Console.Write("│");
        foreach (var header in headers)
        {
            Console.Write(TruncateText(header, colWidth).PadRight(colWidth) + "│");
        }
        Console.WriteLine();
        
        // Print separator
        Console.Write("├");
        for (int i = 0; i < colCount; i++)
        {
            Console.Write(new string('─', colWidth));
            if (i < colCount - 1) Console.Write("┼");
        }
        Console.WriteLine("┤");
        
        // Print data
        foreach (var row in data)
        {
            Console.Write("│");
            for (int i = 0; i < Math.Min(row.Length, headers.Length); i++)
            {
                Console.Write(TruncateText(row[i] ?? "", colWidth).PadRight(colWidth) + "│");
            }
            Console.WriteLine();
        }
        
        // Bottom border
        Console.Write("└");
        for (int i = 0; i < colCount; i++)
        {
            Console.Write(new string('─', colWidth));
            if (i < colCount - 1) Console.Write("┴");
        }
        Console.WriteLine("┘");
    }
    
    private static void PrintFullTable(string[] headers, string[][] data, int width)
    {
        PrintCompactTable(headers, data, width); // Same as compact for now
    }
    
    // Enhanced responsive menu with better styling
    public static void PrintResponsiveMenu(string[] options)
    {
        int width = GetSafeConsoleWidth();
        ScreenSize screenSize = GetScreenSize();
        
        switch (screenSize)
        {
            case ScreenSize.Small:
                PrintVerticalMenu(options, width);
                break;
            case ScreenSize.Medium:
                PrintGridMenu(options, width, 2);
                break;
            case ScreenSize.Large:
                PrintGridMenu(options, width, 3);
                break;
            case ScreenSize.ExtraLarge:
                PrintGridMenu(options, width, 4);
                break;
        }
    }
    
    private static void PrintVerticalMenu(string[] options, int width)
    {
        foreach (var option in options)
        {
            string truncated = TruncateText(option, width - 4);
            Console.WriteLine($"  {truncated}");
        }
    }
    
    private static void PrintGridMenu(string[] options, int width, int columns)
    {
        int itemWidth = (width - (columns + 1)) / columns;
        
        for (int i = 0; i < options.Length; i += columns)
        {
            Console.Write("  ");
            for (int j = 0; j < columns && i + j < options.Length; j++)
            {
                string item = TruncateText(options[i + j], itemWidth);
                Console.Write(item.PadRight(itemWidth));
                if (j < columns - 1 && i + j + 1 < options.Length)
                {
                    Console.Write(" │ ");
                }
            }
            Console.WriteLine();
        }
    }
    
    // Responsive contact card display
    public static void PrintContactCard(string name, string phone, string group, string city)
    {
        int width = GetSafeConsoleWidth();
        ScreenSize screenSize = GetScreenSize();
        
        switch (screenSize)
        {
            case ScreenSize.Small:
                PrintCompactContactCard(name, phone, group, city, width);
                break;
            default:
                PrintExpandedContactCard(name, phone, group, city, width);
                break;
        }
    }
    
    private static void PrintCompactContactCard(string name, string phone, string group, string city, int width)
    {
        int cardWidth = Math.Min(width - 4, 40);
        
        Console.WriteLine("┌" + new string('─', cardWidth) + "┐");
        Console.WriteLine($"│ {TruncateText(name, cardWidth - 2).PadRight(cardWidth - 2)} │");
        Console.WriteLine($"│ ☎ {TruncateText(phone, cardWidth - 5).PadRight(cardWidth - 5)} │");
        Console.WriteLine($"│ ⚑ {TruncateText(group, cardWidth - 5).PadRight(cardWidth - 5)} │");
        Console.WriteLine($"│ ⌖ {TruncateText(city, cardWidth - 5).PadRight(cardWidth - 5)} │");
        Console.WriteLine("└" + new string('─', cardWidth) + "┘");
    }
    
    private static void PrintExpandedContactCard(string name, string phone, string group, string city, int width)
    {
        int cardWidth = Math.Min(width - 4, 60);
        
        Console.WriteLine("┌" + new string('─', cardWidth) + "┐");
        Console.WriteLine($"│ Name:  {TruncateText(name, cardWidth - 9).PadRight(cardWidth - 9)} │");
        Console.WriteLine($"│ Phone: {TruncateText(phone, cardWidth - 9).PadRight(cardWidth - 9)} │");
        Console.WriteLine($"│ Group: {TruncateText(group, cardWidth - 9).PadRight(cardWidth - 9)} │");
        Console.WriteLine($"│ City:  {TruncateText(city, cardWidth - 9).PadRight(cardWidth - 9)} │");
        Console.WriteLine("└" + new string('─', cardWidth) + "┘");
    }
    
    // Responsive progress bar
    public static void PrintProgressBar(int current, int total, string label = "")
    {
        int width = GetSafeConsoleWidth();
        int barWidth = Math.Max(10, width - 20); // Reserve space for percentage and label
        
        double percentage = (double)current / total;
        int filledWidth = (int)(percentage * barWidth);
        
        string bar = "[" + 
                    new string('█', filledWidth) + 
                    new string('░', barWidth - filledWidth) + 
                    "]";
        
        string percentText = $" {percentage:P0}";
        string fullLine = $"{label} {bar} {percentText}";
        
        Console.Write($"\r{TruncateText(fullLine, width)}");
    }
    
    // Center text with proper boundary checking
    public static void CenterText(string text, char fillChar = ' ')
    {
        int width = GetSafeConsoleWidth();
        string displayText = TruncateText(text, width - 2);
        int padding = Math.Max(0, (width - displayText.Length) / 2);
        Console.WriteLine(new string(fillChar, padding) + displayText);
    }
    
    // Create responsive divider lines
    public static void PrintDivider(char character = '-', int? customWidth = null)
    {
        int width = customWidth ?? GetSafeConsoleWidth();
        Console.WriteLine(new string(character, Math.Max(1, width)));
    }
    
    // Responsive form layout
    public static void PrintFormField(string label, string value = "", bool isInput = false)
    {
        int width = GetSafeConsoleWidth();
        ScreenSize screenSize = GetScreenSize();
        
        if (screenSize == ScreenSize.Small)
        {
            // Stack label and input vertically on small screens
            Console.WriteLine(TruncateText(label, width - 2));
            if (isInput)
            {
                Console.Write("> ");
            }
            else if (!string.IsNullOrEmpty(value))
            {
                Console.WriteLine($"  {TruncateText(value, width - 4)}");
            }
        }
        else
        {
            // Show label and input horizontally on larger screens
            int labelWidth = Math.Min(20, width / 3);
            int valueWidth = width - labelWidth - 4;
            
            string truncatedLabel = TruncateText(label, labelWidth);
            if (isInput)
            {
                Console.Write($"{truncatedLabel.PadRight(labelWidth)}: ");
            }
            else
            {
                string truncatedValue = TruncateText(value, valueWidth);
                Console.WriteLine($"{truncatedLabel.PadRight(labelWidth)}: {truncatedValue}");
            }
        }
    }
}