Start-Transcript -Path ${env:scripty.localTempPath}\transcript-non-a-win-defender.txt

Set-MpPreference -ExclusionPath "$HOME"
Set-MpPreference -ExclusionPath "C:\EspaceLabo"