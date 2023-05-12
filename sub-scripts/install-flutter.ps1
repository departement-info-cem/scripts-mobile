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
    # Set it
    [System.Environment]::SetEnvironmentVariable(
        "$name",
        $path,
        'User'
    )
}

function Add-Env([string]$name, [string]$value) {
    if (-Not (Get-Env-Contains $name $value) ) {
        Write-Host '    👍 Ajout de'$value' à'$name'.' -ForegroundColor Blue
        $new_value = [Environment]::GetEnvironmentVariable("$name", "User")
        if (-Not ($new_value -eq $null)) {
            $new_value += [IO.Path]::PathSeparator
        }
        $new_value = $value + $new_value
        [Environment]::SetEnvironmentVariable( "$name", $new_value, "User" )
        if (Get-Env-Contains $name $new_value) {
            Invoke-Env-Reload
            Write-Host '    ✔️  '$value' ajouté à '$name'.'  -ForegroundColor Green
        }
        else {
            Set-Location $INITIAL_DIR
            Write-Host '    ❌ '$value' n''a pas été ajouté à '$name'.' -ForegroundColor Red
            exit
        }
    }
    else {
        Write-Host '    ✔️ '$value' déjà ajouté à '$name'.'  -ForegroundColor Green
    }
}


Invoke-Env-Reload

Write-Host '🕰️  Mise à jour des variables d''environnement' -ForegroundColor Blue


function Install-Flutter() {
     Write-Host '🧠  Flutter SDK' -ForegroundColor Blue

    if (-Not ( Test-Path $HOME\flutter )) {
        Invoke-Download "Flutter" $FLUTTER_SDK "flutter"
        Invoke-Install "Flutter" "$HOME" "." "flutter"
       
    }
    else {
        Write-Host '    ✔️  Flutter est déjà installé.'  -ForegroundColor Green
    }
    Remove-Env "Path" "C:\Flutter\bin"
    Add-Env "Path" "$HOME\flutter\bin"
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
Update-Npm
Install-Firebase-Cli
Install-FlutterFire-Cli

 $User = Read-Host -Prompt 'Installation de Flutter est faite, vous pouvez fermer cette fenetre'