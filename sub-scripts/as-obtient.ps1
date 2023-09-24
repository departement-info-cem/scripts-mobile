$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"
Invoke-Env-Reload
Write-Host 'Android Studio téléchargement'



if (-Not ( Test-Path $HOME\android-studio )) {
    Write-Host 'Android Studio '
    Invoke-Download "Android Studio" $STUDIO_URL "android-studio" $false
    Write-Host 'Android Studio > plugin flutter'
    Invoke-Download "Plugin Flutter" $FLUTTER_PLUGIN_URL_STUDIO "plugin-flutter-android-studio" $false
    Write-Host 'Android Studio > plugin dart'
    Invoke-Download "Plugin Dart Android Studio" $DART_PLUGIN_URL_STUDIO "plugin-dart-android-studio" $false
    Start-Process powershell -ArgumentList "-noexit","${env:scripty.scriptPath}\as-installe.ps1"

}
else {
    Write-Host '    Android Studio est déjà installé.'  -ForegroundColor Green
}


