$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Invoke-Env-Reload
Write-Host 'Script de status sur les installations :::' -ForegroundColor Blue
Do {
  Write-Host '---------------------------------------------'
  $date = Get-Date
  Write-Host "status des installations @ $date" -ForegroundColor Blue

  if (-Not ( Test-Path "$HOME\AppData\Local\Android\Sdk" )) {
    Write-Host '    Android SDK nope.' -ForegroundColor Red
  }
  else {
    Write-Host '    Android SDK ok.' -ForegroundColor Green
  }
  if (-Not ( Test-Path "$HOME\jdk\fini.txt" )) {
    Write-Host '    Java JDK nope.' -ForegroundColor Red
  }
  else {
    Write-Host '    Java JDK  ok.' -ForegroundColor Green
  }
  if (-Not ( Test-Path "$HOME\flutter" )) {
    Write-Host '    Flutter SDK nope.' -ForegroundColor Red
  }
  else {
    Write-Host '    Flutter JDK ok.' -ForegroundColor Green
  }
  if (-Not ( Test-Path "$HOME\android-studio" )) {
    Write-Host '    Android Studio nope.'  -ForegroundColor Red
  }
  else {
    Write-Host '    Android Studio est déjà installé.'  -ForegroundColor Green
  }
  if (-Not ( Test-Path "$HOME\idea\fini.txt" )) {
    Write-Host '    Intellij nope.'  -ForegroundColor Red
  }
  else {
    Write-Host '    Intellij ok.'  -ForegroundColor Green
  }
  Start-Sleep -s 10  # attend 10 seecondes avant de regarder
} While ($true)

