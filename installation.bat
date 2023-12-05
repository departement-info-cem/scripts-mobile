
# pwsh.exe -Command 'Invoke-WebRequest -uri "https://raw.githubusercontent.com/departement-info-cem/scripts-mobile/main/installation-mobile.ps1" -OutFile "c:\installation-mobile.ps1";'
curl "https://raw.githubusercontent.com/departement-info-cem/scripts-mobile/main/installation-mobile.ps1" -o %userprofile%\installe.ps1
pwsh.exe -File %userprofile%\installe.ps1
pause