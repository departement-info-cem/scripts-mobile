$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Invoke-Env-Reload
Write-Host 'Script de status sur les installations :::' -ForegroundColor Blue
Do {
  Write-Host '---------------------------------------------'
  $date = Get-Date
  Write-Host "status des installations @ $date" -ForegroundColor Blue

  if (-Not ( Test-Path "$HOME\AppData\Local\Android\Sdk\fini.txt" )) {
    Write-Host '    Android SDK pas là.' -ForegroundColor Red
  }
  else {
    Write-Host '    ✔️  Android SDK déjà copié et déjà installé.' -ForegroundColor Green
  }
  if (-Not ( Test-Path "$HOME\jdk\fini.txt" )) {
    Write-Host '    Java JDK pas là.' -ForegroundColor Red
  }
  else {
    Write-Host '    ✔️  Java JDK  installé.' -ForegroundColor Green
  }
  if (-Not ( Test-Path "$HOME\flutter\fini.txt" )) {
    Write-Host '    Flutter SDK pas là.' -ForegroundColor Red
  }
  else {
    Write-Host '    ✔️  Flutter JDK  installé.' -ForegroundColor Green
  }
  if (-Not ( Test-Path "$HOME\android-studio" )) {
    Write-Host '    Android Studio pas là.'  -ForegroundColor Red
  }
  else {
    Write-Host '    ✔️  Android Studio est déjà installé.'  -ForegroundColor Green
  }
  if (-Not ( Test-Path "$HOME\idea\fini.txt" )) {
    Write-Host '    Intellij pas là.'  -ForegroundColor Red
  }
  else {
    Write-Host '    ✔️  Intellij est déjà installé.'  -ForegroundColor Green
  }
  Start-Sleep -s 10  # attend 10 seecondes avant de regarder
} While ($true)
