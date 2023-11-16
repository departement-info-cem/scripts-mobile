$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Start-Transcript -Path ${env:scripty.localTempPath}\transcript-jdk-obtient.txt
Write-Host 'Java JDK 17'
Write-Host "JDK non install�e ..."
Invoke-CopyFromCache-Or-Download "Corretto Java Dev Kit" $CORRETTO_URL "jdk.7z" $false
powershell "${env:scripty.scriptPath}\jdk-installe.ps1"

