@echo off
echo Testing External Terminal Launcher...
echo.

REM Test if we can detect VS Code environment
set "VSCODE_TEST=1"
echo Setting VSCODE_INJECTION for testing...
set "VSCODE_INJECTION=1"

echo.
echo Running Contact Manager with launcher detection...
echo.

dotnet run

echo.
echo Test completed.
pause