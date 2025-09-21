using System;

namespace ContactManager
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Check if we should show launcher options
                bool showLauncher = args.Length == 0 && !ExternalTerminalLauncher.IsRunningInExternalTerminal();
                
                if (showLauncher)
                {
                    // Show launcher options when running from VS Code
                    ExternalTerminalLauncher.ShowLaunchOptions();
                }
                
                // Continue with normal application startup
                RunApplication();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }
        
        private static void RunApplication()
        {
            // Create single instances to avoid memory leaks
            Class2D contacts = new Class2D();
            NavigationManager nav = new NavigationManager();
            
            // Initialize and start the application
            ContactManagerApp app = new ContactManagerApp(contacts, nav);
            app.Run();
        }
    }
}
