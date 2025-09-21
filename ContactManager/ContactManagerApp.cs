using System;
using System.Collections.Generic;
using System.Linq;

public class ContactManagerApp
{
    private Class2D contacts;
    private NavigationManager nav;
    private UI ui;
    private TerminalManager terminalManager;

    public ContactManagerApp(Class2D contacts, NavigationManager nav)
    {
        this.contacts = contacts;
        this.nav = nav;
        this.ui = new UI();
        this.terminalManager = TerminalManager.Instance;
        
        // Initialize responsive UI system
        InitializeResponsiveSystem();
    }
    
    private void InitializeResponsiveSystem()
    {
        // Ensure minimum terminal size
        ResponsiveUI.EnsureMinimumSize();
        
        // Only start monitoring if we're in an external terminal
        if (ExternalTerminalLauncher.IsRunningInExternalTerminal())
        {
            // Start monitoring terminal size changes
            terminalManager.StartMonitoring();
            
            // Subscribe to size change events
            terminalManager.SizeChanged += OnTerminalSizeChanged;
        }
    }
    
    private void OnTerminalSizeChanged(int newWidth, int newHeight)
    {
        // Handle terminal size changes
        // Could refresh current screen or show notification
        // For now, we just update the layout recommendation
        var layout = terminalManager.GetLayoutRecommendation();
    }

    public void Run()
    {
        string currentScreen = "home";
        
        // Show welcome message with terminal info
        ShowWelcomeMessage();
        
        while (true)
        {
            try
            {
                switch (currentScreen.ToLower())
                {
                    case "home":
                        currentScreen = ShowHomeScreen();
                        break;
                    case "add":
                        currentScreen = ShowAddScreen();
                        break;
                    case "search":
                        currentScreen = ShowSearchScreen();
                        break;
                    case "view":
                        currentScreen = ShowViewScreen();
                        break;
                    case "modify":
                        currentScreen = ShowModifyScreen();
                        break;
                    case "delete":
                        currentScreen = ShowDeleteScreen();
                        break;
                    case "import":
                        currentScreen = ShowImportScreen();
                        break;
                    case "export":
                        currentScreen = ShowExportScreen();
                        break;
                    default:
                        currentScreen = "home";
                        break;
                }
            }
            catch (Exception ex)
            {
                ui.ShowErrorMessage($"An error occurred: {ex.Message}");
                ui.ShowWarningMessage("Returning to main menu...");
                System.Threading.Thread.Sleep(2000);
                currentScreen = "home";
            }
        }
    }
    
    private void ShowWelcomeMessage()
    {
        var layout = terminalManager.GetLayoutRecommendation();
        bool isExternal = ExternalTerminalLauncher.IsRunningInExternalTerminal();
        
        if (layout.ScreenSize != ResponsiveUI.ScreenSize.Small)
        {
            ui.ClearScreen();
            ui.AddSpacing(1);
            
            if (isExternal)
            {
                ui.Center("🚀 Contact Manager - External Terminal Mode 🚀");
                ui.AddSpacing(1);
                ui.Center("Optimized for the best user experience!");
            }
            else
            {
                ui.Center("📱 Contact Manager - Responsive Mode 📱");
                ui.AddSpacing(1);
                ui.Center("Running in integrated terminal");
            }
            
            ui.AddSpacing(1);
            
            // Show terminal info for debugging (optional)
            if (layout.ScreenSize == ResponsiveUI.ScreenSize.ExtraLarge)
            {
                var info = terminalManager.GetTerminalInfo();
                ResponsiveUI.CenterText($"Terminal: {info.Width}x{info.Height} ({info.ScreenSize})");
                if (isExternal)
                {
                    ResponsiveUI.CenterText("✅ External Terminal Detected");
                }
                else
                {
                    ResponsiveUI.CenterText("ℹ️ Integrated Terminal Detected");
                }
                ui.AddSpacing(1);
            }
            
            ui.ShowLoadingAnimation("Initializing", 1);
        }
    }

