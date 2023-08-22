. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8

$INITIAL_DIR = $HOME

# function Get-Env-Contains([string]$name, [string]$value) {
#     return [System.Environment]::GetEnvironmentVariable($name, "User") -like "*$value*"
# }

# function Invoke-Env-Reload() {
#     $env:Path = [System.Environment]::GetEnvironmentVariable("Path", "Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path", "User")
#     $env:ANDROID_SDK_ROOT = [System.Environment]::GetEnvironmentVariable("ANDROID_SDK_ROOT", "User")
#     $env:ANDROID_HOME = [System.Environment]::GetEnvironmentVariable("ANDROID_HOME", "User")
# }

# function Add-Env([string]$name, [string]$value) {
#     if (-Not (Get-Env-Contains $name $value) ) {
#         Write-Host '    👍 Ajout de'$value' à'$name'.' -ForegroundColor Blue
#         $new_value = [Environment]::GetEnvironmentVariable("$name", "User")
#         if (-Not ($new_value -eq $null)) {
#             $new_value += [IO.Path]::PathSeparator
#         }
#         $new_value += $value
#         [Environment]::SetEnvironmentVariable( "$name", $new_value, "User" )
#         if (Get-Env-Contains $name $new_value) {
#             Invoke-Env-Reload
#             Write-Host '    ✔️  '$value' ajouté à '$name'.'  -ForegroundColor Green
#         }
#         else {
#             Set-Location $INITIAL_DIR
#             Write-Host '    ❌ '$value' n''a pas été ajouté à '$name'.' -ForegroundColor Red
#             exit
#         }
#     }
#     else {
#         Write-Host '    ✔️ '$value' déjà ajouté à '$name'.'  -ForegroundColor Green
#     }
# }

function replaceInFile([string] $filePath, [string] $toReplace, [string] $replacement) {
    # Read the file content using the Get-Content
    $filecontent = Get-Content -Path $filePath -Raw
    $modifiedContent = $filecontent.Replace($toReplace, $replacement)
    # Save the replace line in a file  
    Set-Content -Path $filePath -Value $modifiedContent
}

Invoke-Env-Reload

Write-Host '🕰️  ANDROID EMULATOR Mise à jour des variables d''environnement' -ForegroundColor Blue

function Start-Emulator() {
    Set-Location $INITIAL_DIR
    Add-Env "ANDROID_SDK_ROOT" "$HOME\AppData\Local\Android\Sdk"
    Add-Env "ANDROID_HOME" "$env:ANDROID_SDK_ROOT"
    Append-Env "Path" $HOME\AppData\Local\Android\cmdline-tools\latest\bin
    Append-Env "Path" $HOME"\AppData\Local\Android\Sdk\emulator"

    Write-Host '👾  Création de la machine virtuelle' -ForegroundColor Blue

    avdmanager -s create avd -n pixel --device "pixel_4" -k "system-images;android-34;google_apis;x86_64"
   
    Write-Host '👾  Activation du clavier sur émulateur' -ForegroundColor Blue
   
    replaceInFile $HOME"\.android\avd\pixel.avd\config.ini" "hw.keyboard=no" "hw.keyboard=yes"
    replaceInFile $HOME"\.android\avd\pixel.avd\config.ini" "PlayStore.enabled=no" "PlayStore.enabled=yes"
    Write-Host '👾  Démarrage de la machine virtuelle' -ForegroundColor Blue
    #powershell -c $HOME'\AppData\Local\Android\Sdk\emulator\emulator.exe -avd pixel'
    Start-Process -FilePath $HOME'\AppData\Local\Android\Sdk\emulator\emulator.exe' -ArgumentList '-avd pixel' -NoNewWindow
    $User = Read-Host -Prompt 'Emulateur démarré ne pas fermer cette fenetre'
}

Start-Emulator
