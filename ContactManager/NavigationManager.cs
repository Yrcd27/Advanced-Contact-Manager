using System;
using System.Collections.Generic;

public class NavigationManager
{
    private Stack<string> navigationStack = new Stack<string>();
    private UI ui = new UI();
    private TerminalManager terminalManager = TerminalManager.Instance;

    public void PushScreen(string screenName)
    {
        navigationStack.Push(screenName);
    }

    public string PopScreen()
    {
        if (navigationStack.Count > 1)
        {
            navigationStack.Pop(); // Remove current screen
            return navigationStack.Peek(); // Return previous screen
        }
        return "home"; // Default to home if stack is empty
    }

    public bool CanGoBack()
    {
        return navigationStack.Count > 1;
    }

    public void ClearStack()
    {
        navigationStack.Clear();
    }

    public void ShowNavigationOptions(List<string>? customOptions = null)
    {
        ui.line();
        
        List<string> options = new List<string>();
        
        if (customOptions != null)
        {
            options.AddRange(customOptions);
        }
        
        if (CanGoBack())
        {
            options.Add("Back to Previous Screen : B");
        }
        
        options.Add("Main Menu : M");
        options.Add("Exit : 0");

        // Use responsive menu display
        DisplayResponsiveNavigationMenu(options);
        ui.line();
    }
    
    private void DisplayResponsiveNavigationMenu(List<string> options)
    {
        var layout = terminalManager.GetLayoutRecommendation();
        
        switch (layout.SuggestedMenuStyle)
        {
            case MenuStyle.Vertical:
                DisplayVerticalNavigationMenu(options);
                break;
            case MenuStyle.TwoColumn:
                DisplayColumnNavigationMenu(options, 2);
                break;
            case MenuStyle.ThreeColumn:
                DisplayColumnNavigationMenu(options, 3);
                break;
            case MenuStyle.Horizontal:
                DisplayHorizontalNavigationMenu(options);
                break;
            default:
                DisplayVerticalNavigationMenu(options);
                break;
        }
    }
    
    private void DisplayVerticalNavigationMenu(List<string> options)
    {
        var layout = terminalManager.GetLayoutRecommendation();
        int maxWidth = layout.MaxContentWidth;
        
        foreach (var option in options)
        {
            if (layout.UseCompactMode)
            {
                // Extract key and action for compact display
                string[] parts = option.Split(" : ");
                if (parts.Length >= 2)
                {
                    string action = parts[0].Trim();
                    string key = parts[1].Trim();
                    Console.WriteLine($"  [{key}] {ResponsiveUI.TruncateText(action, maxWidth - 8)}");
                }
                else
                {
                    Console.WriteLine($"  {ResponsiveUI.TruncateText(option, maxWidth - 4)}");
                }
            }
            else
            {
                Console.WriteLine($"  {ResponsiveUI.TruncateText(option, maxWidth - 4)}");
            }
        }
    }
    
    private void DisplayColumnNavigationMenu(List<string> options, int columns)
    {
        var layout = terminalManager.GetLayoutRecommendation();
        int maxWidth = layout.MaxContentWidth;
        int itemWidth = (maxWidth - (columns * 3)) / columns;
        
        for (int i = 0; i < options.Count; i += columns)
        {
            Console.Write("  ");
            for (int j = 0; j < columns && i + j < options.Count; j++)
            {
                string option = options[i + j];
                string displayText;
                
                if (layout.UseCompactMode)
                {
                    // Extract key for compact display
                    string[] parts = option.Split(" : ");
                    if (parts.Length >= 2)
                    {
                        string action = parts[0].Trim();
                        string key = parts[1].Trim();
                        displayText = $"[{key}] {ResponsiveUI.TruncateText(action, itemWidth - 6)}";
                    }
                    else
                    {
                        displayText = ResponsiveUI.TruncateText(option, itemWidth);
                    }
                }
                else
                {
                    displayText = ResponsiveUI.TruncateText(option, itemWidth);
                }
                
                Console.Write(displayText.PadRight(itemWidth));
                if (j < columns - 1 && i + j + 1 < options.Count)
                {
                    Console.Write(" │ ");
                }
            }
            Console.WriteLine();
        }
    }
    
