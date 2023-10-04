$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"
Start-Transcript -Path ${env:scripty.localTempPath}\transcript-as-obtient.txt
Invoke-Env-Reload
Write-Host 'Android Studio téléchargement'



if (-Not ( Test-Path $HOME\android-studio )) {
    Write-Host 'Android Studio '
    Invoke-CopyFromCache-Or-Download "Android Studio" $STUDIO_URL "android-studio.zip" $false
    Write-Host 'Android Studio > plugin flutter'
    Invoke-CopyFromCache-Or-Download "Plugin Flutter" $FLUTTER_PLUGIN_URL_STUDIO "plugin-flutter-android-studio.zip" $false
    Write-Host 'Android Studio > plugin dart'
    Invoke-CopyFromCache-Or-Download "Plugin Flutter Intl Android Studio" $FLUTTER_INTL_PLUGIN_URL_STUDIO "plugin-flutter-intl-android-studio.zip" $false
    
    Write-Host 'Android Studio > plugin dart'
    Invoke-CopyFromCache-Or-Download "Plugin Dart Android Studio" $DART_PLUGIN_URL_STUDIO "plugin-dart-android-studio.zip" $false
    

    # TODO ajouter le flutter intl plugin


    Start-Script "${env:scripty.scriptPath}\as-installe.ps1"

}
else {
    Write-Host '    Android Studio est déjà installé.'  -ForegroundColor Green
}


