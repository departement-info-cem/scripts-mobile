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
    Add-Shortcut $HOME\idea\bin\idea64.exe "IntelliJ IDEA Community"
    Add-Env "Path" "$HOME\idea\bin"
}

Install-Idea
# Start-Process -FilePath C:\Users\joris.deguet\idea\bin\idea64.exe
