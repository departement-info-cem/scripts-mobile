﻿$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Invoke-Env-Reload

Do {
  Write-Host '---------------------------------------------'
  Write-Host 'status des installations'Get-Date'.' -ForegroundColor Blue

  if (-Not ( Test-Path "$HOME\AppData\Local\Android\Sdk" )) {
    Write-Host '    Android SDK pas là.' -ForegroundColor Red
  }
  else {
    Write-Host '    ✔️  Android SDK déjà copié et déjà installé.' -ForegroundColor Green
  }


  if (-Not ( Test-CommandExists('java') )) {
    Write-Host '    Java JDK pas là.' -ForegroundColor Red
  }
  else {
    Write-Host '    ✔️  Java JDK  installé.' -ForegroundColor Green
  }

  if (-Not ( Test-Path $HOME\flutter )) {
    Write-Host '    Flutter SDK pas là.' -ForegroundColor Red
  }
  else {
    Write-Host '    ✔️  Flutter JDK  installé.' -ForegroundColor Green
  }



  if (-Not ( Test-Path $HOME\android-studio )) {
    Write-Host '    Android Studio pas là.'  -ForegroundColor Red
  }
  else {
    Write-Host '    ✔️  Android Studio est déjà installé.'  -ForegroundColor Green
  }


  if (-Not ( Test-Path $HOME\idea )) {
    Write-Host '    Intellij pas là.'  -ForegroundColor Red
  }
  else {
    Write-Host '    ✔️  Intellij est déjà installé.'  -ForegroundColor Green
  }

  Start-Sleep -s 10  # attend 10 seecondes avant de regarder

} While ($true)
