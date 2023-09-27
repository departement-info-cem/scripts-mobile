Start-Transcript -Path ${env:scripty.localTempPath}\transcript-cours-repo.txt
Invoke-WebRequest "https://github.com/departement-info-cem/3N5-Prog3/archive/refs/heads/main.zip" -OutFile "${env:scripty.localTempPath}3n5.zip"
Expand-Archive "${env:scripty.localTempPath}3n5.zip" -DestinationPath $HOME -Force

Invoke-WebRequest "https://github.com/departement-info-cem/4N6-Mobile/archive/refs/heads/master.zip" -OutFile "${env:scripty.localTempPath}4n6.zip"
Expand-Archive "${env:scripty.localTempPath}4n6.zip" -DestinationPath $HOME -Force

Invoke-WebRequest "https://github.com/departement-info-cem/5N6-mobile-2/archive/refs/heads/main.zip" -OutFile "${env:scripty.localTempPath}5n6.zip"
Expand-Archive "${env:scripty.localTempPath}5n6.zip" -DestinationPath $HOME -Force

Invoke-WebRequest "https://github.com/departement-info-cem/KickMyB-Server/archive/refs/heads/main.zip" -OutFile "${env:scripty.localTempPath}kmb.zip"
Expand-Archive "${env:scripty.localTempPath}kmb.zip" -DestinationPath $HOME -Force

