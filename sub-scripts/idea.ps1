$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8


. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"


$INITIAL_DIR = $HOME

$DOWNLOADS = ${env:scripty.cachePath}

$FLUTTER_PLUGIN_URL_IDEA = 'https://plugins.jetbrains.com/plugin/download?rel=true&updateId=231428'
$DART_PLUGIN_URL_IDEA = 'https://plugins.jetbrains.com/plugin/download?rel=true&updateId=233333'


[void](New-Item -type directory -Path "$DOWNLOADS" -Force)

Invoke-Env-Reload

Write-Host '🕰️  Mise à jour des variables d''environnement' -ForegroundColor Blue

function Install-Idea(){
    Write-Host '🧠  IntelliJ' -ForegroundColor Blue

    if (-Not ( Test-Path $HOME\idea )) {
        #Invoke-Download "IntelliJ" "https://data.services.jetbrains.com/products/download?platform=windowsZip&code=IIU" "idea"
        Invoke-Download "IntelliJ" "https://download.jetbrains.com/idea/ideaIC-2022.2.3.win.zip" "idea"
        Invoke-Install "IntelliJ" "$HOME\idea" "bin" "idea"
       
    }
    else {
        Write-Host '    ✔️  IntelliJ est déjà installé.'  -ForegroundColor Green
    }
    #TODO test if shortcut already exists in C:\Users\joris\AppData\Roaming\Microsoft\Windows\Start Menu\Programs
    Add-Shortcut $HOME\idea\bin\idea64.exe "IntelliJ IDEA Ultimate"
    Add-Env "Path" "$HOME\idea\bin"

    if (-Not(Test-Path $HOME\idea\plugins\flutter-intellij)) {
        Invoke-Download "plugin Flutter" $FLUTTER_PLUGIN_URL_IDEA "plugin-flutter-idea"
        Invoke-Install "plugin Flutter" "$HOME\idea\plugins" "flutter-intellij" "plugin-flutter-idea"
    }
    else {
        Write-Host '    ✔️  Le plugin Flutter est déjà installé.'  -ForegroundColor Green
    }

    if (-Not(Test-Path $HOME\idea\plugins\dart)) {
        Invoke-Download "plugin Dart" $DART_PLUGIN_URL_IDEA "plugin-dart-idea"
        Invoke-Install "plugin Dart" "$HOME\idea\plugins" "dart" "plugin-dart-idea"
    }
    else {
        Write-Host '    ✔️  Le plugin Dart est déjà installé.'  -ForegroundColor Green
    }
    $User = Read-Host -Prompt 'Installation de Intellij et de ses plugins terminées, vous pouvez fermer CETTE fenetre'
}

Install-Idea