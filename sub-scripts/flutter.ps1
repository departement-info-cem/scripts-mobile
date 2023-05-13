. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"


Invoke-Env-Reload

Write-Host '🐤  Flutter' -ForegroundColor Blue
[void](git config --global --add safe.directory C:/Flutter)



Write-Host '    👍 Premier démarrage.' -ForegroundColor Blue
Set-Location $INITIAL_DIR\fake_start
flutter run
$User = Read-Host -Prompt 'La mise à jour de Flutter est faite, il faut attendre la fin de installation Android vous pouvez fermer cette fenetre'
