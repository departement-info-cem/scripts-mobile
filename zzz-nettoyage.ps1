

# config du chemin local pour le stockage des GROS zips avant dézippage
${env:scripty.localTempPath} = "$HOME\temp\"

Remove-Item -LiteralPath "${env:scripty.localTempPath}" -Force -Recurse
Remove-Item -LiteralPath "$HOME\.android" -Force -Recurse
Remove-Item -LiteralPath "$HOME\.gradle" -Force -Recurse
Remove-Item -LiteralPath "$HOME\.m2" -Force -Recurse

Remove-Item -LiteralPath "$HOME\AndroidStudioProjects" -Force -Recurse

Remove-Item -LiteralPath "$HOME\idea" -Force -Recurse
Remove-Item -LiteralPath "$HOME\flutter" -Force -Recurse
Remove-Item -LiteralPath "$HOME\fake_start" -Force -Recurse

Remove-Item -LiteralPath "$HOME\jdk" -Force -Recurse
Remove-Item -LiteralPath "$HOME\android-studio" -Force -Recurse

Remove-Item -LiteralPath "$HOME\sdk-manager" -Force -Recurse
Remove-Item -LiteralPath "$HOME\sdk-tools" -Force -Recurse

Remove-Item -LiteralPath "$HOME\AppData\Local\Pub" -Force -Recurse
Remove-Item -LiteralPath "$HOME\AppData\Local\Android" -Force -Recurse
