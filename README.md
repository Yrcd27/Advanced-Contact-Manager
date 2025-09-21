# Contact Manager - Advanced DSA Project

A comprehensive **Data Structures & Algorithms** implementation in C# featuring dynamic arrays, multiple search/sort algorithms, and performance analysis with a responsive console interface.

## 🚀 Tech Stack
- **C# .NET 9.0** - Core application framework
- **Dynamic 2D Arrays** - Self-expanding contact storage with capacity doubling
- **Multiple Search Algorithms** - Linear O(n) and Binary O(log n) implementations
- **5 Sorting Algorithms** - Complete comparison with runtime analysis
- **Performance Monitoring** - Real-time algorithm efficiency measurement
- **Cross-Platform Console** - Enhanced UI with terminal adaptation

## 🎯 Quick Start

### Run Options
```bash
# Smart launcher (detects VS Code and offers external terminal)
dotnet run

# External terminal (Visual Studio-like window)
.\LaunchExternal.ps1

# Traditional method
dotnet build && dotnet run
```

## 🆕 Core DSA Features

### � **Dynamic Data Structures**
- **Auto-expanding 2D Arrays**: Capacity doubling when full (O(1) amortized insertion)
- **Memory Management**: Efficient space utilization with minimal waste
- **Navigation Stack**: Stack-based screen management for user flow
- **Real-time Growth**: Dynamic allocation based on contact count

### 🔍 **Search Algorithm Implementation**
- **Linear Search O(n)**: Sequential search through unsorted data
- **Binary Search O(log n)**: Efficient search on sorted datasets
- **Multi-field Support**: Name, Phone, Group, City search capabilities
- **Algorithm Comparison**: Side-by-side performance analysis

### ⚡ **Sorting Algorithm Suite**
- **Bubble Sort O(n²)**: Simple comparison-based sorting with optimizations
- **Insertion Sort O(n²)**: Efficient for small datasets, adaptive behavior
- **Selection Sort O(n²)**: In-place sorting with minimal memory usage
- **Quick Sort O(n log n)**: Divide-and-conquer with pivot partitioning
- **Merge Sort O(n log n)**: Stable sorting with guaranteed performance

### 📊 **Performance Analysis System**
- **Runtime Measurement**: Precise timing using `Stopwatch` class
- **Algorithm Comparison**: Real-time performance metrics display
- **Complexity Visualization**: Best/Average/Worst case analysis
- **Memory Usage Tracking**: Space complexity demonstration

### 🎮 **Application Implementation**
- **Complete CRUD Operations**: Add, Read, Update, Delete with data structure integration
- **File I/O Management**: Persistent storage with dynamic array serialization
- **Input Validation**: Robust error handling and data integrity checks
- **User Interface**: Responsive console design that adapts to terminal capabilities

## 💡 DSA Concepts Demonstrated

### Data Structure Operations
1. **Dynamic Array Growth**: Automatic capacity doubling when threshold reached
2. **Memory Allocation**: Efficient copying and expansion strategies  
3. **Search Implementation**: Linear vs Binary search with sorted data prerequisites
4. **Sort Analysis**: Comparative performance measurement across 5 algorithms

### Algorithm Analysis Practice
- **Time Complexity**: Practical demonstration of O(n), O(log n), O(n²), O(n log n)
- **Space Complexity**: Memory usage patterns for different algorithm types
- **Best/Worst Cases**: Real scenarios showing algorithm behavior variations
- **Performance Measurement**: Empirical analysis using precise timing

## 🔬 Algorithm Performance Analysis

### Search Algorithm Efficiency
| Algorithm | Time Complexity | Space Complexity | Best Use Case |
|-----------|----------------|------------------|---------------|
| **Linear Search** | O(n) | O(1) | Unsorted data, all field types |
| **Binary Search** | O(log n) | O(1) | Sorted data, faster lookups |

### Sorting Algorithm Comparison
| Algorithm | Best Case | Average Case | Worst Case | Space Complexity | Stability |
|-----------|-----------|--------------|------------|------------------|-----------|
| **Bubble Sort** | O(n) | O(n²) | O(n²) | O(1) | Stable |
| **Insertion Sort** | O(n) | O(n²) | O(n²) | O(1) | Stable |
| **Selection Sort** | O(n²) | O(n²) | O(n²) | O(1) | Unstable |
| **Quick Sort** | O(n log n) | O(n log n) | O(n²) | O(log n) | Unstable |
| **Merge Sort** | O(n log n) | O(n log n) | O(n log n) | O(n) | Stable |

### Runtime Measurement Features
- **Precise Timing**: `Stopwatch` class for microsecond accuracy
- **Comparative Analysis**: Side-by-side algorithm performance
- **Scalability Testing**: Performance across different dataset sizes
- **Memory Profiling**: Dynamic array expansion monitoring

## 🛠️ DSA Implementation Details

### Dynamic Array Architecture
```csharp
// Auto-expanding 2D array with capacity doubling
if (count >= rows) {
    rows = rows * 2;  // Double capacity for O(1) amortized
    string[,] temp = new string[rows, columns];
    // Efficient copying of existing data
    Array.Copy(contacts, temp, contacts.Length);
}
```

### Binary Search Implementation
```csharp
// O(log n) search with range finding for duplicates
int left = 0, right = count - 1;
while (left <= right) {
    int mid = left + (right - left) / 2;
    int comparison = string.Compare(contacts[mid, field], target);
    if (comparison == 0) return FindAllMatches(mid, target);
    else if (comparison < 0) left = mid + 1;
    else right = mid - 1;
}
```

### Performance Monitoring System
```csharp
// Precise algorithm timing and analysis
Stopwatch stopwatch = Stopwatch.StartNew();
PerformSortingAlgorithm(data);
stopwatch.Stop();
Console.WriteLine($"Algorithm: {name} | Runtime: {stopwatch.ElapsedMilliseconds}ms | Comparisons: {comparisonCount}");
```

## 🎯 DSA Learning Outcomes
- **Dynamic Data Structures**: Understanding array expansion, memory management, and amortized analysis
- **Algorithm Complexity**: Practical experience with time/space complexity analysis and measurement
- **Search Techniques**: Implementation and comparison of linear vs binary search algorithms
- **Sorting Algorithms**: Comprehensive study of 5 different sorting approaches with performance metrics
- **Performance Analysis**: Real-world algorithm timing, comparison, and optimization strategies
- **Data Structure Design**: Efficient contact storage, retrieval, and manipulation systems

---

*Comprehensive DSA project demonstrating practical implementation of fundamental Computer Science algorithms and data structures with empirical performance analysis.*