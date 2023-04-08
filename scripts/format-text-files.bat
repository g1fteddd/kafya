@echo off
call "%~dp0_internal/run-bash.bat" "%~dp0format-text-files" %* || if /i %0 == "%~0" pause
exit /b %ERRORLEVEL%
