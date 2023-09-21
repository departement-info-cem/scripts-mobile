$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"
Invoke-Env-Reload
Write-Host "Flutter obtention"

if (-Not ( Test-Path $HOME\flutter )) {
    Invoke-Download "Flutter" $FLUTTER_SDK "flutter" $false


}
else {
    Write-Host '    ✔️  Flutter est déjà installé.'  -ForegroundColor Green
}

Start-Process powershell -argument "${env:scripty.scriptPath}\flutter-installe.ps1"