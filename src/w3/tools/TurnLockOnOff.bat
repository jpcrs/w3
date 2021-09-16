@ECHO OFF 
reg query HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System /v DisableLockWorkstation
if %ERRORLEVEL% EQU 0 regedit /s %~dp0EnableLockWorkstation.reg
if %ERRORLEVEL% EQU 1 regedit /s %~dp0DisableLockWorkstation.reg