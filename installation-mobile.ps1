# config du chemin pour la cache des GROS zips
${env:scripty.cachePath} = 'D:\cache'
#${env:scripty.cachePath} = "\\ed5depinfo\Logiciels\Android\scripts\cache"

# config du chemin de téléchargement de base pour les sous-scripts
${env:scripty.rawRepoPath} = "https://raw.githubusercontent.com/departement-info-cem/scripts-mobile/main/"

# config du chemin local pour le stockage des GROS zips avant dézippage
${env:scripty.localTempPath} = "$HOME\temp\"

Write-Host "Bienvenue sur l'installeur pour les cours de Mobile"
Write-Host " Je vais télécharger des scripts d'installation depuis ${env:scripty.rawRepoPath}"
Write-Host " Je vais également télécharger des fichiers ZIP depuis ${env:scripty.cachePath} vers ${env:scripty.localTempPath}"


# création du dossier local et téléchargement des sous-scripts
[void](New-Item -type directory -Path ${env:scripty.localTempPath} -Force)
[void](New-Item -type directory -Path ${env:scripty.cachePath} -Force)

# download les sous-scripts  TODO remplacer par un git clone du repo 
#Invoke-WebRequest "${env:scripty.rawRepoPath}sub-scripts/fonctions.ps1"         -OutFile "${env:scripty.localTempPath}fonctions.ps1"                 TODO remove 
Invoke-WebRequest "${env:scripty.rawRepoPath}sub-scripts/android-studio.ps1"         -OutFile "${env:scripty.localTempPath}android-studio.ps1" 
Invoke-WebRequest "${env:scripty.rawRepoPath}sub-scripts/android-sdk.ps1"            -OutFile "${env:scripty.localTempPath}android-sdk.ps1" 
Invoke-WebRequest "${env:scripty.rawRepoPath}sub-scripts/idea.ps1"                   -OutFile "${env:scripty.localTempPath}idea.ps1" 
Invoke-WebRequest "${env:scripty.rawRepoPath}sub-scripts/flutter.ps1"                -OutFile "${env:scripty.localTempPath}flutter.ps1" 
#. ${env:scripty.localTempPath}android.download.ps1


if ($args[0] -eq "H4X0R_M0D") {
    for (($i = 0); $i -lt 7; $i++) {
        Start-Process powershell -argument ".\sub-scripts\cmatrix.ps1"
    }
}

Write-Host ${env:scripty.cachePath}
#Start-Process powershell -argument ".\sub-scripts\test-variable.ps1"


#Start-Process powershell -argument "${env:scripty.localTempPath}android-studio.ps1"
Start-Process powershell -argument ".\sub-scripts\android-studio.ps1"

#Start-Process powershell -argument "${env:scripty.localTempPath}idea.ps1"
#Start-Process powershell -argument ".\sub-scripts\idea.ps1"



Start-Process powershell -argument ".\sub-scripts\install-flutter.ps1"



# Start-Process powershell -argument ".\firebase-flutterfire.ps1"

$User = Read-Host -Prompt 'Bloquant script install'