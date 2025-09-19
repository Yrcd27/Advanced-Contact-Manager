using System;
using System.Collections.Generic;

public class NavigationManager
{
    private Stack<string> navigationStack = new Stack<string>();
    private UI ui = new UI();

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

        ui.ListBar(options.Count, options);
        ui.line();
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
        ui.ClearScreen();
        ui.line();
        ui.Center("EXIT CONFIRMATION");
        ui.line();
        Console.WriteLine();
        ui.Center("Are you sure you want to exit? (Y/N)");
        Console.WriteLine();
        ui.line();
        
        string? confirmation = Console.ReadLine();
        if (confirmation?.ToUpper() == "Y" || confirmation?.ToUpper() == "YES")
        {
            ui.ClearScreen();
            ui.Center("Thank you for using Contact Manager!");
            Console.WriteLine();
            Environment.Exit(0);
        }
    }
}