    private string ShowHomeScreen()
    {
        nav.PushScreen("home");
        
        ui.ClearScreen();
        ui.ShowLoadingAnimation("Loading....", 1);
        ui.ClearScreen();
        
        // Show breadcrumb navigation for larger screens
        nav.ShowBreadcrumb();
        
        ui.line();
        ui.logo();
        ui.line();
        ui.AddSpacing(2);

        var layout = terminalManager.GetLayoutRecommendation();
        
        List<string> items;
        if (layout.UseCompactMode)
        {
            // Compact menu for small screens
            items = new List<string> { 
                "Add contact ................... 1", 
                "Search contact ................ 2", 
                "View all ...................... 3",
                "Modify contact ................ 4",
                "Delete contact ................ 5",
                "Import data ................... 6",
                "Export data ................... 7"
            };
        }
        else
        {
            // Full menu for larger screens
            items = new List<string> { 
                "- Add contact     ......................................... 1", 
                "- Search contact  ......................................... 2", 
                "- View all        ......................................... 3",
                "- Modify contact  ......................................... 4",
                "- Delete contact  ......................................... 5",
                "- Import data     ......................................... 6",
                "- Export data     ......................................... 7"
            };
        }
        
        ui.ListMaker(7, items);

        ui.AddSpacing(3);
        
        // Show quick help for larger screens
        nav.ShowQuickHelp();
        
        nav.ShowNavigationOptions();

        string? input = Console.ReadLine();
        input = nav.HandleNavigation(input ?? "", ref contacts);

        return input switch
        {
            "1" => "add",
            "2" => "search", 
            "3" => "view",
            "4" => "modify",
            "5" => "delete",
            "6" => "import",
            "7" => "export",
            _ => HandleInvalidInput("home")
        };
    }

    private string ShowAddScreen()
    {
        nav.PushScreen("add");
        
        ui.ClearScreen();
        ui.ShowLoadingAnimation("Loading....", 1);
        ui.ClearScreen();
        
        // Show breadcrumb navigation
        nav.ShowBreadcrumb();
        
        ui.line();
        ui.Center("ADD NEW CONTACT");
        ui.line();
        ui.AddSpacing(2);

        var layout = terminalManager.GetLayoutRecommendation();

        try
        {
            if (layout.UseCompactMode)
            {
                // Compact form for small screens
                ui.Center("Enter contact details:");
                ui.AddSpacing(1);
            }
            else
            {
                ui.Center("Please enter the contact information below:");
                ui.AddSpacing(2);
            }

            string name = ui.GetValidatedInput("Enter Name", 
                input => !string.IsNullOrWhiteSpace(input), 
                "Name cannot be empty");
            
            string phone = ui.GetValidatedInput("Enter Phone Number", 
                input => !string.IsNullOrWhiteSpace(input), 
                "Phone number cannot be empty");
            
            string group = ui.GetValidatedInput("Enter Group", 
                input => !string.IsNullOrWhiteSpace(input), 
                "Group cannot be empty");
            
            string city = ui.GetValidatedInput("Enter City", 
                input => !string.IsNullOrWhiteSpace(input), 
                "City cannot be empty");

            contacts.Add(name, phone, group, city);
            ui.ShowSuccessMessage("Contact added successfully!");
            
            // Show added contact details
            if (!layout.UseCompactMode)
            {
                ui.AddSpacing(1);
                ui.Center("Contact Details:");
                ui.AddSpacing(1);
                ui.DisplayContact(name, phone, group, city);
            }
        }
        catch (Exception ex)
        {
            ui.ShowErrorMessage($"Failed to add contact: {ex.Message}");
        }

        ui.AddSpacing(2);
        
        List<string> options;
        if (layout.UseCompactMode)
        {
            options = new List<string> { 
                "Add Another : 1", 
                "Save : 2",
                "View All : 3"
            };
        }
        else
        {
            options = new List<string> { 
                "Add Another Contact : 1", 
                "Save to File : 2",
                "View All Contacts : 3"
            };
        }
        
        nav.ShowNavigationOptions(options);

        string? input = Console.ReadLine();
        input = nav.HandleNavigation(input ?? "", ref contacts);

        return input switch
        {
            "1" => "add",
            "2" => "export",
            "3" => "view",
            _ => HandleInvalidInput("add")
        };
    }

