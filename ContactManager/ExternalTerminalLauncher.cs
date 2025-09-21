using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

public static class ExternalTerminalLauncher
{
    public static void LaunchInExternalTerminal()
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                LaunchWindowsTerminal();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                LaunchMacTerminal();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                LaunchLinuxTerminal();
            }
            else
            {
                Console.WriteLine("Unsupported platform. Running in current terminal...");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to launch external terminal: {ex.Message}");
            Console.WriteLine("Running in current terminal...");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
    
    private static void LaunchWindowsTerminal()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string executablePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        string dllPath = executablePath.Replace(".exe", ".dll");
        
        // Try different Windows terminal options in order of preference
        if (TryLaunchWindowsTerminalApp(currentDirectory, dllPath))
            return;
            
        if (TryLaunchPowerShell(currentDirectory, dllPath))
            return;
            
        if (TryLaunchCommandPrompt(currentDirectory, dllPath))
            return;
            
        // Fallback: try generic cmd
        LaunchGenericCmd(currentDirectory, dllPath);
    }
    
    private static bool TryLaunchWindowsTerminalApp(string workingDir, string dllPath)
    {
        try
        {
            // Try Windows Terminal (wt.exe)
            var startInfo = new ProcessStartInfo
            {
                FileName = "wt.exe",
                Arguments = $"--window 0 new-tab --title \"Contact Manager\" cmd /k \"cd /d \"{workingDir}\" && dotnet \"{dllPath}\" && pause\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };
            
            Process.Start(startInfo);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    private static bool TryLaunchPowerShell(string workingDir, string dllPath)
    {
        try
        {
            // Try PowerShell
            var startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoExit -Command \"Set-Location '{workingDir}'; $Host.UI.RawUI.WindowTitle = 'Contact Manager'; dotnet '{dllPath}'; Write-Host 'Press any key to exit...'; $null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown')\"",
                UseShellExecute = true,
                CreateNoWindow = false
            };
            
            Process.Start(startInfo);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    private static bool TryLaunchCommandPrompt(string workingDir, string dllPath)
    {
        try
        {
            // Try Command Prompt
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/k \"title Contact Manager && cd /d \"{workingDir}\" && dotnet \"{dllPath}\" && pause\"",
                UseShellExecute = true,
                CreateNoWindow = false
            };
            
            Process.Start(startInfo);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    private static void LaunchGenericCmd(string workingDir, string dllPath)
    {
        try
        {
            // Generic fallback
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c \"title Contact Manager && cd /d \"{workingDir}\" && dotnet \"{dllPath}\" && echo. && echo Application finished. Press any key to close... && pause > nul\"",
                UseShellExecute = true,
                CreateNoWindow = false
            };
            
            Process.Start(startInfo);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to launch any terminal: {ex.Message}");
            throw;
        }
    }
    
    private static void LaunchMacTerminal()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string executablePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        string dllPath = executablePath.Replace(".exe", ".dll");
        
        try
        {
            // Try Terminal.app
            var script = $@"tell application ""Terminal""
    do script ""cd '{currentDirectory}' && dotnet '{dllPath}' && echo 'Press any key to exit...' && read""
    set custom title of front window to ""Contact Manager""
end tell";
            
            var startInfo = new ProcessStartInfo
            {
                FileName = "osascript",
                Arguments = $"-e \"{script}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };
            
            Process.Start(startInfo);
        }
        catch
        {
            // Fallback to open command
            var startInfo = new ProcessStartInfo
            {
                FileName = "open",
                Arguments = $"-a Terminal \"{currentDirectory}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };
            
            Process.Start(startInfo);
        }
    }
    
    private static void LaunchLinuxTerminal()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string executablePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        string dllPath = executablePath.Replace(".exe", ".dll");
        
        // List of common Linux terminals to try
        string[] terminals = {
            "gnome-terminal",
            "konsole", 
            "xfce4-terminal",
            "mate-terminal",
            "lxterminal",
            "terminator",
            "xterm"
        };
        
        foreach (string terminal in terminals)
        {
            try
            {
                ProcessStartInfo startInfo;
                
                if (terminal == "gnome-terminal")
                {
                    startInfo = new ProcessStartInfo
                    {
                        FileName = terminal,
                        Arguments = $"--title=\"Contact Manager\" --working-directory=\"{currentDirectory}\" -- bash -c \"dotnet '{dllPath}'; echo 'Press any key to exit...'; read\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                }
                else if (terminal == "konsole")
                {
                    startInfo = new ProcessStartInfo
                    {
                        FileName = terminal,
                        Arguments = $"--title \"Contact Manager\" --workdir \"{currentDirectory}\" -e bash -c \"dotnet '{dllPath}'; echo 'Press any key to exit...'; read\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                }
                else if (terminal == "xfce4-terminal")
                {
                    startInfo = new ProcessStartInfo
                    {
                        FileName = terminal,
                        Arguments = $"--title=\"Contact Manager\" --working-directory=\"{currentDirectory}\" --command=\"bash -c 'dotnet \\\"{dllPath}\\\"; echo \\\"Press any key to exit...\\\"; read'\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                }
                else
                {
                    // Generic terminal
                    startInfo = new ProcessStartInfo
                    {
                        FileName = terminal,
                        Arguments = $"-title \"Contact Manager\" -e bash -c \"cd '{currentDirectory}' && dotnet '{dllPath}' && echo 'Press any key to exit...' && read\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                }
                
                Process.Start(startInfo);
                return; // Success, exit
            }
            catch
            {
                // Try next terminal
                continue;
            }
        }
        
        // If all terminals failed, throw exception
        throw new Exception("No suitable terminal found on Linux system");
    }
    
    public static bool IsRunningInExternalTerminal()
    {
        try
        {
            // Check if we're running in VS Code terminal or similar
            string? term = Environment.GetEnvironmentVariable("TERM_PROGRAM");
            string? vscode = Environment.GetEnvironmentVariable("VSCODE_INJECTION");
            
            return string.IsNullOrEmpty(term) && string.IsNullOrEmpty(vscode);
        }
        catch
        {
            return true; // Assume external if we can't determine
        }
    }
    
    public static void ShowLaunchOptions()
    {
        Console.Clear();
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("                    CONTACT MANAGER LAUNCHER                   ");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine();
        Console.WriteLine("Choose how you want to run the Contact Manager:");
        Console.WriteLine();
        Console.WriteLine("  [1] Launch in External Terminal (Recommended)");
        Console.WriteLine("      → Opens in a separate window like Visual Studio");
        Console.WriteLine("      → Better user experience");
        Console.WriteLine("      → Proper terminal size detection");
        Console.WriteLine();
        Console.WriteLine("  [2] Run in Current Terminal");
        Console.WriteLine("      → Runs in VS Code integrated terminal");
        Console.WriteLine("      → May have size detection limitations");
        Console.WriteLine();
        Console.WriteLine("  [0] Exit");
        Console.WriteLine();
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.Write("Your choice (1/2/0): ");
        
        string? choice = Console.ReadLine();
        
        switch (choice?.Trim())
        {
            case "1":
                Console.WriteLine();
                Console.WriteLine("Launching in external terminal...");
                Console.WriteLine("The application will open in a new window.");
                Console.WriteLine("You can close this VS Code terminal.");
                System.Threading.Thread.Sleep(1000);
                LaunchInExternalTerminal();
                Environment.Exit(0);
                break;
                
            case "2":
                Console.WriteLine();
                Console.WriteLine("Starting in current terminal...");
                System.Threading.Thread.Sleep(1000);
                // Continue with normal execution
                break;
                
            case "0":
                Console.WriteLine();
                Console.WriteLine("Goodbye!");
                Environment.Exit(0);
                break;
                
            default:
                Console.WriteLine();
                Console.WriteLine("Invalid choice. Starting in current terminal...");
                System.Threading.Thread.Sleep(1500);
                break;
        }
    }
}