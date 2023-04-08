@echo off
call "%~dp0_internal/run-bash.bat" "%~dp0check-dotnet-format" %* || if /i %0 == "%~0" pause
exit /b %ERRORLEVEL%
