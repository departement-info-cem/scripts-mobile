$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Start-Transcript -Path ${env:scripty.localTempPath}\transcript-flutter-obtient.txt
Invoke-Env-Reload
Write-Host "Flutter obtention"

if (-Not ( Test-Path $HOME\flutter )) {
    Invoke-CopyFromCache-Or-Download "Flutter" $FLUTTER_SDK "flutter.7z" $false
}
else {
    Write-Host '    ✔️  Flutter est déjà installé.'  -ForegroundColor Green
}

Start-Script "${env:scripty.scriptPath}\flutter-installe.ps1"