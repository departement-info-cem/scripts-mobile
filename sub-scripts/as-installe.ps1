$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"
Invoke-Env-Reload
Write-Host "Android Studio installation"



Add-Shortcut $HOME\android-studio\bin\studio64.exe "Android Studio"
Append-Env "Path" "$HOME\android-studio\bin"

Invoke-Install "android-studio" "$HOME" "android-studio.zip"
Invoke-Install "android-studio-dart" "$HOME\android-studio\plugins" "plugin-dart-android-studio.zip"
Invoke-Install "android-studio-flutter" "$HOME\android-studio\plugins" "plugin-flutter-android-studio.zip"
Invoke-Install "android-studio-flutter-intl" "$HOME\android-studio\plugins" "plugin-flutter-intl-android-studio.zip"

If(${env:scripty.auCollege} -eq $false) {
    $studiopath = "$HOME\android-studio\bin\studio64.exe"
    Start-Process -FilePath $studiopath
}
