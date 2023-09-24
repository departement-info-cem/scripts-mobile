$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Invoke-Env-Reload
Write-Host "Intellij Idea installation"

if (-Not ( Test-Path $HOME\idea )) {

    Invoke-Install "IntelliJ Idea" "$HOME\idea"  "idea.zip"
    #Invoke-Install "plugin Flutter" "$HOME\idea\plugins" "plugin-flutter-idea.zip"
    #Invoke-Install "plugin Dart" "$HOME\idea\plugins" "plugin-dart-idea.zip"
    Add-Shortcut $HOME\idea\bin\idea64.exe "IntelliJ IDEA Community"
    Add-Env "Path" "$HOME\idea\bin"
    Start-Process -FilePath "$HOME\idea\bin\idea64.exe"
}
else {
    Write-Host '    ✔️  IntelliJ est déjà installé.'  -ForegroundColor Green
}
