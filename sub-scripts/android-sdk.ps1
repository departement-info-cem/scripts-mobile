. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"


Write-Host 'Mise en place du SDK Android' -ForegroundColor Blue







# Detecter si un SDK sur l'ordinateur

if (-Not ( Test-Path "${env:scripty.cachePath}\Android.zip" )) {
    Write-Host '    Pas de SDK trouvé en cache. Il va falloir construire' -ForegroundColor Green
    [void](New-Item -type directory -Path "$HOME\AppData\Local\Android\Sdk" -Force)
    Write-Host '    TODO installer cmd-tools tout configurer, faire update puis zipper et poser dans le cache' -ForegroundColor Green


    
    #Invoke-Download "Android SDK manager" $ANDROID_SDK_MANAGER "androidsdk"
    #Invoke-Install "Android SDK manager" "$HOME\androidsdk" "bin" "androidsdk"

    # Merci a https://stackoverflow.com/questions/65262340/cmdline-tools-could-not-determine-sdk-root

    #[void](New-Item -type directory -Path "$HOME\androidsdk\cmdline-tools\zzz" -Force)
    #Copy-Item "$HOME\androidsdk\cmdline-tools\*" -Destination "$HOME\androidsdk\cmdline-tools\zzz" -Recurse
    #[void](New-Item -type directory -Path "$HOME\androidsdk\cmdline-tools\latest" -Force)
    #Copy-Item "$HOME\androidsdk\cmdline-tools\zzz\*" -Destination "$HOME\androidsdk\cmdline-tools\latest\" -Recurse


    # 7z u compressed.7z -u!update.7z -mx0 *.zip  to zip the produced sdk without compression

}
else {
    Write-Host '    Cache contient un SDK. On va le copier et installer' -ForegroundColor Green
    # Detecter si un SDK est présent sur la cache
    if (-Not ( Test-Path "$HOME\AppData\Local\Android" )) {
        [void](New-Item -type directory -Path "$HOME\AppData\Local\Android\Sdk" -Force)
        Invoke-Install "Android SDK" "$HOME\AppData\Local" "Android"
    }
    else {
        Write-Host '    ✔️  Android SDK déjà copié et déjà installé. Mettre a jour????' -ForegroundColor Green
    }

    Write-Host '    ✔️  Android SDK déjà copié est déjà installé.' -ForegroundColor Green
}





Add-Env "ANDROID_SDK_ROOT" "$HOME\AppData\Local\Android"
Add-Env "ANDROID_HOME" "$env:ANDROID_SDK_ROOT"
Add-Env "Path" "$env:ANDROID_SDK_ROOT\cmdline-tools\bin"
Add-Env "Path" "$env:ANDROID_SDK_ROOT\Sdk\emulator"

#sdkmanager  'platform-tools' "build-tools" "cmdline-tools;latest" "emulator"

#Write-Host '🕹️  INSTALLATION SDK ANDROID Command Line Tools' -ForegroundColor Blue

#Write-Host '🧮  Installation des outils de développement d''Android' -ForegroundColor Blue

#Write-Host '    👍 Installation démarrée.' -ForegroundColor Blue

# sdkmanager 'platform-tools' "platforms;android-$CURRENT_SDK_VERSION" "system-images;android-$CURRENT_SDK_VERSION;google_apis;x86_64" "build-tools;$CURRENT_BUILD_TOOLS_VERSION" "cmdline-tools;latest"

#Start-Process powershell -argument "\\ed5depinfo\Logiciels\Android\scripts\sub-scripts\android-emulator.ps1"

#Write-Host '    ✔️  Outils installé' -ForegroundColor Green

#sdkmanager --list_installed

$User = Read-Host -Prompt 'Installation du SDK Android terminée vous devriez pouvoir partir Android Studio et fermer cette fenetre'