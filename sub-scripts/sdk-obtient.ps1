$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"
Invoke-Env-Reload
Write-Host "Obtention du SDK Android Studio ${env:scripty.scriptPath}"



If(${env:scripty.auCollege} -eq $true) {

    if (-Not ( Test-Path "${env:scripty.cachePath}\Sdk.7z" )) {
        Write-Host '    Pas de SDK trouvé en cache. Il va falloir construire'
        Write-Host '    Executer les scripts au complet puis lancer Android Studio et finir installation '

        Write-Host ' Quand fini, créer un Sdk.7z et le déposer sur \\ed5depinfo '
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

    Start-Process powershell -argument "${env:scripty.scriptPath}\android-emulator.ps1"

    # partir Android studio
    $finipath = "$HOME\android-studio"
    $studiopath = "$HOME\android-studio\bin\studio64.exe"
    Wait-Until-File-Exists($finipath)
    Write-Host 'Android Studio va partir dans 30 secondes'
    Start-Sleep -s 30
    Start-Process -FilePath $studiopath
}
Else{
    Write-Host "Optimisation du téléchargement du SDK inutile"
    Write-Host "Le SDK Android se téléchargera au premier lancement d'Android Studio"
    Start-Process powershell -argument "${env:scripty.scriptPath}\as-obtient.ps1"
    Start-Process powershell -argument "${env:scripty.scriptPath}\ij-obtient.ps1"
    Start-Process powershell -argument "${env:scripty.scriptPath}\flutter-obtient.ps1"
    Start-Process powershell -argument "${env:scripty.scriptPath}\server-obtient.ps1"
}

If(${env:scripty.devMode} -eq $true) {
    $User = Read-Host -Prompt 'Tu peux fermer cette fenetre.'
}
