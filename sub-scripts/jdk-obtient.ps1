$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Write-Host 'Java JDK 17'
Write-Host "JDK non installé ..."
Invoke-Download "Corretto Java Dev Kit" $CORRETTO_URL "jdk" $false


Start-Process powershell -argument "${env:scripty.scriptPath}\jdk-install.ps1"




If(${env:scripty.devMode} -eq $true) {
    $User = Read-Host -Prompt 'Tu peux fermer cette fenetre.'
}
