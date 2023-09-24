$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"
Invoke-Env-Reload
Write-Host "Flutter obtention"

if (-Not ( Test-Path $HOME\flutter )) {
    Invoke-CopyFromCache-Or-Download "Flutter" $FLUTTER_SDK "flutter.zip" $false


}
else {
    Write-Host '    ✔️  Flutter est déjà installé.'  -ForegroundColor Green
}

Start-Script "${env:scripty.scriptPath}\flutter-installe.ps1"