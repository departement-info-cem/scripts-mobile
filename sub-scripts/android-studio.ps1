
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Invoke-Env-Reload

Write-Host '🤖  Android Studio' -ForegroundColor Blue

if (-Not ( Test-Path $HOME\android-studio )) {
    Invoke-Download "Android Studio" $STUDIO_URL "android-studio" 
    Invoke-Install "Android Studio" "$HOME" "android-studio"
}
else {
    Write-Host '    ✔️  Android Studio est déjà installé.'  -ForegroundColor Green
}

Add-Shortcut $HOME\android-studio\bin\studio64.exe "Android Studio"
Add-Env "Path" "$HOME\android-studio\bin"

Add-Env "JAVA_HOME" "$HOME\android-studio\jre"
Append-Env "Path" "$HOME\android-studio\jre\bin"

Add-Env "ANDROID_SDK_ROOT" "$HOME\androidsdk"
Add-Env "ANDROID_HOME" "$env:ANDROID_SDK_ROOT"

if (-Not(Test-Path $HOME\android-studio\plugins\flutter-intellij)) {
Invoke-Download "Plugin Flutter" $FLUTTER_PLUGIN_URL_STUDIO "plugin-flutter-android-studio"
    Invoke-Install "plugin Flutter" "$HOME\android-studio\plugins" "plugin-flutter-android-studio"
}
else {
    Write-Host '    ✔️  Le plugin Flutter est déjà installé.'  -ForegroundColor Green
}

if (-Not(Test-Path $HOME\android-studio\plugins\dart)) {
    Invoke-Download "Plugin Dart Android Studio" $DART_PLUGIN_URL_STUDIO "plugin-dart-android-studio"
    Invoke-Install "plugin Dart" "$HOME\android-studio\plugins" "plugin-dart-android-studio"
}
else {
    Write-Host '    ✔️  Le plugin Dart est déjà installé.'  -ForegroundColor Green
}

Start-Process powershell -argument "${env:scripty.scriptPath}\android-sdk.ps1"

$User = Read-Host -Prompt 'Installation de Android Studio complétée, tu peux fermer cette fenetre'
