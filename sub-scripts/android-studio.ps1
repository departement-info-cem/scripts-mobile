$INITIAL_DIR = $HOME

$CACHE = "\\ed5depinfo\Logiciels\Android\scripts\cache"

$STUDIO_URL = 'https://redirector.gvt1.com/edgedl/android/studio/ide-zips/2021.3.1.17/android-studio-2021.3.1.17-windows.zip'
$FLUTTER_PLUGIN_URL_STUDIO = 'https://plugins.jetbrains.com/plugin/download?rel=true&updateId=231426'
$DART_PLUGIN_URL_STUDIO = 'https://plugins.jetbrains.com/plugin/download?rel=true&updateId=229981'
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

# Source : https://stackoverflow.com/a/9701907
function Add-Shortcut([string]$source_exe, [string]$name) {
    $WshShell = New-Object -ComObject WScript.Shell
    $Shortcut = $WshShell.CreateShortcut("$HOME\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\$name.lnk")
    $Shortcut.TargetPath = $source_exe
    $Shortcut.Save()
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
            Set-Location $INITIAL_DIR
            Write-Host '    ❌ '$value' n''a pas été ajouté à '$name'.' -ForegroundColor Red
            exit
        }
    }
    else {
        Write-Host '    ✔️ '$value' déjà ajouté à '$name'.'  -ForegroundColor Green
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
    if (Test-Path  $InstallLocation\$FinalDir ) {
        Write-Host '    ✔️ '$Name' installé.' -ForegroundColor Green
    }
    else {
        Set-Location $INITIAL_DIR
        Write-Host '    ❌  Échec lors de l''installation de'$Name'.' -ForegroundColor Red
    }
}

[void](New-Item -type directory -Path "$CACHE" -Force)

Invoke-Env-Reload

Write-Host '🤖  Android Studio' -ForegroundColor Blue

if (-Not ( Test-Path $HOME\android-studio )) {
    Invoke-Install "Android Studio" "$HOME" "android-studio\bin" "android-studio"
}
else {
    Write-Host '    ✔️  Android Studio est déjà installé.'  -ForegroundColor Green
}

Add-Shortcut $HOME\android-studio\bin\studio64.exe "Android Studio"
Add-Env "Path" "$HOME\android-studio\bin"

if (-Not(Test-Path $HOME\android-studio\plugins\flutter-intellij)) {
    Invoke-Install "plugin Flutter" "$HOME\android-studio\plugins" "flutter-intellij" "plugin-flutter-android-studio"
}
else {
    Write-Host '    ✔️  Le plugin Flutter est déjà installé.'  -ForegroundColor Green
}

if (-Not(Test-Path $HOME\android-studio\plugins\dart)) {
    Invoke-Install "plugin Dart" "$HOME\android-studio\plugins" "dart" "plugin-dart-android-studio"
}
else {
    Write-Host '    ✔️  Le plugin Dart est déjà installé.'  -ForegroundColor Green
}

$User = Read-Host -Prompt 'Installation de Android Studio complétée, tu peux fermer cette fenetre'