    private string ShowSearchScreen()
    {
        nav.PushScreen("search");
        
        ui.ClearScreen();
        ui.line();
        ui.Center("SEARCH CONTACT");
        ui.line();
        ui.AddSpacing(2);

        List<string> items = new List<string> { 
            "- Search by name         ......................................... 1", 
            "- Search by number       ......................................... 2", 
            "- Search by group        ......................................... 3",
            "- Search by city         ......................................... 4"
        };
        ui.ListMaker(4, items);

        ui.AddSpacing(3);
        nav.ShowNavigationOptions();

        string? input = Console.ReadLine();
        input = nav.HandleNavigation(input ?? "", ref contacts);

        return input switch
        {
            "1" => ShowSearchByName(),
            "2" => ShowSearchByNumber(),
            "3" => ShowSearchByGroup(),
            "4" => ShowSearchByCity(),
            _ => HandleInvalidInput("search")
        };
    }

    private string ShowViewScreen()
    {
        nav.PushScreen("view");
        
        ui.ClearScreen();
        ui.ShowLoadingAnimation("Loading....", 1);
        ui.ClearScreen();
        ui.line();
        ui.Center("SORT CONTACTS");
        ui.line();
        ui.AddSpacing(2);

        List<string> items = new List<string> { 
            "-View unsorted contact list     ......................................... 1",
            "-Sort with bubble sort          ......................................... 2",
            "-Sort with insertion sort       ......................................... 3",
            "-Sort with selection sort       ......................................... 4",
            "-Sort with merge sort           ......................................... 5",
            "-Sort with quick sort           ......................................... 6"
        };
        ui.ListMaker(6, items);

        ui.AddSpacing(2);
        nav.ShowNavigationOptions();

        string? input = Console.ReadLine();
        input = nav.HandleNavigation(input ?? "", ref contacts);

        if (int.TryParse(input, out int choice) && choice >= 1 && choice <= 6)
        {
            return ShowSortedContacts(choice);
        }

        return HandleInvalidInput("view");
    }

    private string ShowSortedContacts(int sortType)
    {
        ui.ClearScreen();
        ui.ShowLoadingAnimation("Sorting....", 1);
        ui.ClearScreen();
        ui.line();
        ui.Center("CONTACTS");
        ui.line();
        ui.AddSpacing(1);

        try
        {
            switch (sortType)
            {
                case 1:
                    contacts.Print();
                    break;
                case 2:
                    contacts.BubbleSort();
                    contacts.Print();
                    break;
                case 3:
                    contacts.InsertionSort();
                    contacts.Print();
                    break;
                case 4:
                    contacts.SelectionSort();
                    contacts.Print();
                    break;
                case 5:
                    contacts.MergeSort();
                    contacts.Print();
                    break;
                case 6:
                    contacts.QuickSort();
                    contacts.Print();
                    break;
            }
        }
        catch (Exception ex)
        {
            ui.ShowErrorMessage($"Error displaying contacts: {ex.Message}");
        }

        ui.AddSpacing(2);
        
        List<string> options = new List<string> { 
            "Search Contacts : 1"
        };
        nav.ShowNavigationOptions(options);

        string? input = Console.ReadLine();
        input = nav.HandleNavigation(input ?? "", ref contacts);

        return input switch
        {
            "1" => "search",
            _ => HandleInvalidInput("view")
        };
    }

    private string ShowModifyScreen()
    {
        nav.PushScreen("modify");
        
        ui.ClearScreen();
        ui.ShowLoadingAnimation("Loading....", 1);
        ui.ClearScreen();
        ui.line();
        ui.Center("MODIFY CONTACT");
        ui.line();
        ui.AddSpacing(2);

        try
        {
            string name = ui.GetValidatedInput("Enter name of contact to modify", 
                input => !string.IsNullOrWhiteSpace(input), 
                "Name cannot be empty");

            contacts.update(name);
            ui.ShowSuccessMessage("Contact updated successfully!");
        }
        catch (Exception ex)
        {
            ui.ShowErrorMessage($"Failed to modify contact: {ex.Message}");
        }

        ui.AddSpacing(2);
        
        List<string> options = new List<string> { 
            "Add Contact : 1", 
            "Save to File : 2"
        };
        nav.ShowNavigationOptions(options);

        string? input = Console.ReadLine();
        input = nav.HandleNavigation(input ?? "", ref contacts);

        return input switch
        {
            "1" => "add",
            "2" => "export",
            _ => HandleInvalidInput("modify")
        };
    }

