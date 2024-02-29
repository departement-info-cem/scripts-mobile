$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Start-Transcript -Path ${env:scripty.localTempPath}\transcript-dotgradle-obtient.txt
Copy-Item  "${env:scripty.cachePath}\.gradle.7z" "${env:scripty.localTempPath}\.gradle.7z"

Start-Script "${env:scripty.scriptPath}\dotgradle-installe.ps1"

