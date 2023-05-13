﻿# config du chemin pour la cache des GROS zips
${env:scripty.cachePath} = 'D:\cache'
#${env:scripty.cachePath} = "\\ed5depinfo\Logiciels\Android\scripts\cache"

# config du chemin de téléchargement de base pour les sous-scripts
${env:scripty.rawRepoPath} = "https://raw.githubusercontent.com/departement-info-cem/scripts-mobile/main/"

# config du chemin local pour le stockage des GROS zips avant dézippage
${env:scripty.localTempPath} = "$HOME\temp\"

#$branche = "main"
$branche = "UnScriptPourLesGouvernerTous"

Write-Host "Bienvenue sur l'installeur pour les cours de Mobile"
Write-Host " Je vais télécharger des scripts d'installation depuis ${env:scripty.rawRepoPath}"
Write-Host " Je vais également télécharger des fichiers ZIP depuis ${env:scripty.cachePath} vers ${env:scripty.localTempPath}"

# création du dossier local et téléchargement des sous-scripts
[void](New-Item -type directory -Path ${env:scripty.localTempPath} -Force)
[void](New-Item -type directory -Path ${env:scripty.cachePath} -Force)

# download le repo sur la machine cible
Invoke-WebRequest "https://github.com/departement-info-cem/scripts-mobile/archive/refs/heads/$branche.zip" -OutFile "${env:scripty.localTempPath}scripts.zip" 
Expand-Archive "${env:scripty.localTempPath}scripts.zip" -DestinationPath ${env:scripty.localTempPath} -Force
Set-Location ${env:scripty.localTempPath}scripts-mobile-$branche

if ($args[0] -eq "H4X0R_M0D") {
    for (($i = 0); $i -lt 7; $i++) {
        Start-Process powershell -argument ".\sub-scripts\cmatrix.ps1"
    }
}

$scriptPath = ".\sub-scripts"
#$scriptPath = "${env:scripty.localTempPath}scripts-mobile-$branche"


#Start-Process powershell -argument "$scriptPath\android-studio.ps1"
#Start-Process powershell -argument "$scriptPath\idea.ps1"
Start-Process powershell -argument "$scriptPath\install-flutter.ps1"


# Start-Process powershell -argument ".\firebase-flutterfire.ps1"

$User = Read-Host -Prompt 'Bloquant script install'