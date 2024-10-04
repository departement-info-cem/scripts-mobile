Start-Transcript -Path ${env:scripty.localTempPath}\transcript-gh-3n5.txt

Invoke-WebRequest "https://github.com/departement-info-cem/3N5-Prog3/archive/refs/heads/main.zip" -OutFile "${env:scripty.localTempPath}3n5.zip"
Expand-Archive "${env:scripty.localTempPath}3n5.zip" -DestinationPath $HOME\Desktop -Force