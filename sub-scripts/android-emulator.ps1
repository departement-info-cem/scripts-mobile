$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Write-Host 'Emulateur Android ' -ForegroundColor Blue

Set-Location $HOME
Add-Env "ANDROID_SDK_ROOT" "$HOME\AppData\Local\Android\Sdk"
Add-Env "ANDROID_HOME" "$env:ANDROID_SDK_ROOT"
Append-Env "Path" $HOME\AppData\Local\Android\Sdk\cmdline-tools\latest\bin
Append-Env "Path" $HOME\AppData\Local\Android\Sdk\emulator

# il faut attendre le JDK plus récent et recharger les variables d'environnement
Invoke-Env-Reload
$finijavapath = "$HOME\jdk\fini.txt"
Wait-Until-File-Exists($finijavapath)

Write-Host '👾  Création de la machine virtuelle' -ForegroundColor Blue
avdmanager -s create avd -n pixel --device "pixel_5" -k "system-images;android-34;google_apis;x86_64"

Write-Host '👾  Activation du clavier sur émulateur' -ForegroundColor Blue
replaceInFile $HOME"\.android\avd\pixel.avd\config.ini" "hw.keyboard=no" "hw.keyboard=yes"
replaceInFile $HOME"\.android\avd\pixel.avd\config.ini" "PlayStore.enabled=no" "PlayStore.enabled=yes"

# TODO trouver un moyen pour que fermer la fenêtre ne ferme pas l'émulateur
Write-Host '👾  Démarrage de la machine virtuelle' -ForegroundColor Blue
Start-Process -FilePath $HOME'\AppData\Local\Android\Sdk\emulator\emulator.exe' -ArgumentList '-avd pixel' -NoNewWindow

# on donne 15 secondes emulateur pour partir
Start-Sleep -s 15
Start-Process powershell -argument "${env:scripty.scriptPath}\start-flutter.ps1"

$User = Read-Host -Prompt 'Emulateur démarré ne pas fermer cette fenetre'