    private void DisplayHorizontalNavigationMenu(List<string> options)
    {
        var layout = terminalManager.GetLayoutRecommendation();
        int maxWidth = layout.MaxContentWidth;
        
        List<string> displayOptions = new List<string>();
        
        foreach (var option in options)
        {
            if (layout.UseCompactMode)
            {
                // Extract key for compact display
                string[] parts = option.Split(" : ");
                if (parts.Length >= 2)
                {
                    string key = parts[1].Trim();
                    displayOptions.Add($"[{key}]");
                }
                else
                {
                    displayOptions.Add(ResponsiveUI.TruncateText(option, 10));
                }
            }
            else
            {
                displayOptions.Add(ResponsiveUI.TruncateText(option, 20));
            }
        }
        
        string horizontalMenu = "  " + string.Join(" │ ", displayOptions);
        Console.WriteLine(ResponsiveUI.TruncateText(horizontalMenu, maxWidth));
    }

    public string HandleNavigation(string input, ref Class2D contacts)
    {
        switch (input.ToUpper())
        {
            case "B":
                if (CanGoBack())
                {
                    return PopScreen();
                }
                break;
            case "M":
                ClearStack();
                return "home";
            case "0":
                ShowExitConfirmation();
                break;
        }
        return input; // Return original input if not a navigation command
    }

    private void ShowExitConfirmation()
    {
        var layout = terminalManager.GetLayoutRecommendation();
        
        ui.ClearScreen();
        ui.line();
        ui.Center("EXIT CONFIRMATION");
        ui.line();
        ui.AddSpacing(1);
        
        if (layout.UseCompactMode)
        {
            Console.WriteLine("Exit? (Y/N)");
        }
        else
        {
            ui.Center("Are you sure you want to exit?");
            ui.AddSpacing(1);
            ui.Center("This will close the Contact Manager application.");
            ui.AddSpacing(1);
        }
        
        ui.line();
        Console.Write("Your choice (Y/N): ");
        
        string? confirmation = Console.ReadLine();
        if (confirmation?.ToUpper() == "Y" || confirmation?.ToUpper() == "YES")
        {
            ShowExitMessage();
            Environment.Exit(0);
        }
    }
    
    private void ShowExitMessage()
    {
        var layout = terminalManager.GetLayoutRecommendation();
        
        ui.ClearScreen();
        
        if (layout.UseCompactMode)
        {
            ui.Center("Thanks for using CM!");
        }
        else
        {
            ui.AddSpacing(2);
            ui.Center("Thank you for using Contact Manager!");
            ui.AddSpacing(1);
            ui.Center("Have a great day!");
            ui.AddSpacing(2);
        }
        
        System.Threading.Thread.Sleep(1500); // Show message briefly
    }
    
    // Show breadcrumb navigation for larger screens
    public void ShowBreadcrumb()
    {
        var layout = terminalManager.GetLayoutRecommendation();
        
        if (layout.ScreenSize != ResponsiveUI.ScreenSize.Small && navigationStack.Count > 1)
        {
            var screens = navigationStack.ToArray();
            Array.Reverse(screens); // Reverse to show in correct order
            
            string breadcrumb = string.Join(" > ", screens.Select(s => CapitalizeFirst(s)));
            Console.WriteLine($"Navigation: {ResponsiveUI.TruncateText(breadcrumb, layout.MaxContentWidth)}");
            Console.WriteLine();
        }
    }
    
    private string CapitalizeFirst(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;
        
        return char.ToUpper(input[0]) + input.Substring(1).ToLower();
    }
    
    // Show quick help for navigation keys
    public void ShowQuickHelp()
    {
        var layout = terminalManager.GetLayoutRecommendation();
        
        if (layout.ScreenSize != ResponsiveUI.ScreenSize.Small)
        {
            Console.WriteLine();
            ResponsiveUI.CenterText("Quick Help: [M] Main Menu • [B] Back • [0] Exit");
            Console.WriteLine();
        }
    }
}