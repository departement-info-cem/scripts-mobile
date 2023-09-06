$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

# https://www.how2shout.com/how-to/how-to-install-node-js-and-npm-on-windows-10-or-11-using-cmd.html
function Install-Npm() {

}


function Update-Npm() { 
     Write-Host '    ✔️  Mise à jour de NPM.'  -ForegroundColor Green
    npm install -g npm@latest
}

function Install-Firebase-Cli() {
     Write-Host '    ✔️ Installation de firebase cli.'  -ForegroundColor Green
     npm install -g firebase-tools
}

function Install-FlutterFire-Cli(){
    Write-Host '    ✔️ Installation de FlutterFire cli.'  -ForegroundColor Green
    dart pub global activate flutterfire_cli
}


Update-Npm
Install-Firebase-Cli
Install-FlutterFire-Cli

#$User = Read-Host -Prompt 'Installation de Firebase et Flutterfire faites, vous pouvez fermer cette fenetre'