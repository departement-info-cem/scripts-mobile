Start-Transcript -Path ${env:scripty.localTempPath}\transcript-gh-kmb.txt

Invoke-WebRequest "https://github.com/departement-info-cem/KickMyB-Server/archive/refs/heads/main.zip" -OutFile "${env:scripty.localTempPath}kmb.zip"
Expand-Archive "${env:scripty.localTempPath}kmb.zip" -DestinationPath $HOME\Desktop -Force