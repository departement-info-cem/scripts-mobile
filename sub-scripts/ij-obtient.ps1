$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Invoke-Env-Reload
Write-Host "Intellij Idea récupération"

if (-Not ( Test-Path $HOME\idea )) {
    #Invoke-Download "IntelliJ" "https://data.services.jetbrains.com/products/download?platform=windowsZip&code=IIU" "idea"
    Invoke-Download "IntelliJ Idea" "$IDEA_URL" "idea" $false
    Invoke-Download "plugin Flutter" $FLUTTER_PLUGIN_URL_IDEA "plugin-flutter-idea" $false
    Invoke-Download "plugin Dart" $DART_PLUGIN_URL_IDEA "plugin-dart-idea" $false
    Start-Process powershell -argument "${env:scripty.scriptPath}\ij-installe.ps1"
}
else {
    Write-Host '    ✔️  IntelliJ est déjà installé.'  -ForegroundColor Green
}
