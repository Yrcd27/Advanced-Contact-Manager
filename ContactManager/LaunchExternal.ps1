# Contact Manager External Launcher
# This script launches the Contact Manager in an external terminal window

param(
    [switch]$Force,
    [string]$Terminal = "auto"
)

function Write-Banner {
    Clear-Host
    Write-Host "═══════════════════════════════════════════════════════════════" -ForegroundColor Cyan
    Write-Host "                    CONTACT MANAGER LAUNCHER                   " -ForegroundColor Yellow
    Write-Host "═══════════════════════════════════════════════════════════════" -ForegroundColor Cyan
    Write-Host ""
}

function Test-TerminalAvailable {
    param([string]$TerminalName)
    
    try {
        $null = Get-Command $TerminalName -ErrorAction Stop
        return $true
    }
    catch {
        return $false
    }
}

function Start-InWindowsTerminal {
    try {
        $currentPath = Get-Location
        $arguments = "--window 0 new-tab --title `"Contact Manager`" PowerShell -NoExit -Command `"Set-Location '$currentPath'; `$Host.UI.RawUI.WindowTitle = 'Contact Manager'; dotnet run -c Release; Write-Host 'Press any key to exit...'; `$null = `$Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown')`""
        
        Start-Process "wt.exe" -ArgumentList $arguments
        return $true
    }
    catch {
        return $false
    }
}

function Start-InPowerShell {
    try {
        $currentPath = Get-Location
        $arguments = "-NoExit -Command `"Set-Location '$currentPath'; `$Host.UI.RawUI.WindowTitle = 'Contact Manager'; dotnet run -c Release; Write-Host 'Press any key to exit...'; `$null = `$Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown')`""
        
        Start-Process "powershell.exe" -ArgumentList $arguments
        return $true
    }
    catch {
        return $false
    }
}

function Start-InCommandPrompt {
    try {
        $currentPath = Get-Location
        $arguments = "/k `"title Contact Manager && cd /d `"$currentPath`" && dotnet run -c Release && pause`""
        
        Start-Process "cmd.exe" -ArgumentList $arguments
        return $true
    }
    catch {
        return $false
    }
}

# Main execution
Write-Banner

Write-Host "🔨 Building Contact Manager..." -ForegroundColor Yellow
try {
    $buildResult = dotnet build -c Release --nologo -v q 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Build failed!" -ForegroundColor Red
        Write-Host $buildResult -ForegroundColor Red
        Write-Host ""
        Write-Host "Press any key to exit..." -ForegroundColor Gray
        $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
        exit 1
    }
}
catch {
    Write-Host "❌ Build failed: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "Press any key to exit..." -ForegroundColor Gray
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    exit 1
}

Write-Host "✅ Build successful!" -ForegroundColor Green
Write-Host ""

Write-Host "🚀 Launching Contact Manager in external terminal..." -ForegroundColor Yellow

$launched = $false

# Try different terminals based on preference
switch ($Terminal.ToLower()) {
    "wt" { 
        if (Test-TerminalAvailable "wt.exe") {
            $launched = Start-InWindowsTerminal
        }
    }
    "powershell" { 
        $launched = Start-InPowerShell
    }
    "cmd" { 
        $launched = Start-InCommandPrompt
    }
    default {
        # Auto-detect best terminal
        if (Test-TerminalAvailable "wt.exe") {
            Write-Host "   → Using Windows Terminal..." -ForegroundColor Gray
            $launched = Start-InWindowsTerminal
        }
        elseif (Test-TerminalAvailable "powershell.exe") {
            Write-Host "   → Using PowerShell..." -ForegroundColor Gray
            $launched = Start-InPowerShell
        }
        else {
            Write-Host "   → Using Command Prompt..." -ForegroundColor Gray
            $launched = Start-InCommandPrompt
        }
    }
}

if ($launched) {
    Write-Host "✅ Contact Manager launched successfully!" -ForegroundColor Green
    Write-Host ""
    Write-Host "The application is now running in a separate terminal window." -ForegroundColor Cyan
    Write-Host "You can close this PowerShell window." -ForegroundColor Gray
    Write-Host ""
    Write-Host "Closing in 3 seconds..." -ForegroundColor Gray
    Start-Sleep -Seconds 3
}
else {
    Write-Host "❌ Failed to launch external terminal!" -ForegroundColor Red
    Write-Host ""
    Write-Host "Falling back to current terminal..." -ForegroundColor Yellow
    Write-Host "Press any key to start..." -ForegroundColor Gray
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    
    # Run in current terminal as fallback
    dotnet run -c Release
}