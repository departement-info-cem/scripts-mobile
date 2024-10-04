Start-Transcript -Path ${env:scripty.localTempPath}\transcript-gh-4n6.txt

Invoke-WebRequest "https://github.com/departement-info-cem/4N6-Mobile/archive/refs/heads/master.zip" -OutFile "${env:scripty.localTempPath}4n6.zip"
Expand-Archive "${env:scripty.localTempPath}4n6.zip" -DestinationPath $HOME\Desktop -Force