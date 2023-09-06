$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
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

# TODO wait until a certain file in SDK exists 

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
flutter precache

Start-Process powershell -ArgumentList "-noexit", "-command [console]::windowwidth=200; [console]::windowtop=50; [console]::windowleft=50; [console]::windowheight=200;","${env:scripty.scriptPath}\start-flutter.ps1"
Start-Process powershell -argument "${env:scripty.scriptPath}\firebase-flutterfire.ps1"
#Update-Npm
#Install-Firebase-Cli
#Install-FlutterFire-Cli
