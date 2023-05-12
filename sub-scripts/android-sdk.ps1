$CACHE = "\\ed5depinfo\Logiciels\Android\scripts\cache"
. fonctions.ps1

Prout

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

Start-Process powershell -argument "\\ed5depinfo\Logiciels\Android\scripts\sub-scripts\android-emulator.ps1"

Write-Host '    ✔️  Outils installé' -ForegroundColor Green

sdkmanager --list_installed

$User = Read-Host -Prompt 'Installation du SDK Android terminée vous devriez pouvoir partir Android Studio et fermer cette fenetre'