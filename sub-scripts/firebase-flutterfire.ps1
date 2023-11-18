$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"
Start-Transcript -Path ${env:scripty.localTempPath}\transcript-firebase-flutterfire.txt

function Install-Firebase-Cli() {
     Write-Host '    ✔️ Installation de firebase cli.'  -ForegroundColor Green
     npm install -g firebase-tools
}

function Install-FlutterFire-Cli(){
    Write-Host '    ✔️ Installation de FlutterFire cli.'  -ForegroundColor Green
    dart pub global activate flutterfire_cli
}

Write-Host 'Firebase et FlutterFire'
# https://stackoverflow.com/questions/70320263/error-the-term-flutterfire-is-not-recognized-as-the-name-of-a-cmdlet-functio
Append-Env "Path" $HOME\AppData\Local\Pub\Cache\bin
Install-Firebase-Cli
Install-FlutterFire-Cli

