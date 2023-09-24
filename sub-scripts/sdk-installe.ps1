$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"
Invoke-Env-Reload


Write-Host "Installation du SDK Android Studio"
Invoke-Unzip "Android SDK" "Sdk.7z" "$HOME\AppData\Local\Android"



Start-Process powershell -argument "${env:scripty.scriptPath}\android-emulator.ps1"

# partir Android studio
$finipath = "$HOME\android-studio"
$studiopath = "$HOME\android-studio\bin\studio64.exe"
Wait-Until-File-Exists($finipath)
Write-Host 'Android Studio va partir dans 30 secondes'
Start-Sleep -s 30
Start-Process -FilePath $studiopath