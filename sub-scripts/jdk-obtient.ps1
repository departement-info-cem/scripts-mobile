$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Write-Host 'Java JDK 17'
Write-Host "JDK non installé ..."
Invoke-Download "Corretto Java Dev Kit" $CORRETTO_URL "jdk" $false
powershell "${env:scripty.scriptPath}\jdk-installe.ps1"

