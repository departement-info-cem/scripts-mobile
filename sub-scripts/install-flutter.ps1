$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8

$INITIAL_DIR = $HOME

$DOWNLOADS = "\\ed5depinfo\Logiciels\Android\scripts\cache"


$FLUTTER_SDK = 'https://storage.googleapis.com/flutter_infra_release/releases/stable/windows/flutter_windows_3.3.9-stable.zip'


function Get-Env-Contains([string]$name, [string]$value) {
    return [System.Environment]::GetEnvironmentVariable($name, "User") -like "*$value*"
}

function Invoke-Env-Reload() {
    $env:Path = [System.Environment]::GetEnvironmentVariable("Path", "User") + ";" + [System.Environment]::GetEnvironmentVariable("Path", "Machine")
    $env:ANDROID_SDK_ROOT = [System.Environment]::GetEnvironmentVariable("ANDROID_SDK_ROOT", "User")
    $env:ANDROID_HOME = [System.Environment]::GetEnvironmentVariable("ANDROID_HOME", "User")
}

# Source : https://stackoverflow.com/a/9701907
function Add-Shortcut([string]$source_exe, [string]$name) {
    $WshShell = New-Object -ComObject WScript.Shell
    $Shortcut = $WshShell.CreateShortcut("$HOME\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\$name.lnk")
    $Shortcut.TargetPath = $source_exe
    $Shortcut.Save()
}
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

function Invoke-Download {
    Param(
        [parameter(Mandatory = $true)]
        [String]
        $Name,
        [parameter(Mandatory = $true)]
        [String]
        $Url,
        [parameter(Mandatory = $true)]
        [String]
        $ZipName
    )
    if ( -Not ( Test-Path $DOWNLOADS\$ZipName.zip)) {
        Write-Host '    👍 Téléchargement de'$Name' débuté.' -ForegroundColor Blue
        Set-Location $DOWNLOADS
        $ProgressPreference = 'SilentlyContinue'
        Invoke-WebRequest $Url -OutFile "$ZipName.zip"
        $ProgressPreference = 'Continue'
                
        if (Test-Path $DOWNLOADS/$ZipName.zip ) {
            Write-Host '    ✔️ '$Name' téléchargé.' -ForegroundColor Green
        }
        else {
            Set-Location $INITIAL_DIR
            Write-Host '    ❌ '$Name' n''a pas pu être téléchargé.' -ForegroundColor Red
            exit
        }
    }
    else {
        Write-Host '    ✔️ '$Name' est déjà téléchargé.' -ForegroundColor Green
    }
}

function Invoke-Install() {
    Param(
        [parameter(Mandatory = $true)]
        [String]
        $Name,
        [parameter(Mandatory = $true)]
        [String]
        $InstallLocation,
        [parameter(Mandatory = $true)]
        [String]
        $FinalDir,
        [parameter(Mandatory = $true)]
        [String]
        $ZipName
    )
    Write-Host '    👍 Extraction de'$Name' débuté.' -ForegroundColor Blue
    $ZIP_LOCATION = Get-ChildItem $DOWNLOADS\"$ZipName.zip"
    Copy-Item  $ZIP_LOCATION -Destination "$HOME\$ZipName.zip"
    $ProgressPreference = 'SilentlyContinue'
    & ${env:ProgramFiles}\7-Zip\7z.exe x "$HOME\$ZipName.zip" "-o$($InstallLocation)" -y
    # Expand-7Zip -ArchiveFileName "$HOME\$ZipName.zip" -TargetPath $InstallLocation
    #Expand-Archive "$HOME\$ZipName.zip" 
    $ProgressPreference = 'Continue'
}

[void](New-Item -type directory -Path "$DOWNLOADS" -Force)

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