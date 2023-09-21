﻿# TODO comment on gère le path et un potentiel conflit sur le path pour java et flutter
# Piste pour le conflit flutter au college, scripter la suppression du Flutter déjà installé
# Piste pour le PATH ? comment embarquer avec notre Java à nous



${env:scripty.auCollege} = Test-Path "\\ed5depinfo\Logiciels\Android"
${env:scripty.devMode} = Test-Path ".\sub-scripts"
${env:scripty.verbose} = ${env:scripty.devMode}
${env:scripty.localTempPath} = "$HOME\temp\"

Write-Host "Installeur pour les cours de Mobile"

Write-Host " .. obtient les ZIP @ ${env:scripty.cachePath} "
Write-Host " .. en mode développeur? ${env:scripty.devMode} "
Write-Host " .. au college? ${env:scripty.auCollege} "

If (${env:scripty.devMode} -eq $true) {
    $branche = "dev"
    $currentDirectory = Get-Location
    ${env:scripty.scriptPath} = "$currentDirectory\sub-scripts"
    If (${env:scripty.auCollege} -eq $true) {
        ${env:scripty.cachePath} = "\\ed5depinfo\Logiciels\Android\scripts\cacheDev"
    }
    Else  {
        ${env:scripty.cachePath} = ${env:scripty.localTempPath}
    }
}
Else {
    $branche = "main"
    ${env:scripty.scriptPath} = "${env:scripty.localTempPath}scripts-mobile-$branche\sub-scripts"
    If (${env:scripty.auCollege} -eq $true) {
        ${env:scripty.cachePath} = "\\ed5depinfo\Logiciels\Android\scripts\cache"
    }
    Else  {
        ${env:scripty.cachePath} = ${env:scripty.localTempPath}
    }
}

# config du chemin de téléchargement de base pour les sous-scripts
If (${env:scripty.devMode} -eq $false) {
    ${env:scripty.rawRepoPath} = "https://github.com/departement-info-cem/scripts-mobile/archive/refs/heads/$branche.zip"
    Write-Host " obtient les scripts @ ${env:scripty.rawRepoPath}"

    # download le repo sur la machine cible
    Invoke-WebRequest ${env:scripty.rawRepoPath} -OutFile "${env:scripty.localTempPath}scripts.zip"
    Expand-Archive "${env:scripty.localTempPath}scripts.zip" -DestinationPath ${env:scripty.localTempPath} -Force

}

# création du dossier local et téléchargement des sous-scripts
[void](New-Item -type directory -Path ${env:scripty.localTempPath} -Force)
[void](New-Item -type directory -Path ${env:scripty.cachePath} -Force)

Write-Host " scripts localement @ ${env:scripty.scriptPath}"

Start-Process powershell -argument "${env:scripty.scriptPath}\sdk-obtient.ps1"
#Start-Process powershell -argument "${env:scripty.scriptPath}\android-sdk.ps1"
#Start-Process powershell -argument "${env:scripty.scriptPath}\install-jdk.ps1"
#Start-Process powershell -argument "${env:scripty.scriptPath}\idea.ps1"
#Start-Process powershell -argument "${env:scripty.scriptPath}\install-flutter.ps1"

#Start-Process pwsh -argument "${env:scripty.scriptPath}\status.ps1"
# Start-Process powershell -ArgumentList "-command [console]::windowtop=10; [console]::windowleft=10; ","${env:scripty.scriptPath}\status.ps1"

#$User = Read-Host -Prompt 'Tu peux fermer cette fenetre.'

If(${env:scripty.devMode} -eq $true) {
    $User = Read-Host -Prompt 'Tu peux fermer cette fenetre.'
}
