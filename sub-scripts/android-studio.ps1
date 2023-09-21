$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Invoke-Env-Reload

Write-Host 'Android Studio' -ForegroundColor Blue

if (-Not ( Test-Path $HOME\android-studio )) {
    Invoke-Download "Android Studio" $STUDIO_URL "android-studio" $false
    Invoke-Download "Plugin Flutter" $FLUTTER_PLUGIN_URL_STUDIO "plugin-flutter-android-studio" $false
    Invoke-Download "Plugin Dart Android Studio" $DART_PLUGIN_URL_STUDIO "plugin-dart-android-studio" $false


}
else {
    Write-Host '    Android Studio est déjà installé.'  -ForegroundColor Green
}
