echo Installing Service... 


cd C:\Windows\Microsoft.NET\Framework\v4.0.30319\


InstallUtil.exe /u "C:\Program Files (x86)\VaaaN\MLFF\LocalDataService.exe" 
InstallUtil.exe /u "C:\Program Files (x86)\VaaaN\MLFF\SMSService.exe" 
InstallUtil.exe /u "C:\Program Files (x86)\VaaaN\MLFF\VMSService.exe" 
InstallUtil.exe /u "C:\Program Files (x86)\VaaaN\MLFF\DTSService.exe" 

echo Done.
pause
