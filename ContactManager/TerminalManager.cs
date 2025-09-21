using System;
using System.Threading;
using System.Threading.Tasks;

public class TerminalManager
{
    private static TerminalManager? _instance;
    private static readonly object _lock = new object();
    
    private int _lastWidth;
    private int _lastHeight;
    private bool _monitoringEnabled;
    private CancellationTokenSource? _cancellationTokenSource;
    
    // Events for size changes
    public event Action<int, int>? SizeChanged;
    public event Action? OrientationChanged;
    
    private TerminalManager()
    {
        _lastWidth = ResponsiveUI.GetSafeConsoleWidth();
        _lastHeight = ResponsiveUI.GetSafeConsoleHeight();
        _monitoringEnabled = false;
    }
    
    public static TerminalManager Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new TerminalManager();
                    }
                }
            }
            return _instance;
        }
    }
    
    // Properties
    public int CurrentWidth => ResponsiveUI.GetSafeConsoleWidth();
    public int CurrentHeight => ResponsiveUI.GetSafeConsoleHeight();
    public ResponsiveUI.ScreenSize ScreenSize => ResponsiveUI.GetScreenSize();
    public bool IsLandscape => CurrentWidth > CurrentHeight;
    public bool IsPortrait => CurrentHeight > CurrentWidth;
    public bool IsSquare => CurrentWidth == CurrentHeight;
    
    // Start monitoring terminal size changes
    public void StartMonitoring(int intervalMs = 500)
    {
        if (_monitoringEnabled) return;
        
        _monitoringEnabled = true;
        _cancellationTokenSource = new CancellationTokenSource();
        
        Task.Run(async () => await MonitorSizeChanges(intervalMs, _cancellationTokenSource.Token));
    }
    
    // Stop monitoring terminal size changes
    public void StopMonitoring()
    {
        _monitoringEnabled = false;
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
    }
    
    // Force a size check
    public bool CheckForSizeChange()
    {
        int currentWidth = CurrentWidth;
        int currentHeight = CurrentHeight;
        
        if (currentWidth != _lastWidth || currentHeight != _lastHeight)
        {
            bool orientationChanged = (IsLandscape != (_lastWidth > _lastHeight));
            
            _lastWidth = currentWidth;
            _lastHeight = currentHeight;
            
            // Trigger events
            SizeChanged?.Invoke(currentWidth, currentHeight);
            
            if (orientationChanged)
            {
                OrientationChanged?.Invoke();
            }
            
            return true;
        }
        
        return false;
    }
    
    // Get recommended layout for current size
    public LayoutRecommendation GetLayoutRecommendation()
    {
        var screenSize = ScreenSize;
        var width = CurrentWidth;
        var height = CurrentHeight;
        
        return new LayoutRecommendation
        {
            ScreenSize = screenSize,
            SuggestedColumns = GetOptimalColumns(),
            SuggestedSpacing = GetOptimalSpacing(),
            UseCompactMode = screenSize == ResponsiveUI.ScreenSize.Small,
            UseVerticalLayout = IsPortrait || width < 80,
            MaxContentWidth = GetMaxContentWidth(),
            SuggestedMenuStyle = GetOptimalMenuStyle()
        };
    }
    
    // Get optimal number of columns for current screen
    public int GetOptimalColumns()
    {
        return ScreenSize switch
        {
            ResponsiveUI.ScreenSize.Small => 1,
            ResponsiveUI.ScreenSize.Medium => 2,
            ResponsiveUI.ScreenSize.Large => 3,
            ResponsiveUI.ScreenSize.ExtraLarge => 4,
            _ => 2
        };
    }
    
    // Get optimal spacing for current screen
    public int GetOptimalSpacing()
    {
        return ScreenSize switch
        {
            ResponsiveUI.ScreenSize.Small => 1,
            ResponsiveUI.ScreenSize.Medium => 2,
            ResponsiveUI.ScreenSize.Large => 3,
            ResponsiveUI.ScreenSize.ExtraLarge => 4,
            _ => 2
        };
    }
    
    // Get maximum content width (with margins)
    public int GetMaxContentWidth()
    {
        int margin = ScreenSize switch
        {
            ResponsiveUI.ScreenSize.Small => 2,
            ResponsiveUI.ScreenSize.Medium => 4,
            ResponsiveUI.ScreenSize.Large => 8,
            ResponsiveUI.ScreenSize.ExtraLarge => 12,
            _ => 4
        };
        
        return Math.Max(20, CurrentWidth - margin);
    }
    
    // Get optimal menu style for current screen
    public MenuStyle GetOptimalMenuStyle()
    {
        return ScreenSize switch
        {
            ResponsiveUI.ScreenSize.Small => MenuStyle.Vertical,
            ResponsiveUI.ScreenSize.Medium => MenuStyle.TwoColumn,
            ResponsiveUI.ScreenSize.Large => MenuStyle.ThreeColumn,
            ResponsiveUI.ScreenSize.ExtraLarge => MenuStyle.Horizontal,
            _ => MenuStyle.Vertical
        };
    }
    
    // Fit text to screen with word wrapping
    public string[] FitTextToScreen(string text, int? maxWidth = null)
    {
        int width = maxWidth ?? GetMaxContentWidth();
        var lines = ResponsiveUI.WrapText(text, width);
        return lines.ToArray();
    }
    
    // Clear screen with size optimization
    public void ClearScreen()
    {
        try
        {
            Console.Clear();
            
            // Ensure we're working with current size after clear
            CheckForSizeChange();
        }
        catch
        {
            // Fallback: print enough newlines to "clear" screen
            for (int i = 0; i < Math.Max(25, CurrentHeight); i++)
            {
                Console.WriteLine();
            }
        }
    }
    
    // Set window size if possible
    public bool TrySetWindowSize(int width, int height)
    {
        try
        {
            Console.SetWindowSize(
                Math.Max(ResponsiveUI.GetSafeConsoleWidth(), width),
                Math.Max(ResponsiveUI.GetSafeConsoleHeight(), height)
            );
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    // Get terminal info for debugging
    public TerminalInfo GetTerminalInfo()
    {
        return new TerminalInfo
        {
            Width = CurrentWidth,
            Height = CurrentHeight,
            ScreenSize = ScreenSize,
            IsLandscape = IsLandscape,
            IsPortrait = IsPortrait,
            CanSetSize = CanSetWindowSize(),
            BufferWidth = GetBufferWidth(),
            BufferHeight = GetBufferHeight()
        };
    }
    
    // Check if we can set window size
    private bool CanSetWindowSize()
    {
        try
        {
            var originalWidth = Console.WindowWidth;
            var originalHeight = Console.WindowHeight;
            return true; // If we can read, we can probably set
        }
        catch
        {
            return false;
        }
    }
    
    // Get buffer dimensions
    private int GetBufferWidth()
    {
        try
        {
            return Console.BufferWidth;
        }
        catch
        {
            return CurrentWidth;
        }
    }
    
    private int GetBufferHeight()
    {
        try
        {
            return Console.BufferHeight;
        }
        catch
        {
            return CurrentHeight;
        }
    }
    
    // Background monitoring task
    private async Task MonitorSizeChanges(int intervalMs, CancellationToken cancellationToken)
    {
        while (_monitoringEnabled && !cancellationToken.IsCancellationRequested)
        {
            try
            {
                CheckForSizeChange();
                await Task.Delay(intervalMs, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch
            {
                // Continue monitoring even if there's an error
                await Task.Delay(intervalMs * 2, cancellationToken); // Back off on error
            }
        }
    }
    
    // Cleanup
    public void Dispose()
    {
        StopMonitoring();
    }
}

// Supporting classes
public class LayoutRecommendation
{
    public ResponsiveUI.ScreenSize ScreenSize { get; set; }
    public int SuggestedColumns { get; set; }
    public int SuggestedSpacing { get; set; }
    public bool UseCompactMode { get; set; }
    public bool UseVerticalLayout { get; set; }
    public int MaxContentWidth { get; set; }
    public MenuStyle SuggestedMenuStyle { get; set; }
}

public class TerminalInfo
{
    public int Width { get; set; }
    public int Height { get; set; }
    public ResponsiveUI.ScreenSize ScreenSize { get; set; }
    public bool IsLandscape { get; set; }
    public bool IsPortrait { get; set; }
    public bool CanSetSize { get; set; }
    public int BufferWidth { get; set; }
    public int BufferHeight { get; set; }
    
    public override string ToString()
    {
        return $"Terminal: {Width}x{Height} ({ScreenSize}), " +
               $"Orientation: {(IsLandscape ? "Landscape" : IsPortrait ? "Portrait" : "Square")}, " +
               $"Buffer: {BufferWidth}x{BufferHeight}, " +
               $"Resizable: {CanSetSize}";
    }
}

public enum MenuStyle
{
    Vertical,
    TwoColumn,
    ThreeColumn,
    Horizontal,
    Grid
}