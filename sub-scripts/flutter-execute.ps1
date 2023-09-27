$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Start-Transcript -Path ${env:scripty.localTempPath}\transcript-flutter-execute.txt

Append-Env "Path" "$HOME\flutter\bin"
Invoke-Env-Reload


#$User = Read-Host -Prompt 'Block pour voir la fenetre'
Write-Host 'D�marrage Flutter.' -ForegroundColor Blue
#wait on flutter to exist

Wait-Until-File-Exists("$HOME\flutter\bin\flutter")
Start-Sleep -s 60

Set-Location $HOME
flutter create fake_start
Write-Host ' Premier d�marrage.' -ForegroundColor Blue
Set-Location $HOME\fake_start

flutter run

$User = Read-Host -Prompt 'L appli devrait etre partie sur �mulateur'
