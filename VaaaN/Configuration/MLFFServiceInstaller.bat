echo Installing Service... 


cd C:\Windows\Microsoft.NET\Framework\v4.0.30319\


InstallUtil.exe /i "C:\Program Files (x86)\VaaaN\MLFF\LocalDataService.exe" 
InstallUtil.exe /i "C:\Program Files (x86)\VaaaN\MLFF\SMSService.exe" 
InstallUtil.exe /i "C:\Program Files (x86)\VaaaN\MLFF\VMSService.exe" 
InstallUtil.exe /i "C:\Program Files (x86)\VaaaN\MLFF\DTSService.exe"
InstallUtil.exe /i "C:\Program Files (x86)\VaaaN\MLFF\MobileBroadCastService.exe" 
InstallUtil.exe /i "C:\Program Files (x86)\VaaaN\MLFF\ServiceMoniter.exe" 

echo Done.
pause
