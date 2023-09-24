$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

# installe un 7 zip local si on trouve pas dans Program Files
Local7Zip

Write-Host "Script d'installation des outils mobile CEM"
# TODO make sure we get 7zip.exe in portable format so we can start from fresh Windows install
Write-Host "--- Installation du JDK 17 de Corretto"
powershell "${env:scripty.scriptPath}\jdk-obtient.ps1"

Write-Host "--- Téléchargement du SDK"
powershell "${env:scripty.scriptPath}\sdk-obtient.ps1"

Write-Host "--- Téléchargement / installation de Android Studio"
powershell "${env:scripty.scriptPath}\as-obtient.ps1"

Write-Host "--- Téléchargement / installation de Intellij"
powershell "${env:scripty.scriptPath}\ij-obtient.ps1"

Write-Host "--- Téléchargement / installation de Flutter"
powershell "${env:scripty.scriptPath}\flutter-obtient.ps1"

Write-Host "--- Téléchargement / installation de Firebase CLI et FlutterFire"
powershell "${env:scripty.scriptPath}\sdk-obtient.ps1"

Write-Host "--- Téléchargement / installation des repos de 3N5 4N6 et 5N6"
powershell "${env:scripty.scriptPath}\cours-repo.ps1"