$CACHE = "\\ed5depinfo\Logiciels\Android\scripts\cache"

$CURRENT_SDK_VERSION = "30"
$CURRENT_BUILD_TOOLS_VERSION = "30.0.2"

function Get-Env-Contains([string]$name, [string]$value) {
    return [System.Environment]::GetEnvironmentVariable($name, "User") -like "*$value*"
}

function Invoke-Env-Reload() {
    $env:Path = [System.Environment]::GetEnvironmentVariable("Path", "Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path", "User")
    $env:ANDROID_SDK_ROOT = [System.Environment]::GetEnvironmentVariable("ANDROID_SDK_ROOT", "User")
    $env:ANDROID_HOME = [System.Environment]::GetEnvironmentVariable("ANDROID_HOME", "User")
}

function Add-Env([string]$name, [string]$value) {
    if (-Not (Get-Env-Contains $name $value) ) {
        Write-Host '    👍 Ajout de'$value' à'$name'.' -ForegroundColor Blue
        $new_value = [Environment]::GetEnvironmentVariable("$name", "User")
        if (-Not ($new_value -eq $null)) {
            $new_value += [IO.Path]::PathSeparator
        }
        $new_value += $value
        [Environment]::SetEnvironmentVariable( "$name", $new_value, "User" )
        if (Get-Env-Contains $name $new_value) {
            Invoke-Env-Reload
            Write-Host '    ✔️  '$value' ajouté à '$name'.'  -ForegroundColor Green
        }
        else {
            Set-Location $HOME
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
    if ( -Not ( Test-Path $CACHE\$ZipName.zip)) {
        Write-Host '    👍 Téléchargement de'$Name' débuté.' -ForegroundColor Blue
        Set-Location $CACHE
        $ProgressPreference = 'SilentlyContinue'
        Invoke-WebRequest $Url -OutFile "$ZipName.zip"
        $ProgressPreference = 'Continue'
                
        if (Test-Path $CACHE/$ZipName.zip ) {
            Write-Host '    ✔️ '$Name' téléchargé.' -ForegroundColor Green
        }
        else {
            Set-Location $HOME
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
    $ZIP_LOCATION = Get-ChildItem $CACHE\"$ZipName.zip"
    Copy-Item  $ZIP_LOCATION -Destination "$HOME\$ZipName.zip"
    $ProgressPreference = 'SilentlyContinue'
    & ${env:ProgramFiles}\7-Zip\7z.exe x "$HOME\$ZipName.zip" "-o$($InstallLocation)" -y 
    $ProgressPreference = 'Continue'
}

[void](New-Item -type directory -Path "$CACHE" -Force)

Write-Host '🕰️  Mise à jour des variables d''environnement' -ForegroundColor Blue

Add-Env "ANDROID_SDK_ROOT" "$HOME\AppData\Local\Android\Sdk"
Add-Env "ANDROID_HOME" "$env:ANDROID_SDK_ROOT"
Add-Env "Path" "$env:ANDROID_SDK_ROOT\cmdline-tools\version\bin"
Add-Env "Path" $HOME"\AppData\Local\Android\Sdk\emulator"

Write-Host '🕹️  INSTALLATION SDK ANDROID Command Line Tools' -ForegroundColor Blue

if (-Not ( Test-Path "$HOME\AppData\Local\Android\Sdk" )) {
    [void](New-Item -type directory -Path "$HOME\AppData\Local\Android\Sdk" -Force)
    Invoke-Install "Android SDK" "$HOME\AppData\Local\Android\" "android-studio\bin" "Sdk"
}
else {
    Write-Host '    ✔️  Android SDK déjà copié est déjà installé.' -ForegroundColor Green
}

Write-Host '🧮  Installation des outils de développement d''Android' -ForegroundColor Blue

Write-Host '    👍 Installation démarrée.' -ForegroundColor Blue

# sdkmanager 'platform-tools' "platforms;android-$CURRENT_SDK_VERSION" "system-images;android-$CURRENT_SDK_VERSION;google_apis;x86_64" "build-tools;$CURRENT_BUILD_TOOLS_VERSION" "cmdline-tools;latest"

Start-Process powershell -argument ".\android-emulator.ps1"

Write-Host '    ✔️  Outils installé' -ForegroundColor Green
sdkmanager --list_installed

$User = Read-Host -Prompt 'Installation du SDK Android terminée vous devriez pouvoir partir Android Studio et fermer cette fenetre'