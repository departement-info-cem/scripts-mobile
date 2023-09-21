$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Invoke-Install "Corretto Java Dev Kit" "$HOME\jdk" "jdk.zip"
$jdkVersion = (Get-ChildItem $HOME\jdk | Select-Object -First 1).Name
Add-Env "JAVA_HOME" "$HOME\jdk\$jdkVersion"
Append-Env "Path" "$HOME\jdk\$jdkVersion\bin"

Start-Process powershell -argument "${env:scripty.scriptPath}\jdk-obtient.ps1"