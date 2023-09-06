$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Write-Host 'Premier projet Flutter.' -ForegroundColor Blue
Set-Location $HOME
flutter create fake_start
Write-Host '    👍 Premier démarrage.' -ForegroundColor Blue
Set-Location $HOME\fake_start

#TODO wait on emulator to exist
Wait-Until-File-Exists("$HOME\.android\avd\pixel.ini")
flutter run
