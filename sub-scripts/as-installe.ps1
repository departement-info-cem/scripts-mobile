﻿$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"
Start-Transcript -Path ${env:scripty.localTempPath}\transcript-as-installe.txt
Invoke-Env-Reload
Write-Host "Android Studio installation"

Add-Shortcut $HOME\android-studio\bin\studio64.exe "Android Studio"
Append-Env "Path" "$HOME\android-studio\bin"

Invoke-Install "android-studio" "$HOME" "android-studio-plugins.7z"

If(${env:scripty.auCollege} -eq $false) {
    $studiopath = "$HOME\android-studio\bin\studio64.exe"
    Start-Process -FilePath $studiopath
}
