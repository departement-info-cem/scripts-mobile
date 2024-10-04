Start-Transcript -Path ${env:scripty.localTempPath}\transcript-gn-5n6.txt

Invoke-WebRequest "https://github.com/departement-info-cem/5N6-mobile-2/archive/refs/heads/main.zip" -OutFile "${env:scripty.localTempPath}5n6.zip"
Expand-Archive "${env:scripty.localTempPath}5n6.zip" -DestinationPath $HOME\Desktop -Force