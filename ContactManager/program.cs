using System;

namespace ContactManager
{
    class Program
    {
        static void Main(string[] args)
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
