using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics; // to calculate run time
using System.IO; // for file operations and Path.Combine


public class Class1D
    {
        public int capacity;
        public int count;
        public int[] array;

        public Class1D() 
        {
            capacity = 3;
            count = 0;
            array = new int[capacity];
        }
        public void Add(int value)
        {
            if (count< capacity)
            {
               array[count] = value;
                count++;
            }

            else
            {
                capacity = capacity * 2;
                int[] temp = new int[capacity];

                for(int i=0 ; i<count ; i++)
                {
                    temp[i] = array[i];
                }
                temp[count] = value;
                array = temp;
                count++;

            }

            
        }


        public void Print()
        {
            for(int i = 0;i < count; i++)
            {
                Console.WriteLine(array[i]);
            }
        }
}

    // Contacts Class
   

    public class Class2D
    {
        public int rows;
        public int columns;
        public int count;
        public string[,] table;

        // Constructor
        public Class2D()
        {
            rows = 3;
            columns = 4;
            count = 0;
            table = new string[rows, columns];
        }

        // Method to return the number of rows in the 2D array
        public int CountRows()
        {
            return table.GetLength(0);  // GetLength(0) returns the number of rows in the 2D array
        }

        /*---------------------------------------------------------------------------------------------------------------------------*/
        /* CRUD Section Open */

        // Add Contacts
        public void Add(string Cname, string Ctpno, string Cgroup, string Ccity)
        {
            if (count < rows)
            {
                table[count, 0] = Cname;
                table[count, 1] = Ctpno;
                table[count, 2] = Cgroup;
                table[count, 3] = Ccity;
                count++;
            }
            else
            {
                rows = rows * 2;
                string[,] temp = new string[rows, columns];

                for (int i = 0; i < count; i++)
                {
                    temp[i, 0] = table[i, 0];
                    temp[i, 1] = table[i, 1];
                    temp[i, 2] = table[i, 2];
                    temp[i, 3] = table[i, 3];
                }
                temp[count, 0] = Cname;
                temp[count, 1] = Ctpno;
                temp[count, 2] = Cgroup;
                temp[count, 3] = Ccity;
                table = temp;
                count++;
            }
        }

        // Print Contacts
        public void Print() // Print Contacts
        {
            UI ui = new UI();
            
            if (count == 0)
            {
                ui.ShowWarningMessage("No contacts to display. Add some contacts first!");
                return;
            }
            
            var terminalManager = TerminalManager.Instance;
            var layout = terminalManager.GetLayoutRecommendation();
            
            // Use different display methods based on screen size
            switch (layout.ScreenSize)
            {
                case ResponsiveUI.ScreenSize.Small:
                    PrintContactsAsCards();
                    break;
                case ResponsiveUI.ScreenSize.Medium:
                    PrintContactsAsCompactTable();
                    break;
                default:
                    PrintContactsAsFullTable();
                    break;
            }
            
            Console.WriteLine();
            ui.ShowSuccessMessage($"Displayed {count} contacts.");
        }
        
        private void PrintContactsAsCards()
        {
            Console.WriteLine();
            for (int i = 0; i < count; i++)
            {
                ResponsiveUI.PrintContactCard(table[i, 0], table[i, 1], table[i, 2], table[i, 3]);
                if (i < count - 1)
                {
                    Console.WriteLine(); // Space between cards
                }
            }
        }
        
        private void PrintContactsAsCompactTable()
        {
            // Prepare data for responsive table
            string[] headers = { "Name", "Phone", "Group", "City" };
            string[][] data = new string[count][];
            
            for (int i = 0; i < count; i++)
            {
                data[i] = new string[] { table[i, 0], table[i, 1], table[i, 2], table[i, 3] };
            }
            
            ResponsiveUI.PrintResponsiveTable(headers, data);
        }
        
        private void PrintContactsAsFullTable()
        {
            int consoleWidth = ResponsiveUI.GetSafeConsoleWidth();
            int tableWidth = Math.Min(consoleWidth - 4, 80); // Leave margin and cap width
            
            // Calculate column widths dynamically
            int nameWidth = Math.Max(12, tableWidth / 4);
            int phoneWidth = Math.Max(12, tableWidth / 4);
            int groupWidth = Math.Max(8, tableWidth / 6);
            int cityWidth = tableWidth - nameWidth - phoneWidth - groupWidth - 3; // 3 for separators
            
            // Calculate left padding to center-align the table
            int leftPadding = Math.Max(0, (consoleWidth - tableWidth) / 2);
            string padding = new string(' ', leftPadding);

            // Create dynamic format string
            string headerFormat = $"{{0,-{nameWidth}}} {{1,-{phoneWidth}}} {{2,-{groupWidth}}} {{3,-{cityWidth}}}";
            string separator = new string('─', tableWidth);

            // Print the header with column names
            Console.WriteLine(padding + "┌" + separator + "┐");
            Console.WriteLine(padding + "│" + string.Format(headerFormat, 
                ResponsiveUI.TruncateText("Name", nameWidth),
                ResponsiveUI.TruncateText("Phone", phoneWidth),
                ResponsiveUI.TruncateText("Group", groupWidth),
                ResponsiveUI.TruncateText("City", cityWidth)) + "│");
            Console.WriteLine(padding + "├" + separator + "┤");

            // Print the table data
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(padding + "│" + string.Format(headerFormat,
                    ResponsiveUI.TruncateText(table[i, 0], nameWidth),
                    ResponsiveUI.TruncateText(table[i, 1], phoneWidth),
                    ResponsiveUI.TruncateText(table[i, 2], groupWidth),
                    ResponsiveUI.TruncateText(table[i, 3], cityWidth)) + "│");
            }
            
            Console.WriteLine(padding + "└" + separator + "┘");
        }
        // Delete Contacts
        public void delete(string name)
        {
            UI ui = new UI();
            for (int i = 0; i < count; i++)
            {
                if (table[i, 0] == name)
                {
                    // Shift all elements after the deleted contact
                    for (int j = i; j < count - 1; j++)
                    {
                        table[j, 0] = table[j + 1, 0];
                        table[j, 1] = table[j + 1, 1];
                        table[j, 2] = table[j + 1, 2];
                        table[j, 3] = table[j + 1, 3];
                    }
                    count--;
                    ui.ShowSuccessMessage($"Contact '{name}' deleted successfully!");
                    return;
                }
            }
            ui.ShowErrorMessage($"Contact '{name}' not found.");
        }

        // Update Contacts
        public void update(string name)
        {
            UI ui = new UI();
            for (int i = 0; i < count; i++)
            {
                if (table[i, 0] == name)
                {
                    ui.AddSpacing(1);
                    ui.Center($"Current details for: {name}");
                    ui.line();
                    
                    // Show current details using responsive form layout
                    ui.DisplayFormField("Current Name", table[i, 0]);
                    ui.DisplayFormField("Current Phone", table[i, 1]);
                    ui.DisplayFormField("Current Group", table[i, 2]);
                    ui.DisplayFormField("Current City", table[i, 3]);
                    
                    ui.line();
                    ui.AddSpacing(1);
                    
                    ui.Center("Enter new details:");
                    ui.AddSpacing(1);
                    
                    // Get new details with validation
                    string newName = ui.GetValidatedInput("Enter new Name", 
                        input => !string.IsNullOrWhiteSpace(input), 
                        "Name cannot be empty");
                    
                    string newPhone = ui.GetValidatedInput("Enter new Phone Number", 
                        input => !string.IsNullOrWhiteSpace(input), 
                        "Phone number cannot be empty");
                    
                    string newGroup = ui.GetValidatedInput("Enter new Group", 
                        input => !string.IsNullOrWhiteSpace(input), 
                        "Group cannot be empty");
                    
                    string newCity = ui.GetValidatedInput("Enter new City", 
                        input => !string.IsNullOrWhiteSpace(input), 
                        "City cannot be empty");

                    // Update the contact
                    table[i, 0] = newName;
                    table[i, 1] = newPhone;
                    table[i, 2] = newGroup;
                    table[i, 3] = newCity;
                    table[i, 3] = newCity;
                    
                    ui.ShowSuccessMessage("Contact updated successfully!");
                    return;
                }
            }
            
            // Contact not found
            ui.ShowErrorMessage($"Contact '{name}' not found.");
        }

        /* CRUD Section Close */
        /*---------------------------------------------------------------------------------------------------------------------------*/


        /*---------------------------------------------------------------------------------------------------------------------------*/
        /* Searching Section Open by linear search */

        // Search Contacts by Name (Default)           linear search
        public void searchname(string name)
        {
            UI ui = new UI();
            List<int> foundIndices = new List<int>();
            
            // Find all matching contacts
            for (int i = 0; i < count; i++)
            {
                if (table[i, 0].Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    foundIndices.Add(i);
                }
            }
            
            if (foundIndices.Count == 0)
            {
                ui.AddSpacing(1);
                ui.ShowWarningMessage($"No contact found with name: {name}");
                ui.AddSpacing(1);
                return;
            }
            
            // Display results using responsive layout
            DisplaySearchResults(foundIndices, $"Search Results for Name: {name}");
        }

        // Search Contacts by Group                    linear search
        public void searchgroup(string group)
        {
            UI ui = new UI();
            List<int> foundIndices = new List<int>();
            
            // Find all matching contacts
            for (int i = 0; i < count; i++)
            {
                if (table[i, 2].Equals(group, StringComparison.OrdinalIgnoreCase))
                {
                    foundIndices.Add(i);
                }
            }
            
            if (foundIndices.Count == 0)
            {
                ui.AddSpacing(1);
                ui.ShowWarningMessage($"No contacts found in group: {group}");
                ui.AddSpacing(1);
                return;
            }
            
            // Display results using responsive layout
            DisplaySearchResults(foundIndices, $"Search Results for Group: {group}");
        }

        // Search Contacts by City                     linear search
        public void searchcity(string city)
        {
            UI ui = new UI();
            List<int> foundIndices = new List<int>();
            
            // Find all matching contacts
            for (int i = 0; i < count; i++)
            {
                if (table[i, 3].Equals(city, StringComparison.OrdinalIgnoreCase))
                {
                    foundIndices.Add(i);
                }
            }
            
            if (foundIndices.Count == 0)
            {
                ui.AddSpacing(1);
                ui.ShowWarningMessage($"No contacts found in city: {city}");
                ui.AddSpacing(1);
                return;
            }
            
            // Display results using responsive layout
            DisplaySearchResults(foundIndices, $"Search Results for City: {city}");
        }

        // Search Contacts by Phone                    linear search
        public void searchphone(string phone)
        {
            UI ui = new UI();
            List<int> foundIndices = new List<int>();
            
            // Find all matching contacts (exact match for phone)
            for (int i = 0; i < count; i++)
            {
                if (table[i, 1].Equals(phone, StringComparison.OrdinalIgnoreCase))
                {
                    foundIndices.Add(i);
                }
            }
            
            if (foundIndices.Count == 0)
            {
                ui.AddSpacing(1);
                ui.ShowWarningMessage($"No contact found with phone: {phone}");
                ui.AddSpacing(1);
                return;
            }
            
            // Display results using responsive layout
            DisplaySearchResults(foundIndices, $"Search Results for Phone: {phone}");
        }
        
        // Helper method to display search results responsively
        private void DisplaySearchResults(List<int> indices, string title)
        {
            UI ui = new UI();
            var terminalManager = TerminalManager.Instance;
            var layout = terminalManager.GetLayoutRecommendation();
            
            ui.AddSpacing(1);
            ui.Center(title);
            ui.line();
            ui.AddSpacing(1);
            
            // Use different display methods based on screen size
            switch (layout.ScreenSize)
            {
                case ResponsiveUI.ScreenSize.Small:
                    DisplaySearchResultsAsCards(indices);
                    break;
                case ResponsiveUI.ScreenSize.Medium:
                    DisplaySearchResultsAsCompactTable(indices);
                    break;
                default:
                    DisplaySearchResultsAsFullTable(indices);
                    break;
            }
            
            ui.AddSpacing(1);
            ui.ShowSuccessMessage($"Found {indices.Count} contact(s).");
        }
        
        private void DisplaySearchResultsAsCards(List<int> indices)
        {
            foreach (int i in indices)
            {
                ResponsiveUI.PrintContactCard(table[i, 0], table[i, 1], table[i, 2], table[i, 3]);
                Console.WriteLine();
            }
        }
        
        private void DisplaySearchResultsAsCompactTable(List<int> indices)
        {
            string[] headers = { "Name", "Phone", "Group", "City" };
            string[][] data = new string[indices.Count][];
            
            for (int j = 0; j < indices.Count; j++)
            {
                int i = indices[j];
                data[j] = new string[] { table[i, 0], table[i, 1], table[i, 2], table[i, 3] };
            }
            
            ResponsiveUI.PrintResponsiveTable(headers, data);
        }
        
        private void DisplaySearchResultsAsFullTable(List<int> indices)
        {
            int consoleWidth = ResponsiveUI.GetSafeConsoleWidth();
            int tableWidth = Math.Min(consoleWidth - 4, 80);
            
            // Calculate column widths dynamically
            int nameWidth = Math.Max(12, tableWidth / 4);
            int phoneWidth = Math.Max(12, tableWidth / 4);
            int groupWidth = Math.Max(8, tableWidth / 6);
            int cityWidth = tableWidth - nameWidth - phoneWidth - groupWidth - 3;
            
            int leftPadding = Math.Max(0, (consoleWidth - tableWidth) / 2);
            string padding = new string(' ', leftPadding);
            string headerFormat = $"{{0,-{nameWidth}}} {{1,-{phoneWidth}}} {{2,-{groupWidth}}} {{3,-{cityWidth}}}";
            string separator = new string('─', tableWidth);

            // Print the header with column names
            Console.WriteLine(padding + "┌" + separator + "┐");
            Console.WriteLine(padding + "│" + string.Format(headerFormat, 
                ResponsiveUI.TruncateText("Name", nameWidth),
                ResponsiveUI.TruncateText("Phone", phoneWidth),
                ResponsiveUI.TruncateText("Group", groupWidth),
                ResponsiveUI.TruncateText("City", cityWidth)) + "│");
            Console.WriteLine(padding + "├" + separator + "┤");

            // Print matching contacts
            foreach (int i in indices)
            {
                Console.WriteLine(padding + "│" + string.Format(headerFormat,
                    ResponsiveUI.TruncateText(table[i, 0], nameWidth),
                    ResponsiveUI.TruncateText(table[i, 1], phoneWidth),
                    ResponsiveUI.TruncateText(table[i, 2], groupWidth),
                    ResponsiveUI.TruncateText(table[i, 3], cityWidth)) + "│");
            }
            
            Console.WriteLine(padding + "└" + separator + "┘");
        }

     /*  // Search Contacts by Group and Copy           linear search
        public void search_and_copybygroup(string group)
        {
            string[,] temp = new string[rows, columns];
            int tempcount = 0;

            for (int i = 0; i < count; i++)
            {
                if (table[i, 2] == group)
                {
                    temp[tempcount, 0] = table[i, 0];
                    temp[tempcount, 1] = table[i, 1];
                    temp[tempcount, 2] = table[i, 2];
                    temp[tempcount, 3] = table[i, 3];
                    tempcount++;
                }
            }

            Console.WriteLine("{0,-20} {1,-15} {2,-10} {3,-20}", "Name", "Phone Number", "Group", "City");
            Console.WriteLine(new string('-', 65)); // Add a separator line for better readability

            for (int i = 0; i < tempcount; i++)
            {
                Console.WriteLine("{0,-20} {1,-15} {2,-10} {3,-20}",
                temp[i, 0], temp[i, 1], temp[i, 2], temp[i, 3]);
            }
        }

        // Search Contacts by City and Copy             linear search
        public void search_and_copybycity(string city) 
        {
            string[,] temp = new string[rows, columns];
            int tempcount = 0;

            for (int i = 0; i < count; i++)
            {
                if (table[i, 3] == city)
                {
                    temp[tempcount, 0] = table[i, 0];
                    temp[tempcount, 1] = table[i, 1];
                    temp[tempcount, 2] = table[i, 2];
                    temp[tempcount, 3] = table[i, 3];
                    tempcount++;
                }
            }

            Console.WriteLine("{0,-20} {1,-15} {2,-10} {3,-20}", "Name", "Phone Number", "Group", "City");
            Console.WriteLine(new string('-', 65)); // Add a separator line for better readability

            for (int i = 0; i < tempcount; i++)
            {
                Console.WriteLine("{0,-20} {1,-15} {2,-10} {3,-20}",
                    temp[i, 0], temp[i, 1], temp[i, 2], temp[i, 3]);
            }
        }

*/
    /* Searching Section Close by linear search */




    /* Searching Section Open by binear search */
    /*=============================================================================================================================================================================*/
        private void SortByName()
        {
            for (int i = 0; i < count - 1; i++)
            {
                for (int j = 0; j < count - i - 1; j++)
                {
                    if (string.Compare(table[j, 0], table[j + 1, 0], StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        // Swap rows
                        for (int k = 0; k < columns; k++)
                        {
                            string temp = table[j, k];
                            table[j, k] = table[j + 1, k];
                            table[j + 1, k] = temp;
                        }
                    }
                }
            }
        }
        public void BinarySearchByName(string name) 
        {
            SortByName();
            int left = 0, right = count - 1;
            int consoleWidth = Console.WindowWidth; // Get the console width
            int tableWidth = 65; // Total table width (adjusted for column formatting)
            int leftPadding = Math.Max(0, (consoleWidth - tableWidth) / 2);
            


            while (left <= right)
            {
                
                int mid = left + (right - left) / 2;
                int compare = string.Compare(table[mid, 0], name, StringComparison.OrdinalIgnoreCase);

                if (compare == 0)
                {
                    // Contact found, print details
                    Console.WriteLine("{0,-20} {1,-15} {2,-10} {3,-20}", "Name", "Phone Number", "Group", "City");
                    Console.WriteLine(new string('-', 65)); // Add separator line
                    Console.WriteLine(new string(' ', leftPadding) + "{0,-20} {1,-15} {2,-10} {3,-20}", table[mid, 0], table[mid, 1], table[mid, 2], table[mid, 3]);return;
                }
                else if (compare < 0)
                {
                    left = mid + 1; // Search right half
                }
                else
                {
                    right = mid - 1; // Search left half
                }
            }

            Console.WriteLine("Contact not found.");
        }
    /*=============================================================================================================================================================================*/
        private void SortByGroup()
        {
            for (int i = 0; i < count - 1; i++)
            {
                for (int j = 0; j < count - i - 1; j++)
                {
                    if (string.Compare(table[j, 2], table[j + 1, 2], StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        // Swap rows
                        for (int k = 0; k < columns; k++)
                        {
                            string temp = table[j, k];
                            table[j, k] = table[j + 1, k];
                            table[j + 1, k] = temp;
                        }
                    }
                }
            }
        }

        public void BinarySearchByGroup(string group)
        {
            SortByGroup(); // Ensure the table is sorted by Group before searching

            int left = 0, right = count - 1;
            int firstMatch = -1, lastMatch = -1;

            // Binary search to find one occurrence
            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                int compare = string.Compare(table[mid, 2], group, StringComparison.OrdinalIgnoreCase);

                if (compare == 0)
                {
                    firstMatch = lastMatch = mid;

                    // Expand left to find first matching element
                    while (firstMatch > 0 && string.Compare(table[firstMatch - 1, 2], group, StringComparison.OrdinalIgnoreCase) == 0)
                        firstMatch--;

                    // Expand right to find last matching element
                    while (lastMatch < count - 1 && string.Compare(table[lastMatch + 1, 2], group, StringComparison.OrdinalIgnoreCase) == 0)
                        lastMatch++;

                    break;
                }
                else if (compare < 0)
                {
                    left = mid + 1; // Search right half
                }
                else
                {
                    right = mid - 1; // Search left half
                }
            }

            // If no match is found
            if (firstMatch == -1)
            {
                Console.WriteLine("No contacts found in Group: " + group);
                return;
            }

            // Print all matching contacts
            Console.WriteLine("\nContacts found in Group: " + group);
            int consoleWidth = Console.WindowWidth; // Get the console width
            int tableWidth = 65; // Total table width (adjusted for column formatting)
            int leftPadding = Math.Max(0, (consoleWidth - tableWidth) / 2);
            string separator = new string('-', tableWidth);
            Console.WriteLine(new string(' ', leftPadding) + "{0,-20} {1,-15} {2,-10} {3,-20}", "Name", "Phone Number", "Group", "City");
            Console.WriteLine(new string(' ', leftPadding) + separator);


            for (int i = firstMatch; i <= lastMatch; i++)
            {
                Console.WriteLine(new string(' ', leftPadding) + "{0,-20} {1,-15} {2,-10} {3,-20}", table[i, 0], table[i, 1], table[i, 2], table[i, 3]);
            
            }
        }

    /*=============================================================================================================================================================================*/


    private void SortByCity()
    {
        for (int i = 0; i < count - 1; i++)
        {
            for (int j = 0; j < count - i - 1; j++)
            {
                if (string.Compare(table[j, 3], table[j + 1, 3], StringComparison.OrdinalIgnoreCase) > 0)
                {
                    // Swap rows
                    for (int k = 0; k < columns; k++)
                    {
                        string temp = table[j, k];
                        table[j, k] = table[j + 1, k];
                        table[j + 1, k] = temp;
                    }
                }
            }
        }
    }

    public void BinarySearchByCity(string city)
    {
        SortByCity(); // Ensure the table is sorted by City before searching

        int left = 0, right = count - 1;
        int firstMatch = -1, lastMatch = -1;

        // Binary search to find one occurrence
        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            int compare = string.Compare(table[mid, 3], city, StringComparison.OrdinalIgnoreCase);

            if (compare == 0)
            {
                firstMatch = lastMatch = mid;

                // Expand left to find first matching element
                while (firstMatch > 0 && string.Compare(table[firstMatch - 1, 3], city, StringComparison.OrdinalIgnoreCase) == 0)
                    firstMatch--;

                // Expand right to find last matching element
                while (lastMatch < count - 1 && string.Compare(table[lastMatch + 1, 3], city, StringComparison.OrdinalIgnoreCase) == 0)
                    lastMatch++;

                break;
            }
            else if (compare < 0)
            {
                left = mid + 1; // Search right half
            }
            else
            {
                right = mid - 1; // Search left half
            }
        }

        // If no match is found
        if (firstMatch == -1)
        {
            Console.WriteLine("-No contacts found in City: " + city);
            return;
        }

        // Print all matching contacts
            int consoleWidth = Console.WindowWidth; // Get the console width
            int tableWidth = 65; // Total table width (adjusted for column formatting)
            int leftPadding = Math.Max(0, (consoleWidth - tableWidth) / 2);
            string separator = new string('-', tableWidth);
            Console.WriteLine(new string(' ', leftPadding) + "{0,-20} {1,-15} {2,-10} {3,-20}", "Name", "Phone Number", "Group", "City");
            Console.WriteLine(new string(' ', leftPadding) + separator);

        for (int i = firstMatch; i <= lastMatch; i++)
        {
            Console.WriteLine(new string(' ', leftPadding) + "{0,-20} {1,-15} {2,-10} {3,-20}", table[i, 0], table[i, 1], table[i, 2], table[i, 3]);
            
        }
    }
    /*=============================================================================================================================================================================*/

    /* Searching Section close by binear search */

    /*---------------------------------------------------------------------------------------------------------------------------*/

    // Export Contacts
    public void export()                                                            
    {
        UI ui = new UI();
        try
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Contacts.txt");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
            {
                file.WriteLine("{0,-20} {1,-15} {2,-10} {3,-20}", "Name", "Phone Number", "Group", "City");
                file.WriteLine(new string('-', 65)); // Add a separator line for better readability

                for (int i = 0; i < count; i++)
                {
                    file.WriteLine("{0,-20} {1,-15} {2,-10} {3,-20}", 
                        table[i, 0], table[i, 1], table[i, 2], table[i, 3]);
                }
            }
            ui.ShowSuccessMessage($"Successfully exported {count} contacts to: {path}");
        }
        catch (Exception ex)
        {
            ui.ShowErrorMessage($"Export failed: {ex.Message}");
        }
    }
        // Import Data from text file
    public void load_data_fromtxt()
    {
        UI ui = new UI();
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Contacts.txt");

        // Check if file exists before trying to read it
        if (!File.Exists(path))
        {
            ui.ShowErrorMessage($"Contacts file not found at: {path}");
            ui.ShowWarningMessage("Please make sure the Contacts.txt file exists in the project directory.");
            return;
        }

        try
        {
            // Read all lines from the file
            string[] lines = System.IO.File.ReadAllLines(path);
            int originalCount = count; // Store original count to calculate imported contacts

            // Start processing from the 3rd line (assuming the first two lines are headers)
            for (int i = 2; i < lines.Length; i++)
            {
                // Skip empty lines
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                // Split the line into columns using multiple spaces as delimiter
                string[] words = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Skip lines with insufficient data
                if (words.Length < 4) continue;

                // Ensure the array has enough rows to accommodate new data
                if (count >= rows)
                {
                    rows *= 2; // Double the size of rows
                    string[,] temp = new string[rows, columns];

                    // Copy existing data to the new array
                    for (int j = 0; j < count; j++)
                    {
                        for (int k = 0; k < columns; k++)
                        {
                            temp[j, k] = table[j, k];
                        }
                    }
                    table = temp;
                }

                // Add the imported data to the dynamic array
                table[count, 0] = words[0]; // Name
                table[count, 1] = words[1]; // Phone Number
                table[count, 2] = words[2]; // Group
                table[count, 3] = words[3]; // City
                count++;
            }
            
            int importedCount = count - originalCount;
            if (importedCount > 0)
            {
                ui.ShowSuccessMessage($"Successfully imported {importedCount} contacts from: {path}");
            }
            else
            {
                ui.ShowWarningMessage("No new contacts were imported. File may be empty or contain invalid data.");
            }
        }
        catch (Exception ex)
        {
            ui.ShowErrorMessage($"Import failed: {ex.Message}");
        }
    }

        /*---------------------------------------------------------------------------------------------------------------------------
           Sorting Functions 
          ---------------------------------------------------------------------------------------------------------------------------*/

        // 1) Bubble Sort
        public void BubbleSort()
        {
            // Measure runtime
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < count - 1; i++)
            {
                for (int j = 0; j < count - i - 1; j++)
                {
                    // Compare table[j, 0] and table[j+1, 0]
                    if (String.Compare(table[j, 0], table[j + 1, 0]) > 0)
                    {
                        // Swap the entire row
                        for (int k = 0; k < columns; k++)
                        {
                            string temp = table[j, k];
                            table[j, k] = table[j + 1, k];
                            table[j + 1, k] = temp;
                        }
                    }
                }
            }

            stopwatch.Stop();
            UI uI1 = new UI();
            uI1.Center($"Runtime: {stopwatch.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine();
        }

        // 2) Insertion Sort
        public void InsertionSort()
        {
             // Measure runtime
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 1; i < count; i++)
            {
                // Save the current row in temporary variables
                string keyName = table[i, 0];
                string keyPhone = table[i, 1];
                string keyGroup = table[i, 2];
                string keyCity = table[i, 3];

                int j = i - 1;

                // Move elements of table[0..i-1] that are greater than keyName
                // to one position ahead of their current position
                while (j >= 0 && String.Compare(table[j, 0], keyName) > 0)
                {
                    table[j + 1, 0] = table[j, 0];
                    table[j + 1, 1] = table[j, 1];
                    table[j + 1, 2] = table[j, 2];
                    table[j + 1, 3] = table[j, 3];
                    j--;
                }

                // Place the saved row at the correct position
                table[j + 1, 0] = keyName;
                table[j + 1, 1] = keyPhone;
                table[j + 1, 2] = keyGroup;
                table[j + 1, 3] = keyCity;
            }
            stopwatch.Stop();
            UI uI1 = new UI();
            uI1.Center($"Runtime: {stopwatch.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine();
        }

        // 3) Selection Sort
        public void SelectionSort()
        {
            // Measure runtime
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < count - 1; i++)
            {
                // Find the minimum element in table[i..count-1]
                int minIndex = i;
                for (int j = i + 1; j < count; j++)
                {
                    if (String.Compare(table[j, 0], table[minIndex, 0]) < 0)
                    {
                        minIndex = j;
                    }
                }

                // Swap the found minimum row with the i-th row
                if (minIndex != i)
                {
                    for (int k = 0; k < columns; k++)
                    {
                        string temp = table[i, k];
                        table[i, k] = table[minIndex, k];
                        table[minIndex, k] = temp;
                    }
                }
            }
            stopwatch.Stop();
            UI uI1 = new UI();
            uI1.Center($"Runtime: {stopwatch.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine();
        }

        // 4) Quick Sort
        public void QuickSort()
        {
            // Measure runtime
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            QuickSortHelper(0, count - 1);

            stopwatch.Stop();
            UI uI1 = new UI();
            uI1.Center($"Runtime: {stopwatch.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine();
        }

        private void QuickSortHelper(int low, int high)
        {
            if (low < high)
            {
                int pivotIndex = Partition(low, high);
                QuickSortHelper(low, pivotIndex - 1);
                QuickSortHelper(pivotIndex + 1, high);
            }
        }

        private int Partition(int low, int high)
        {
            // Choose the rightmost element as pivot
            string pivot = table[high, 0];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                // If current element is smaller than pivot
                if (String.Compare(table[j, 0], pivot) < 0) 
                {
                    i++;
                    // Swap entire rows i and j
                    for (int k = 0; k < columns; k++)
                    {
                        string temp = table[i, k];
                        table[i, k] = table[j, k];
                        table[j, k] = temp;
                    }
                }
            }

            // Swap the pivot row with row (i+1)
            int pivotIndex = i + 1;
            for (int k = 0; k < columns; k++)
            {
                string temp = table[pivotIndex, k];
                table[pivotIndex, k] = table[high, k];
                table[high, k] = temp;
            }

            return pivotIndex;
        }



        // 5) Merge Sort
        public void MergeSort()
        {
            // Measure runtime
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            MergeSortHelper(0, count - 1);

            stopwatch.Stop();
            UI uI1 = new UI();
            uI1.Center($"Runtime: {stopwatch.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine();

        }

        private void MergeSortHelper(int left, int right)
        {
            if (left < right)
            {
                int mid = (left + right) / 2;
                MergeSortHelper(left, mid);
                MergeSortHelper(mid + 1, right);
                Merge(left, mid, right);
            }
        }

        private void Merge(int left, int mid, int right)
        {
            int n1 = mid - left + 1;
            int n2 = right - mid;

            // Create temp arrays
            string[,] L = new string[n1, columns];
            string[,] R = new string[n2, columns];

            // Copy data to temp arrays L and R
            for (int i = 0; i < n1; i++)
            {
                for (int c = 0; c < columns; c++)
                {
                    L[i, c] = table[left + i, c];
                }
            }

            for (int j = 0; j < n2; j++)
            {
                for (int c = 0; c < columns; c++)
                {
                    R[j, c] = table[mid + 1 + j, c];
                }
            }

            // Merge the temp arrays back into table[left..right]
            int iIndex = 0;  // Initial index of first subarray
            int jIndex = 0;  // Initial index of second subarray
            int k = left;    // Initial index of merged subarray

            while (iIndex < n1 && jIndex < n2)
            {
                if (String.Compare(L[iIndex, 0], R[jIndex, 0]) <= 0)
                {
                    for (int c = 0; c < columns; c++)
                    {
                        table[k, c] = L[iIndex, c];
                    }
                    iIndex++;
                }
                else
                {
                    for (int c = 0; c < columns; c++)
                    {
                        table[k, c] = R[jIndex, c];
                    }
                    jIndex++;
                }
                k++;
            }

            // Copy the remaining elements of L[], if there are any
            while (iIndex < n1)
            {
                for (int c = 0; c < columns; c++)
                {
                    table[k, c] = L[iIndex, c];
                }
                iIndex++;
                k++;
            }

            // Copy the remaining elements of R[], if there are any
            while (jIndex < n2)
            {
                for (int c = 0; c < columns; c++)
                {
                    table[k, c] = R[jIndex, c];
                }
                jIndex++;
                k++;
            }
        }
    }

