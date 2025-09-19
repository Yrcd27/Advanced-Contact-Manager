# Contact Manager - DSA Project

A comprehensive contact management system implemented in C# using Data Structures and Algorithms concepts. This project demonstrates various DSA implementations including dynamic arrays, multiple search algorithms, and five different sorting techniques.

## 🚀 Quick Start

### Prerequisites
- .NET 9.0 SDK or later
- Windows PowerShell (for running commands)
- Any C# compatible IDE (Visual Studio, VS Code, etc.)

### Running the Application

```bash
# Navigate to the project directory
cd "ContactManager"

# Build and run the project
dotnet build
dotnet run
```

## 📁 Project Structure

```
ContactManager/
├── ContactManager/              # Main project folder
│   ├── program.cs              # Entry point
│   ├── class.cs                # Core data structures and algorithms
│   ├── ContactManagerApp.cs    # Main application controller
│   ├── NavigationManager.cs    # Navigation and flow control
│   ├── UI.cs                   # User interface utilities
│   ├── firstscreen.cs          # Main menu interface
│   ├── addC.cs                 # Add contact functionality
│   ├── deleteC.cs              # Delete contact functionality
│   ├── modifyC.cs              # Modify contact functionality
│   ├── SearchC.cs              # Search menu interface
│   ├── searchBy*.cs            # Individual search operations
│   ├── viewAll.cs              # View contacts with sorting
│   ├── viewOp.cs               # View options menu
│   ├── Contacts.txt            # Data persistence file
│   └── ContactManager.csproj   # Project configuration
└── README.md                   # This documentation
```

## 🔧 Data Structures & Algorithms Implemented

### Data Structures
- **Dynamic 2D Array**: Self-expanding contact storage with automatic capacity doubling
- **Navigation Stack**: For managing screen navigation and user flow

### Search Algorithms
1. **Linear Search** - O(n) time complexity
   - Search by Name, Phone, Group, City
   - Case-insensitive matching
2. **Binary Search** - O(log n) time complexity  
   - Requires sorted data
   - Implemented for Name, Group, City searches

### Sorting Algorithms (All implemented with runtime measurement)
1. **Bubble Sort** - O(n²) time complexity
2. **Insertion Sort** - O(n²) time complexity  
3. **Selection Sort** - O(n²) time complexity
4. **Quick Sort** - O(n log n) average case
5. **Merge Sort** - O(n log n) guaranteed

## 📋 Features

### Core Functionality
- ✅ **CRUD Operations**: Complete Create, Read, Update, Delete operations
- ✅ **Multiple Search Types**: Name, Phone Number, Group, City
- ✅ **Dual Search Modes**: Linear and Binary search implementations
- ✅ **Five Sorting Algorithms**: With performance metrics
- ✅ **File Persistence**: Import/Export to Contacts.txt
- ✅ **Dynamic Memory Management**: Auto-expanding arrays

### User Experience
- ✅ **Professional UI**: Loading animations, centered layouts
- ✅ **Robust Navigation**: Universal Back/Main Menu/Exit options
- ✅ **Input Validation**: Comprehensive validation with error handling
- ✅ **Colored Feedback**: Success (green), Error (red), Warning (yellow)
- ✅ **Confirmation Dialogs**: For destructive operations
- ✅ **Case-Insensitive Search**: User-friendly search operations

### Technical Features
- ✅ **Memory Efficient**: No recursive object creation, single instances
- ✅ **Error Handling**: Comprehensive try-catch blocks
- ✅ **Cross-Platform Paths**: Uses relative paths for portability
- ✅ **Performance Monitoring**: Runtime measurement for sorting algorithms

## 💾 Data Management

### File Format
The application uses `Contacts.txt` for data persistence with a fixed-width format:

```
Name                 Phone Number    Group      City                
-----------------------------------------------------------------
John_Doe             1234567890      Friends    NewYork
Jane_Smith           0987654321      Family     LosAngeles
```

### Import/Export Operations
- **Export (Option 7)**: Saves all contacts to `Contacts.txt`
- **Import (Option 6)**: Loads contacts from `Contacts.txt`
- **Automatic Expansion**: Dynamic array grows as needed during import
- **Error Handling**: File existence validation and user feedback

## 🎮 User Interface

### Navigation System
- **B**: Go back to previous screen
- **M**: Return to main menu  
- **0**: Exit application (with confirmation)

### Main Menu Options
1. **Add Contact** - Add new contact with validation
2. **View Contacts** - Display with sorting options
3. **Search Contacts** - Multiple search criteria
4. **Update Contact** - Modify existing contact details
5. **Delete Contact** - Remove contact with confirmation
6. **Import Data** - Load contacts from file
7. **Export Data** - Save contacts to file
8. **Exit** - Close application

