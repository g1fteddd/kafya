@echo off
call "%~dp0_internal/run-bash.bat" "%~dp0fix-scripts" %* || if /i %0 == "%~0" pause
exit /b %ERRORLEVEL%
