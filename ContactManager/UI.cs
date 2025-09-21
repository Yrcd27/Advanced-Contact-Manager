using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class UI
{
    private TerminalManager terminalManager;
    
    public UI()
    {
        terminalManager = TerminalManager.Instance;
    }
    
    //Logo Function
    public void logo()  // display the logo
    {
        ResponsiveUI.EnsureMinimumSize();
        
        string text = @" ╔═╗╔═╗╔╗╔╔╦╗╔═╗╔═╗╔╦╗    ╔╦╗╔═╗╔╗╔╔═╗╔═╗╔═╗╦═╗
 ║  ║ ║║║║ ║ ╠═╣║   ║     ║║║╠═╣║║║╠═╣║ ╦║╣ ╠╦╝
╚═╝╚═╝╝╚╝ ╩ ╩ ╩╚═╝ ╩     ╩ ╩╩ ╩╝╚╝╩ ╩╚═╝╚═╝╩╚═";
        
        string compactText = "CONTACT MANAGER";
        string miniText = "CM";

        // Set the font color to green
        Console.ForegroundColor = ConsoleColor.Green;

        // Get the console width and screen size
        var screenSize = ResponsiveUI.GetScreenSize();
        int consoleWidth = ResponsiveUI.GetSafeConsoleWidth();

        // Use different versions based on screen size
        switch (screenSize)
        {
            case ResponsiveUI.ScreenSize.Small:
                if (consoleWidth < 20)
                {
                    ResponsiveUI.CenterText(miniText);
                }
                else
                {
                    ResponsiveUI.CenterText(compactText);
                }
                break;
            case ResponsiveUI.ScreenSize.Medium:
                ResponsiveUI.CenterText(compactText);
                break;
            default:
                // Split the text into lines and center each one
                string[] lines = text.Split('\n');
                foreach (var line in lines)
                {
                    ResponsiveUI.CenterText(line);
                }
                break;
        }

        // Reset the console color to the default color after printing
        Console.ResetColor();
    }

    // Display line function with responsive width
    public void line(){     // display line
        ResponsiveUI.PrintDivider('-');
    }

    // Function that clears the Console screen with size detection
    public void ClearScreen()   // screen clear
    {
        terminalManager.ClearScreen();
    }

    // Make and print responsive vertical list
    public void ListMaker(int numElements, List<string> items) 
    {
        if (items.Count != numElements)
        {
            Console.WriteLine("Error: The number of items does not match the specified number of elements.");
            return;
        }

        var layout = terminalManager.GetLayoutRecommendation();
        int maxWidth = layout.MaxContentWidth;
        
        foreach (var item in items)
        {
            if (layout.UseCompactMode)
            {
                // Ultra-compact for very small screens
                string[] parts = item.Split(new string[] { "........................................" }, StringSplitOptions.None);
                if (parts.Length >= 2)
                {
                    string label = parts[0].Trim().TrimStart('-', ' ');
                    string number = parts[1].Trim();
                    Console.WriteLine($"{number}. {ResponsiveUI.TruncateText(label, maxWidth - 4)}");
                }
                else
                {
                    Console.WriteLine($"  {ResponsiveUI.TruncateText(item, maxWidth - 2)}");
                }
            }
            else
            {
                // Regular formatting for larger screens
                string displayItem = ResponsiveUI.TruncateText(item, maxWidth);
                
                if (layout.ScreenSize == ResponsiveUI.ScreenSize.Small)
                {
                    // Left-align for small screens
                    Console.WriteLine($"  {displayItem}");
                }
                else
                {
                    // Center-align for larger screens
                    ResponsiveUI.CenterText(displayItem);
                }
            }
        }
    }

    // Make and print responsive horizontal/grid list
    public void ListBar(int numElements, List<string> items)    
    {
        if (items.Count != numElements)
        {
            Console.WriteLine("Error: The number of items does not match the specified number of elements.");
            return;
        }

        var layout = terminalManager.GetLayoutRecommendation();
        
        switch (layout.SuggestedMenuStyle)
        {
            case MenuStyle.Vertical:
                PrintVerticalMenu(items, layout.MaxContentWidth);
                break;
            case MenuStyle.TwoColumn:
                PrintColumnMenu(items, 2, layout.MaxContentWidth);
                break;
            case MenuStyle.ThreeColumn:
                PrintColumnMenu(items, 3, layout.MaxContentWidth);
                break;
            case MenuStyle.Horizontal:
                PrintHorizontalMenu(items, layout.MaxContentWidth);
                break;
            default:
                PrintVerticalMenu(items, layout.MaxContentWidth);
                break;
        }
    }

    private void PrintVerticalMenu(List<string> items, int maxWidth)
    {
        foreach (var item in items)
        {
            Console.WriteLine($"  {ResponsiveUI.TruncateText(item, maxWidth - 4)}");
        }
    }

    private void PrintColumnMenu(List<string> items, int columns, int maxWidth)
    {
        int itemWidth = (maxWidth - (columns * 3)) / columns;
        
        for (int i = 0; i < items.Count; i += columns)
        {
            Console.Write("  ");
            for (int j = 0; j < columns && i + j < items.Count; j++)
            {
                string item = ResponsiveUI.TruncateText(items[i + j], itemWidth);
                Console.Write(item.PadRight(itemWidth));
                if (j < columns - 1 && i + j + 1 < items.Count)
                {
                    Console.Write(" │ ");
                }
            }
            Console.WriteLine();
        }
    }

    private void PrintHorizontalMenu(List<string> items, int maxWidth)
    {
        var truncatedItems = items.Select(item => ResponsiveUI.TruncateText(item, 20)).ToArray();
        string horizontalList = "  " + string.Join(" │ ", truncatedItems);
        Console.WriteLine(ResponsiveUI.TruncateText(horizontalList, maxWidth));
    }

    // Enhanced loading animation with responsive positioning
    public void ShowLoadingAnimation(string message, int durationInSeconds)     
    {
        // Set console text color to green
        Console.ForegroundColor = ConsoleColor.Green;

        var layout = terminalManager.GetLayoutRecommendation();
        string displayMessage = ResponsiveUI.TruncateText(message, layout.MaxContentWidth - 4);

        // Center the loading message
        int padding = Math.Max(0, (ResponsiveUI.GetSafeConsoleWidth() - displayMessage.Length - 2) / 2);
        Console.Write(new string(' ', padding) + displayMessage + " ");

        // Define the animation frames
        char[] animationFrames = { '|', '/', '-', '\\' };

        // Calculate the end time
        DateTime endTime = DateTime.Now.AddSeconds(durationInSeconds);

        int frameIndex = 0;

        while (DateTime.Now < endTime)
        {
            // Display the next frame
            Console.Write(animationFrames[frameIndex]);
            
            // Move the cursor back to overwrite the frame
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);

            // Update the frame index
            frameIndex = (frameIndex + 1) % animationFrames.Length;

            // Wait for a short time before showing the next frame
            Thread.Sleep(100);
        }

        // Reset the text color to default
        Console.ResetColor();
    }

    // Center text using ResponsiveUI
    public void Center(string txt)    
    {
        ResponsiveUI.CenterText(txt);
    }

    // Add responsive spacing
    public void AddSpacing(int lines = 1)
    {
        int adaptiveSpacing = Math.Max(1, Math.Min(lines, terminalManager.GetOptimalSpacing()));
        for (int i = 0; i < adaptiveSpacing; i++)
        {
            Console.WriteLine();
        }
    }

    // Show success message with responsive formatting
    public void ShowSuccessMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        var layout = terminalManager.GetLayoutRecommendation();
        string displayMessage = ResponsiveUI.TruncateText($"✓ {message}", layout.MaxContentWidth);
        
        if (layout.UseCompactMode)
        {
            Console.WriteLine(displayMessage);
        }
        else
        {
            ResponsiveUI.CenterText(displayMessage);
        }
        Console.ResetColor();
    }

    // Show error message with responsive formatting
    public void ShowErrorMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        var layout = terminalManager.GetLayoutRecommendation();
        string displayMessage = ResponsiveUI.TruncateText($"✗ {message}", layout.MaxContentWidth);
        
        if (layout.UseCompactMode)
        {
            Console.WriteLine(displayMessage);
        }
        else
        {
            ResponsiveUI.CenterText(displayMessage);
        }
        Console.ResetColor();
    }

    // Show warning message with responsive formatting
    public void ShowWarningMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        var layout = terminalManager.GetLayoutRecommendation();
        string displayMessage = ResponsiveUI.TruncateText($"⚠ {message}", layout.MaxContentWidth);
        
        if (layout.UseCompactMode)
        {
            Console.WriteLine(displayMessage);
        }
        else
        {
            ResponsiveUI.CenterText(displayMessage);
        }
        Console.ResetColor();
    }

    // Get user input with responsive prompt
    public string GetUserInput(string prompt)
    {
        var layout = terminalManager.GetLayoutRecommendation();
        ResponsiveUI.PrintFormField(prompt, "", true);
        return Console.ReadLine() ?? "";
    }

    // Get validated user input with responsive layout
    public string GetValidatedInput(string prompt, Func<string, bool> validator, string errorMessage)
    {
        string input;
        do
        {
            input = GetUserInput(prompt);
            if (!validator(input))
            {
                ShowErrorMessage(errorMessage);
            }
        } while (!validator(input));
        
        return input;
    }

    // Show confirmation dialog with responsive formatting
    public bool ShowConfirmation(string message)
    {
        Console.WriteLine();
        var layout = terminalManager.GetLayoutRecommendation();
        string prompt = ResponsiveUI.TruncateText($"{message} (Y/N)", layout.MaxContentWidth - 4);
        
        if (layout.UseCompactMode)
        {
            Console.Write($"{prompt}: ");
        }
        else
        {
            ResponsiveUI.CenterText(prompt);
            Console.Write("Your choice: ");
        }
        
        string? response = Console.ReadLine();
        return response?.ToUpper() == "Y" || response?.ToUpper() == "YES";
    }

    // New method: Display responsive contact information
    public void DisplayContact(string name, string phone, string group, string city)
    {
        ResponsiveUI.PrintContactCard(name, phone, group, city);
    }

    // New method: Display progress for operations
    public void ShowProgress(int current, int total, string operation = "Processing")
    {
        ResponsiveUI.PrintProgressBar(current, total, operation);
    }

    // New method: Show terminal info for debugging
    public void ShowTerminalInfo()
    {
        var info = terminalManager.GetTerminalInfo();
        Console.WriteLine(info.ToString());
    }

    // New method: Create responsive form layout
    public void DisplayFormField(string label, string value)
    {
        ResponsiveUI.PrintFormField(label, value, false);
    }

    // New method: Display responsive table
    public void DisplayTable(string[] headers, string[][] data)
    {
        ResponsiveUI.PrintResponsiveTable(headers, data);
    }

    // New method: Create a responsive box/panel
    public void DisplayPanel(string title, List<string> content)
    {
        var layout = terminalManager.GetLayoutRecommendation();
        int width = Math.Min(layout.MaxContentWidth, 80);
        
        // Top border
        Console.WriteLine("┌" + new string('─', width - 2) + "┐");
        
        // Title
        if (!string.IsNullOrEmpty(title))
        {
            string truncatedTitle = ResponsiveUI.TruncateText(title, width - 4);
            int titlePadding = (width - 2 - truncatedTitle.Length) / 2;
            Console.WriteLine($"│{new string(' ', titlePadding)}{truncatedTitle}{new string(' ', width - 2 - titlePadding - truncatedTitle.Length)}│");
            Console.WriteLine("├" + new string('─', width - 2) + "┤");
        }
        
        // Content
        foreach (var line in content)
        {
            var wrappedLines = ResponsiveUI.WrapText(line, width - 4);
            foreach (var wrappedLine in wrappedLines)
            {
                Console.WriteLine($"│ {wrappedLine.PadRight(width - 4)} │");
            }
        }
        
        // Bottom border
        Console.WriteLine("└" + new string('─', width - 2) + "┘");
    }
}
