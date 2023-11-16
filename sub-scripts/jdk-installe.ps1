$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Start-Transcript -Path ${env:scripty.localTempPath}\transcript-jdk-installe.txt
Invoke-Install "jdk" "$HOME\jdk" "jdk.7z"
$jdkVersion = (Get-ChildItem $HOME\jdk | Select-Object -First 1).Name
Add-Env "JAVA_HOME" "$HOME\jdk\$jdkVersion"
Append-Env "Path" "$HOME\jdk\$jdkVersion\bin"
