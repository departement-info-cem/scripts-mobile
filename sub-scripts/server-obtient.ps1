$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Invoke-Env-Reload
Write-Host "Serveur de Kick My B récupération"

Invoke-WebRequest "https://github.com/departement-info-cem/KickMyB-Server/archive/refs/heads/main.zip" -OutFile "${env:scripty.localTempPath}server.zip"
Expand-Archive "${env:scripty.localTempPath}server.zip" -DestinationPath ${env:scripty.localTempPath} -Force



If(${env:scripty.devMode} -eq $true) {
    $User = Read-Host -Prompt 'Tu peux fermer cette fenetre.'
}
