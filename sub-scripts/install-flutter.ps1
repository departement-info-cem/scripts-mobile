. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"


# TODO cannot remove existing flutter PATH as it is defined in the Machine part of the PATH
function Remove-Env([string]$name, [string]$value) {
    $path = [System.Environment]::GetEnvironmentVariable(
    "$name",
    'User'
    )
    # Remove unwanted elements
    $path = ($path.Split(';') | Where-Object { $_ -ne '$value' }) -join ';'
    Write-Host $path
    # Set it
    [System.Environment]::SetEnvironmentVariable(
        "$name",
        $path,
        'User'
    )
}


Invoke-Env-Reload

Write-Host '🕰️  Mise à jour des variables d''environnement' -ForegroundColor Blue


function Install-Flutter() {
     Write-Host '🧠  Flutter SDK' -ForegroundColor Blue

     if (-Not ( Test-Path $HOME\flutter )) {
         Invoke-Download "Flutter" $FLUTTER_SDK "flutter" $false
         Invoke-Install "Flutter" "$HOME" "flutter.zip"

     }
     else {
         Write-Host '    ✔️  Flutter est déjà installé.'  -ForegroundColor Green
     }
     Write-Host 'MAJ des variables environnement' -ForegroundColor Blue
     Remove-Env "Path" "C:\Flutter\bin"
     Append-Env "Path" "$HOME\flutter\bin"
     [void](flutter config --android-sdk "$HOME\AppData\Local\Android\Sdk")
     [void](flutter config --android-studio-dir="$HOME\android-studio")
     Write-Host '    👍 Mise à jour' -ForegroundColor Blue
     [void](flutter upgrade)
     Write-Host '    👍 Accepter les licenses.' -ForegroundColor Blue
     flutter doctor --android-licenses
}

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

Install-Flutter
[void](flutter config --android-sdk "$HOME\AppData\Local\Android\Sdk")
[void](flutter config --android-studio-dir="$HOME\android-studio")
Write-Host '    👍 Mise à jour' -ForegroundColor Blue
[void](flutter upgrade)
Write-Host '    👍 Accepter les licenses.' -ForegroundColor Blue
flutter doctor --android-licenses
    
Set-Location $HOME
Write-Host '✔️ ✔️ ✔️  Mise en place complétée ✔️ ✔️ ✔️'`n -ForegroundColor Green
flutter doctor
Write-Host '    👍 Création de projet fake pour first run.' -ForegroundColor Blue
Set-Location $HOME
flutter create fake_start
Write-Host '    👍 Premier démarrage.' -ForegroundColor Blue
Set-Location $HOME\fake_start
flutter run
$User = Read-Host -Prompt 'La mise à jour de Flutter est faite, il faut attendre la fin de installation Android vous pouvez fermer cette fenetre'


#Update-Npm
#Install-Firebase-Cli
#Install-FlutterFire-Cli

 $User = Read-Host -Prompt 'Installation de Flutter est faite dans $HOME\flutter, vous pouvez fermer cette fenetre'