$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Start-Transcript -Path ${env:scripty.localTempPath}\transcript-flutter-execute.txt

Append-Env "Path" "$HOME\flutter\bin"
Invoke-Env-Reload

Write-Host 'Démarrage Flutter.' -ForegroundColor Blue

Wait-Until-File-Exists("$HOME\flutter\bin\flutter")
Start-Sleep -s 60

Set-Location $HOME
flutter create fake_start
Write-Host ' Premier démarrage.' -ForegroundColor Blue
Set-Location $HOME\fake_start

flutter run

$User = Read-Host -Prompt 'L appli devrait etre partie sur émulateur'
