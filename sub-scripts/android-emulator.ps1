$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Write-Host 'Emulateur Android ' -ForegroundColor Blue

Set-Location $HOME
Add-Env "ANDROID_SDK_ROOT" "$HOME\AppData\Local\Android\Sdk"
Add-Env "ANDROID_HOME" "$env:ANDROID_SDK_ROOT"
Append-Env "Path" $HOME\AppData\Local\Android\Sdk\cmdline-tools\latest\bin
Append-Env "Path" $HOME\AppData\Local\Android\Sdk\emulator

Write-Host '👾  Création de la machine virtuelle' -ForegroundColor Blue

avdmanager -s create avd -n pixel --device "pixel_5" -k "system-images;android-34;google_apis;x86_64"

Write-Host '👾  Activation du clavier sur émulateur' -ForegroundColor Blue
   
replaceInFile $HOME"\.android\avd\pixel.avd\config.ini" "hw.keyboard=no" "hw.keyboard=yes"
replaceInFile $HOME"\.android\avd\pixel.avd\config.ini" "PlayStore.enabled=no" "PlayStore.enabled=yes"

Write-Host '👾  Démarrage de la machine virtuelle' -ForegroundColor Blue

Start-Process -FilePath $HOME'\AppData\Local\Android\Sdk\emulator\emulator.exe' -ArgumentList '-avd pixel' -NoNewWindow

$User = Read-Host -Prompt 'Emulateur démarré ne pas fermer cette fenetre'
