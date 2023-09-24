$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Invoke-Env-Reload
Write-Host 'Flutter installation'

if (-Not ( Test-Path $HOME\flutter )) {
    Invoke-Install "Flutter" "$HOME" "flutter.zip"

}
else {
    Write-Host '    Flutter est déjà installé.'  -ForegroundColor Green
}
Write-Host 'MAJ des variables environnement' -ForegroundColor Blue
Remove-Env "Path" "C:\Flutter\bin"
Append-Env "Path" "$HOME\flutter\bin"
[void](flutter config --android-sdk "$HOME\AppData\Local\Android\Sdk")
[void](flutter config --android-studio-dir="$HOME\android-studio")
Write-Host '    Mise à jour'
#[void](flutter upgrade)
Write-Host '    Accepter les licenses. En attente du SDK Android ...'
flutter doctor --android-licenses


# TODO wait until a certain file in SDK exists
Write-Host 'Flutter'

Install-Flutter
[void](flutter config --android-sdk "$HOME\AppData\Local\Android\Sdk")
[void](flutter config --android-studio-dir="$HOME\android-studio")
#Write-Host '    Mise à jour' -ForegroundColor Blue
#[void](flutter upgrade)
Write-Host '    Accepter les licenses.' -ForegroundColor Blue
flutter doctor --android-licenses

Set-Location $HOME
Write-Host '✔️ ✔️ ✔️  Mise en place complétée ✔️ ✔️ ✔️'`n -ForegroundColor Green
flutter doctor
flutter precache

Start-Process powershell -argument "${env:scripty.scriptPath}\firebase-flutterfire.ps1"

