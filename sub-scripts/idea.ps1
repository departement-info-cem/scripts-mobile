$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Invoke-Env-Reload

function Install-Idea(){
    Write-Host 'IntelliJ' -ForegroundColor Blue

    if (-Not ( Test-Path $HOME\idea )) {
        #Invoke-Download "IntelliJ" "https://data.services.jetbrains.com/products/download?platform=windowsZip&code=IIU" "idea"
        Invoke-Download "IntelliJ Idea" "$IDEA_URL" "idea" $false
        Invoke-Install "IntelliJ Idea" "$HOME\idea"  "idea.zip"
       
    }
    else {
        Write-Host '    ✔️  IntelliJ est déjà installé.'  -ForegroundColor Green
    }
    #TODO test if shortcut already exists in C:\Users\joris\AppData\Roaming\Microsoft\Windows\Start Menu\Programs
    Add-Shortcut $HOME\idea\bin\idea64.exe "IntelliJ IDEA Ultimate"
    Add-Env "Path" "$HOME\idea\bin"

    if (-Not(Test-Path $HOME\idea\plugins\flutter-intellij)) {
        Invoke-Download "plugin Flutter" $FLUTTER_PLUGIN_URL_IDEA "plugin-flutter-idea" $false
        Invoke-Install "plugin Flutter" "$HOME\idea\plugins" "plugin-flutter-idea.zip"
    }
    else {
        Write-Host '    ✔️  Le plugin Flutter est déjà installé.'  -ForegroundColor Green
    }

    if (-Not(Test-Path $HOME\idea\plugins\dart)) {
        Invoke-Download "plugin Dart" $DART_PLUGIN_URL_IDEA "plugin-dart-idea" $false
        Invoke-Install "plugin Dart" "$HOME\idea\plugins" "plugin-dart-idea.zip"
    }
    else {
        Write-Host '    ✔️  Le plugin Dart est déjà installé.'  -ForegroundColor Green
    }
    #$User = Read-Host -Prompt 'Installation de Intellij et de ses plugins terminées, vous pouvez fermer CETTE fenetre'
}

Install-Idea
Start-Process -FilePath C:\Users\joris.deguet\idea\bin\idea64.exe
