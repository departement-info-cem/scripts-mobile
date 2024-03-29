﻿$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Start-Transcript -Path ${env:scripty.localTempPath}\transcript-ij-installe.txt
Invoke-Env-Reload
Write-Host "Intellij Idea installation"

if (-Not ( Test-Path $HOME\idea )) {
    Invoke-Install "intellij" "$HOME\idea"  "idea.7z"
}
else {
    Write-Host '    ✔️  IntelliJ est déjà installé.'  -ForegroundColor Green
}

Add-Shortcut $HOME\idea\bin\idea64.exe "IntelliJ IDEA Community"
Add-Env "Path" "$HOME\idea\bin"
Start-Process -FilePath "$HOME\idea\bin\idea64.exe"