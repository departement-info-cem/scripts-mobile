$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Write-Host 'SDK Android' -ForegroundColor Blue

Check-Or-Install-Java

# Detecter si un SDK sur l'ordinateur

if (-Not ( Test-Path "${env:scripty.cachePath}\Sdk.7z" )) {
    Write-Host '    Pas de SDK trouvé en cache. Il va falloir construire' -ForegroundColor Green
    [void](New-Item -type directory -Path "$HOME\AppData\Local\Android\Sdk" -Force)
    Write-Host '    TODO installer cmd-tools tout configurer, faire update puis zipper et poser dans le cache' -ForegroundColor Green


    
    Invoke-Download "Android SDK manager" $ANDROID_SDK_MANAGER "sdk-manager"
    Invoke-Install "Android SDK manager" "$HOME\sdk-manager" "sdk-manager.zip"

    #Invoke-Download "Android Platform Tools" $ANDROID_PLATFORM_TOOLS "sdk-tools"
    #Invoke-Install "Android Platform Tools" "$HOME\sdk-tools" "sdk-tools.zip"

    # Merci a https://stackoverflow.com/questions/65262340/cmdline-tools-could-not-determine-sdk-root

    Remove-Item -LiteralPath "$HOME\AppData\Local\Android" -Force -Recurse
    [void](New-Item -type directory -Path "$HOME\AppData\Local\Android\Sdk" -Force)
    [void](New-Item -type directory -Path "$HOME\AppData\Local\Android\cmdline-tools\latest\" -Force)

    Move-Item "$HOME\sdk-manager\cmdline-tools\*" -Destination "$HOME\AppData\Local\Android\cmdline-tools\latest\" -Force #-Recurse
    #Move-Item "$HOME\sdk-tools\platform-tools" -Destination "$HOME\AppData\Local\Android\Sdk" -Force #-Recurse


    Add-Env "ANDROID_SDK_ROOT" "$HOME\AppData\Local\Android"
    Add-Env "ANDROID_HOME" "$env:ANDROID_SDK_ROOT"
    Append-Env "Path" "$HOME\AppData\Local\Android\cmdline-tools\latest\bin"
    
    Write-Host '    a' -ForegroundColor Green
    (1..10 | ForEach-Object {"y"; Start-Sleep -Milliseconds 100 }) | sdkmanager "cmdline-tools;latest"
    Write-Host '    b' -ForegroundColor Green
    sdkmanager  'platform-tools' "build-tools;33.0.2" "platforms;android-33" 
    Write-Host '    b' -ForegroundColor Green
    sdkmanager "system-images;android-33;google_apis_playstore;x86_64"
    Write-Host '    c' -ForegroundColor Green
    sdkmanager emulator
    Write-Host '    d' -ForegroundColor Green
    (1..10 | ForEach-Object {"y"; Start-Sleep -Milliseconds 100 }) | sdkmanager --licenses
    sdkmanager --update
    #Append-Env "Path" "$HOME\AppData\Local\Android\Sdk\emulator"
    & ${env:ProgramFiles}\7-Zip\7z.exe a -tzip ${env:scripty.cachePath}\Android.zip -mx0 $HOME\AppData\Local\Android  -y 

    # 7z u compressed.7z -u!update.7z -mx0 *.zip  to zip the produced sdk without compression

}
else {
    Write-Host '    Cache contient un SDK. On va le copier et installer' -ForegroundColor Green
    # Detecter si un SDK est présent sur la cache
    if (-Not ( Test-Path "$HOME\AppData\Local\Android\Sdk" )) {
        [void](New-Item -type directory -Path "$HOME\AppData\Local\Android\Sdk" -Force)
        Invoke-Install "Android SDK" "$HOME\AppData\Local\Android" "Sdk.7z"
    }
    else {
        Write-Host '    ✔️  Android SDK déjà copié et déjà installé.' -ForegroundColor Green
    }
}

#sdkmanager  'platform-tools' "build-tools;33.0.2" "cmdline-tools;latest" "emulator"

#Write-Host '🕹️  INSTALLATION SDK ANDROID Command Line Tools' -ForegroundColor Blue

#Write-Host '🧮  Installation des outils de développement d''Android' -ForegroundColor Blue

#Write-Host '    👍 Installation démarrée.' -ForegroundColor Blue

# sdkmanager 'platform-tools' "platforms;android-$CURRENT_SDK_VERSION" "system-images;android-$CURRENT_SDK_VERSION;google_apis;x86_64" "build-tools;$CURRENT_BUILD_TOOLS_VERSION" "cmdline-tools;latest"

Start-Process powershell -argument "${env:scripty.scriptPath}\android-emulator.ps1"

#Write-Host '    ✔️  Outils installé' -ForegroundColor Green

#sdkmanager --list_installed

$studiopath = "$HOME\android-studio\bin\studio64.exe"
Wait-Until-File-Exists($studiopath)
Start-Sleep -s 30
Start-Process -FilePath $studiopath

#$User = Read-Host -Prompt 'Installation du SDK Android terminée vous devriez pouvoir partir Android Studio et fermer cette fenetre'
