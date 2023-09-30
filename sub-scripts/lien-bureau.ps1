$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Start-Transcript -Path ${env:scripty.localTempPath}\transcript-liens-bureau.txt

Add-Desktop-Shortcut "$HOME\android-studio\bin\studio64.exe" "androidstudio"

Add-Desktop-Shortcut  "$HOME\idea\bin\idea64.exe" "idea"
