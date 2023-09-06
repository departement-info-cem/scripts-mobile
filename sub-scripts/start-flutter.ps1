$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

#wait on flutter to exist
Wait-Until-File-Exists("$HOME\flutter\bin\flutter")

Write-Host 'Démarrage Flutter.' -ForegroundColor Blue
Set-Location $HOME
flutter create fake_start
Write-Host '    👍 Premier démarrage.' -ForegroundColor Blue
Set-Location $HOME\fake_start

flutter run

$User = Read-Host -Prompt 'L appli devrait être partie sur émulateur'
