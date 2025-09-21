@echo off
title Contact Manager Launcher
cls

echo ═══════════════════════════════════════════════════════════════
echo                    CONTACT MANAGER LAUNCHER                   
echo ═══════════════════════════════════════════════════════════════
echo.
echo This will launch Contact Manager in a new terminal window.
echo.
echo Building application...

REM Build the application
dotnet build -c Release --nologo -v q

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo ❌ Build failed! Please check for compilation errors.
    echo.
    pause
    exit /b 1
)

echo ✅ Build successful!
echo.
echo Launching Contact Manager in external terminal...
echo You can close this window after the application starts.
echo.

REM Launch in external terminal
start "Contact Manager" cmd /k "title Contact Manager && dotnet run -c Release && echo. && echo Application finished. Press any key to close this window... && pause > nul"

REM Wait a moment then close this launcher
timeout /t 3 /nobreak > nul
exit