    private string ShowDeleteScreen()
    {
        nav.PushScreen("delete");
        
        ui.ClearScreen();
        ui.ShowLoadingAnimation("Loading....", 1);
        ui.ClearScreen();
        ui.line();
        ui.Center("DELETE CONTACT");
        ui.line();
        ui.AddSpacing(2);

        try
        {
            string name = ui.GetValidatedInput("Enter name of contact to delete", 
                input => !string.IsNullOrWhiteSpace(input), 
                "Name cannot be empty");

            if (ui.ShowConfirmation($"Are you sure you want to delete '{name}'?"))
            {
                contacts.delete(name);
                ui.ShowSuccessMessage("Contact deleted successfully!");
            }
            else
            {
                ui.ShowWarningMessage("Delete operation cancelled.");
            }
        }
        catch (Exception ex)
        {
            ui.ShowErrorMessage($"Failed to delete contact: {ex.Message}");
        }

        ui.AddSpacing(2);
        
        List<string> options = new List<string> { 
            "Delete Another : 1", 
            "View All Contacts : 2"
        };
        nav.ShowNavigationOptions(options);

        string? input = Console.ReadLine();
        input = nav.HandleNavigation(input ?? "", ref contacts);

        return input switch
        {
            "1" => "delete",
            "2" => "view",
            _ => HandleInvalidInput("delete")
        };
    }

    private string ShowImportScreen()
    {
        nav.PushScreen("import");
        
        ui.ClearScreen();
        ui.ShowLoadingAnimation("Importing....", 1);
        ui.ClearScreen();
        ui.line();
        ui.Center("IMPORT DATA");
        ui.line();
        ui.AddSpacing(2);

        try
        {
            contacts.load_data_fromtxt();
            ui.ShowSuccessMessage("Data imported successfully!");
        }
        catch (Exception ex)
        {
            ui.ShowErrorMessage($"Failed to import data: {ex.Message}");
        }

        ui.AddSpacing(2);
        nav.ShowNavigationOptions();

        string? input = Console.ReadLine();
        return nav.HandleNavigation(input ?? "", ref contacts);
    }

    private string ShowExportScreen()
    {
        nav.PushScreen("export");
        
        ui.ClearScreen();
        ui.ShowLoadingAnimation("Exporting....", 1);
        ui.ClearScreen();
        ui.line();
        ui.Center("EXPORT DATA");
        ui.line();
        ui.AddSpacing(2);

        try
        {
            contacts.export();
            ui.ShowSuccessMessage("Data exported successfully!");
        }
        catch (Exception ex)
        {
            ui.ShowErrorMessage($"Failed to export data: {ex.Message}");
        }

        ui.AddSpacing(2);
        nav.ShowNavigationOptions();

        string? input = Console.ReadLine();
        return nav.HandleNavigation(input ?? "", ref contacts);
    }

    private string ShowSearchByName()
    {
        nav.PushScreen("search-name");
        
        ui.ClearScreen();
        ui.ShowLoadingAnimation("Loading....", 1);
        ui.ClearScreen();
        ui.line();
        ui.Center("SEARCH BY NAME");
        ui.line();
        ui.AddSpacing(2);

        try
        {
            string name = ui.GetValidatedInput("Enter name to search", 
                input => !string.IsNullOrWhiteSpace(input), 
                "Name cannot be empty");

            ui.AddSpacing(1);
            contacts.searchname(name);
        }
        catch (Exception ex)
        {
            ui.ShowErrorMessage($"Search failed: {ex.Message}");
        }

        ui.AddSpacing(2);
        
        List<string> options = new List<string> { 
            "Add Contact : 1", 
            "Save to File : 2",
            "Search Again : 3"
        };
        nav.ShowNavigationOptions(options);

        string? input = Console.ReadLine();
        input = nav.HandleNavigation(input ?? "", ref contacts);

        return input switch
        {
            "1" => "add",
            "2" => "export",
            "3" => "search",
            _ => HandleInvalidInput("search")
        };
    }

