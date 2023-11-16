$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Start-Transcript -Path ${env:scripty.localTempPath}\transcript-ij-obtient.txt
Invoke-Env-Reload
Write-Host "Intellij Idea r�cup�ration"

if (-Not ( Test-Path $HOME\idea )) {
    Invoke-CopyFromCache-Or-Download "IntelliJ Idea" $IDEA_URL "idea.zip" $false
    Start-Script "${env:scripty.scriptPath}\ij-installe.ps1"
}
else {
    Write-Host '    ✔️  IntelliJ est d�j� install�.'  -ForegroundColor Green
}
