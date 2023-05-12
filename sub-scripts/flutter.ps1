. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"


Invoke-Env-Reload

Write-Host '🕰️  Mise à jour des variables d''environnement' -ForegroundColor Blue

$INITIAL_DIR = $HOME
Set-Location $INITIAL_DIR
Write-Host '🐤  Flutter' -ForegroundColor Blue
[void](git config --global --add safe.directory C:/Flutter)
[void](flutter config --android-sdk "$HOME\AppData\Local\Android\Sdk")
[void](flutter config --android-studio-dir="$HOME\android-studio")
Write-Host '    👍 Mise à jour' -ForegroundColor Blue
[void](flutter upgrade)
Write-Host '    👍 Accepter les licenses.' -ForegroundColor Blue
flutter doctor --android-licenses
    
Set-Location $INITIAL_DIR
Write-Host '✔️ ✔️ ✔️  Mise en place complétée ✔️ ✔️ ✔️'`n -ForegroundColor Green
flutter doctor
Write-Host '    👍 Création de projet fake pour first run.' -ForegroundColor Blue
Set-Location $INITIAL_DIR
flutter create fake_start
Write-Host '    👍 Premier démarrage.' -ForegroundColor Blue
Set-Location $INITIAL_DIR\fake_start
flutter run
$User = Read-Host -Prompt 'La mise à jour de Flutter est faite, il faut attendre la fin de installation Android vous pouvez fermer cette fenetre'
