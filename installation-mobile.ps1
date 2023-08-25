# TODO voir si on peut trouver une fonction plus rapide pour télécharger mais ça arrive pas souvent
# TODO comment on gère le path et un potentiel conflit sur le path pour java et flutter
# Piste pour le conflit flutter au college, scripter la suppression du Flutter déjà installé
# Piste pour le PATH ? comment embarquer avec notre Java à nous

${env:scripty.dev} = $false
${env:scripty.debug} = $false

# config du chemin local pour le stockage des GROS zips avant dézippage
${env:scripty.localTempPath} = "$HOME\temp\"

If (${env:scripty.dev} -eq $true) {
    $branche = "dev"
    ${env:scripty.cachePath} = "\\ed5depinfo\Logiciels\Android\scripts\cacheDev"
    ${env:scripty.scriptPath} = ".\sub-scripts"
}
Else {
    $branche = "main"
    ${env:scripty.cachePath} = "\\ed5depinfo\Logiciels\Android\scripts\cache"
    ${env:scripty.scriptPath} = "${env:scripty.localTempPath}scripts-mobile-$branche\sub-scripts"
}

# config du chemin de téléchargement de base pour les sous-scripts
${env:scripty.rawRepoPath} = "https://github.com/departement-info-cem/scripts-mobile/archive/refs/heads/$branche.zip"

Write-Host "Bienvenue sur l'installeur pour les cours de Mobile"
Write-Host " Je vais télécharger des scripts d'installation depuis ${env:scripty.rawRepoPath}"
Write-Host " Je vais également télécharger des fichiers ZIP depuis ${env:scripty.cachePath} vers ${env:scripty.localTempPath}"

# création du dossier local et téléchargement des sous-scripts
[void](New-Item -type directory -Path ${env:scripty.localTempPath} -Force)
[void](New-Item -type directory -Path ${env:scripty.cachePath} -Force)

# download le repo sur la machine cible
Invoke-WebRequest ${env:scripty.rawRepoPath} -OutFile "${env:scripty.localTempPath}scripts.zip"
Expand-Archive "${env:scripty.localTempPath}scripts.zip" -DestinationPath ${env:scripty.localTempPath} -Force

if ($args[0] -eq "H4X0R_M0D") {
    for (($i = 0); $i -lt 7; $i++) {
        Start-Process powershell -argument "${env:scripty.scriptPath}\cmatrix.ps1"
    }
}

Start-Process powershell -argument "${env:scripty.scriptPath}\android-studio.ps1"
Start-Process powershell -argument "${env:scripty.scriptPath}\idea.ps1"
Start-Process powershell -argument "${env:scripty.scriptPath}\install-flutter.ps1"

# Start-Process powershell -argument ".\firebase-flutterfire.ps1"

$User = Read-Host -Prompt 'Tu peux fermer cette fenetre.'
