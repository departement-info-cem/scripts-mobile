$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"
Start-Transcript -Path ${env:scripty.localTempPath}\transcript-as-obtient.txt
Invoke-Env-Reload
Write-Host 'Android Studio téléchargement'

if (-Not ( Test-Path $HOME\android-studio )) {
    Write-Host 'Android Studio '
    Invoke-CopyFromCache-Or-Download "Android Studio" $STUDIO_URL "android-studio-plugins.7z" $false
    Start-Script "${env:scripty.scriptPath}\as-installe.ps1"
}
else {
    Write-Host '    Android Studio est déjà installé.'  -ForegroundColor Green
}


