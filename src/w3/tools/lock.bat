regedit /s %~dp0EnableLockWorkstation.reg
rundll32.exe user32.dll,LockWorkStation
regedit /s %~dp0DisableLockWorkstation.reg