### Search Options
1. **Search by Name** (Linear/Binary)
2. **Search by Phone Number** (Linear)
3. **Search by Group** (Linear/Binary) 
4. **Search by City** (Linear/Binary)

### Sorting Options
1. **Bubble Sort** with runtime display
2. **Insertion Sort** with runtime display
3. **Selection Sort** with runtime display
4. **Quick Sort** with runtime display
5. **Merge Sort** with runtime display

## 🔬 Algorithm Performance Analysis

### Search Algorithm Comparison
| Algorithm | Time Complexity | Space Complexity | Use Case |
|-----------|----------------|------------------|----------|
| Linear Search | O(n) | O(1) | Unsorted data, all field types |
| Binary Search | O(log n) | O(1) | Sorted data, faster lookups |

### Sorting Algorithm Comparison
| Algorithm | Best Case | Average Case | Worst Case | Space Complexity |
|-----------|-----------|--------------|------------|------------------|
| Bubble Sort | O(n) | O(n²) | O(n²) | O(1) |
| Insertion Sort | O(n) | O(n²) | O(n²) | O(1) |
| Selection Sort | O(n²) | O(n²) | O(n²) | O(1) |
| Quick Sort | O(n log n) | O(n log n) | O(n²) | O(log n) |
| Merge Sort | O(n log n) | O(n log n) | O(n log n) | O(n) |

## 🛠️ Technical Implementation

### Dynamic Array Implementation
```csharp
// Auto-expanding 2D array for contacts
if (count >= rows) {
    rows = rows * 2;  // Double capacity
    string[,] temp = new string[rows, columns];
    // Copy existing data and add new contact
}
```

### Binary Search with Range Finding
```csharp
// Finds all matching entries for group/city searches
while (firstMatch > 0 && table[firstMatch - 1, 2] == target)
    firstMatch--;
while (lastMatch < count - 1 && table[lastMatch + 1, 2] == target)
    lastMatch++;
```

### Runtime Measurement
```csharp
Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();
// Sorting algorithm execution
stopwatch.Stop();
Console.WriteLine($"Runtime: {stopwatch.Elapsed.TotalMilliseconds} ms");
```

## 🚀 Advanced Features

### Memory Management
- **Single Instance Pattern**: Prevents memory leaks from object recreation
- **Navigation Stack**: Efficient screen state management
- **Resource Disposal**: Proper cleanup of file operations

### Error Handling & Validation
- **Input Validation**: Custom validators for all user inputs
- **File Operations**: Comprehensive error handling for import/export
- **User Feedback**: Clear error messages and success confirmations

### User Experience Enhancements
- **Loading Animations**: Visual feedback during operations
- **Colored Messages**: Status-coded feedback system
- **Confirmation Dialogs**: Prevention of accidental data loss
- **Consistent Navigation**: Universal navigation options on every screen

## 📚 Educational Value

This project demonstrates key Computer Science concepts:

1. **Data Structures**: Dynamic arrays, 2D arrays, stack-based navigation
2. **Algorithm Analysis**: Time/space complexity comparison
3. **Search Techniques**: Linear vs Binary search trade-offs  
4. **Sorting Algorithms**: Implementation and performance analysis
5. **Software Engineering**: Modular design, error handling, user experience
6. **Memory Management**: Efficient resource utilization
7. **File I/O**: Data persistence and cross-platform compatibility

## 🎯 Learning Outcomes

- Understanding of dynamic data structures and their growth strategies
- Implementation of classic search and sorting algorithms
- Performance analysis and runtime measurement techniques
- User interface design principles for console applications
- Error handling and input validation best practices
- File I/O operations and data persistence strategies
- Memory management and resource optimization

## 🤝 Usage Examples

### Adding a Contact
1. Select option 1 from main menu
2. Enter contact details (all fields validated)
3. Confirmation message displayed
4. Contact automatically added to dynamic array

### Searching with Binary Search
1. Select option 3 (Search Contacts)
2. Choose binary search option (Name/Group/City)
3. Data automatically sorted before search
4. Results displayed with formatted table

### Performance Comparison
1. Add multiple contacts (test data)
2. Select option 2 (View Contacts)  
3. Try different sorting algorithms
4. Compare runtime measurements displayed

## 🔧 Development Notes

### Architecture Decisions
- **Separation of Concerns**: UI, Logic, and Data layers separated
- **Single Responsibility**: Each class handles specific functionality
- **Error Recovery**: Graceful handling of invalid inputs and file errors
- **Extensibility**: Easy to add new search criteria or sorting algorithms

### Code Quality Features
- Comprehensive error handling with try-catch blocks
- Input validation for all user interactions
- Consistent code formatting and documentation
- Memory-efficient implementation patterns

---

*This project was developed as part of a Data Structures and Algorithms course, demonstrating practical implementation of fundamental CS concepts in a real-world application.*