$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Start-Transcript -Path ${env:scripty.localTempPath}\transcript-main.txt
# installe un 7 zip local si on trouve pas dans Program Files cassé voir comment installer le .exe portable
#Local7Zip

# Fix some PATH before all 
Remove-Env "Path" "C:\Flutter\bin"
Append-Env "Path" "$HOME\flutter\bin"
Append-Env "Path" $HOME\AppData\Local\Pub\Cache\bin

Write-Host "Script d'installation des outils mobile CEM"
# TODO make sure we get 7zip.exe in portable format so we can start from fresh Windows install
Write-Host "--- Installation du JDK 17 de Corretto"
powershell "${env:scripty.scriptPath}\jdk-obtient.ps1"

Write-Host "--- Téléchargement du cache gradle"
powershell "${env:scripty.scriptPath}\dotgradle-obtient.ps1"

Write-Host "--- Téléchargement du SDK"
powershell "${env:scripty.scriptPath}\sdk-obtient.ps1"

Write-Host "--- Téléchargement / installation de Android Studio"
powershell "${env:scripty.scriptPath}\as-obtient.ps1"

Write-Host "--- Téléchargement / installation de Intellij"
powershell "${env:scripty.scriptPath}\ij-obtient.ps1"

Write-Host "--- Téléchargement / installation de Flutter"
powershell "${env:scripty.scriptPath}\flutter-obtient.ps1"

Write-Host "--- Téléchargement / installation des repos de 3N5 4N6 et 5N6"
powershell "${env:scripty.scriptPath}\cours-repo.ps1"

Write-Host "--- Création des lien sur le bureau"
powershell "${env:scripty.scriptPath}\lien-bureau.ps1"

Invoke-Env-Reload
Write-Host 'Status des installations :::' -ForegroundColor Blue

$data = @('\jdk\jdk17.0.9_8\bin',
    '\AppData\Local\Android\Sdk\licenses',
    '\android-studio\bin',
    '\idea\bin',
    '\flutter\bin'

    )

$counter = 0
Do {
  Write-Host '---------------------------------------------'
  $date = Get-Date
  Write-Host "status des installations @ $date" -ForegroundColor Blue
  $allgood = $true
  $counter = 0
  foreach ( $node in $data )
  {
        $filePath = "$HOME$node"
         if (-Not ( Test-Path $filePath )) {
           Write-Host $filePath'     nope.' -ForegroundColor Red
           $allgood = $false
         }
         else {
            $counter++
           Write-Host $filePath'     ok.' -ForegroundColor Green
         }
  }
  Start-Sleep -s 10  # attend 10 secondes avant de regarder
} While ($data.Length -ne $counter)