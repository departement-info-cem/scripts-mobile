$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8

$INITIAL_DIR = $HOME

$DOWNLOADS = "Z:\.config\android"
$DOWNLOADS = "\\ed4depinfo\Cours\A22\3N5"
# $DOWNLOADS = $HOME + '\Downloads'

$STUDIO_URL = 'https://redirector.gvt1.com/edgedl/android/studio/ide-zips/2021.3.1.17/android-studio-2021.3.1.17-windows.zip'
$CMD_LINE_TOOLS_URL = 'https://dl.google.com/android/repository/commandlinetools-win-8512546_latest.zip'
$FLUTTER_PLUGIN_URL_IDEA = 'https://plugins.jetbrains.com/plugin/download?rel=true&updateId=231428'
$DART_PLUGIN_URL_IDEA = 'https://plugins.jetbrains.com/plugin/download?rel=true&updateId=233333'
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

function replaceInFile([string] $filePath, [string] $toReplace, [string] $replacement) {
        # Read the file content using the Get-Content
    $filecontent = Get-Content -Path $filePath -Raw
    $modifiedContent =  $filecontent.Replace($toReplace, $replacement)
    # Save the replace line in a file  
    Set-Content -Path $filePath -Value $modifiedContent
}


[void](New-Item -type directory -Path "$DOWNLOADS" -Force)

Invoke-Env-Reload

Write-Host '🕰️  ANDROID EMULATOR Mise à jour des variables d''environnement' -ForegroundColor Blue




function Start-Emulator() {
    Set-Location $INITIAL_DIR
    Add-Env "ANDROID_SDK_ROOT" "$HOME\AppData\Local\Android\Sdk"
    Add-Env "ANDROID_HOME" "$env:ANDROID_SDK_ROOT"
    Add-Env "Path" "$env:ANDROID_SDK_ROOT\cmdline-tools\version\bin"
    Add-Env "Path" $HOME"\AppData\Local\Android\Sdk\emulator"

    Write-Host '👾  Création de la machine virtuelle' -ForegroundColor Blue

    avdmanager -s create avd -n pixel --device "pixel_4" -k "system-images;android-33;google_apis;x86_64"
   
   

    Write-Host '👾  Activation du clavier sur émulateur' -ForegroundColor Blue
   
    replaceInFile $HOME"\.android\avd\pixel.avd\config.ini" "hw.keyboard=no" "hw.keyboard=yes"
    replaceInFile $HOME"\.android\avd\pixel.avd\config.ini" "PlayStore.enabled=no" "PlayStore.enabled=yes"
    Write-Host '👾  Démarrage de la machine virtuelle' -ForegroundColor Blue
    #powershell -c $HOME'\AppData\Local\Android\Sdk\emulator\emulator.exe -avd pixel'
    Start-Process -FilePath $HOME'\AppData\Local\Android\Sdk\emulator\emulator.exe' -ArgumentList '-avd pixel' -NoNewWindow
    $User = Read-Host -Prompt 'Emulateur démarré ne pas fermer cette fenetre'
}

Start-Emulator
