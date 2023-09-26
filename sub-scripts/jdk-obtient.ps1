$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Write-Host 'Java JDK 17'
Write-Host "JDK non installée ..."
Invoke-CopyFromCache-Or-Download "Corretto Java Dev Kit" $CORRETTO_URL "jdk.zip" $false
powershell "${env:scripty.scriptPath}\jdk-installe.ps1"