    private string ShowSearchByNumber()
    {
        nav.PushScreen("search-number");
        
        ui.ClearScreen();
        ui.ShowLoadingAnimation("Loading....", 1);
        ui.ClearScreen();
        ui.line();
        ui.Center("SEARCH BY PHONE NUMBER");
        ui.line();
        ui.AddSpacing(2);

        try
        {
            string phone = ui.GetValidatedInput("Enter phone number to search", 
                input => !string.IsNullOrWhiteSpace(input), 
                "Phone number cannot be empty");

            ui.AddSpacing(1);
            contacts.searchphone(phone);
        }
        catch (Exception ex)
        {
            ui.ShowErrorMessage($"Search failed: {ex.Message}");
        }

        ui.AddSpacing(2);
        
        List<string> options = new List<string> { 
            "Add Contact : 1", 
            "Save to File : 2",
            "Search Again : 3"
        };
        nav.ShowNavigationOptions(options);

        string? input = Console.ReadLine();
        input = nav.HandleNavigation(input ?? "", ref contacts);

        return input switch
        {
            "1" => "add",
            "2" => "export",
            "3" => "search",
            _ => HandleInvalidInput("search")
        };
    }

    private string ShowSearchByGroup()
    {
        nav.PushScreen("search-group");
        
        ui.ClearScreen();
        ui.ShowLoadingAnimation("Loading....", 1);
        ui.ClearScreen();
        ui.line();
        ui.Center("SEARCH BY GROUP");
        ui.line();
        ui.AddSpacing(2);

        try
        {
            string group = ui.GetValidatedInput("Enter group to search", 
                input => !string.IsNullOrWhiteSpace(input), 
                "Group cannot be empty");

            ui.AddSpacing(1);
            contacts.searchgroup(group);
        }
        catch (Exception ex)
        {
            ui.ShowErrorMessage($"Search failed: {ex.Message}");
        }

        ui.AddSpacing(2);
        
        List<string> options = new List<string> { 
            "Add Contact : 1", 
            "Save to File : 2",
            "Search Again : 3"
        };
        nav.ShowNavigationOptions(options);

        string? input = Console.ReadLine();
        input = nav.HandleNavigation(input ?? "", ref contacts);

        return input switch
        {
            "1" => "add",
            "2" => "export",
            "3" => "search",
            _ => HandleInvalidInput("search")
        };
    }

    private string ShowSearchByCity()
    {
        nav.PushScreen("search-city");
        
        ui.ClearScreen();
        ui.ShowLoadingAnimation("Loading....", 1);
        ui.ClearScreen();
        ui.line();
        ui.Center("SEARCH BY CITY");
        ui.line();
        ui.AddSpacing(2);

        try
        {
            string city = ui.GetValidatedInput("Enter city to search", 
                input => !string.IsNullOrWhiteSpace(input), 
                "City cannot be empty");

            ui.AddSpacing(1);
            contacts.searchcity(city);
        }
        catch (Exception ex)
        {
            ui.ShowErrorMessage($"Search failed: {ex.Message}");
        }

        ui.AddSpacing(2);
        
        List<string> options = new List<string> { 
            "Add Contact : 1", 
            "Save to File : 2",
            "Search Again : 3"
        };
        nav.ShowNavigationOptions(options);

        string? input = Console.ReadLine();
        input = nav.HandleNavigation(input ?? "", ref contacts);

        return input switch
        {
            "1" => "add",
            "2" => "export",
            "3" => "search",
            _ => HandleInvalidInput("search")
        };
    }

    private string HandleInvalidInput(string currentScreen)
    {
        ui.ShowErrorMessage("Invalid option. Please try again.");
        System.Threading.Thread.Sleep(1500);
        return currentScreen;
    }
}