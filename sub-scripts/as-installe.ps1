$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"
Invoke-Env-Reload
Write-Host "Android Studio installation"



Add-Shortcut $HOME\android-studio\bin\studio64.exe "Android Studio"
Append-Env "Path" "$HOME\android-studio\bin"

Invoke-Install "Android Studio" "$HOME" "android-studio.zip"

Invoke-Install "plugin Dart" "$HOME\android-studio\plugins" "plugin-dart-android-studio.zip"
Invoke-Install "plugin Flutter" "$HOME\android-studio\plugins" "plugin-flutter-android-studio.zip"

If(${env:scripty.auCollege} -eq $false) {
    $studiopath = "$HOME\android-studio\bin\studio64.exe"
    Start-Process -FilePath $studiopath
}


If(${env:scripty.devMode} -eq $true) {
    $User = Read-Host -Prompt 'Tu peux fermer cette fenetre.'
}
