. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

${env:scripty.debug} = $true
${env:scripty.debug} = $true

If (${env:scripty.dev} -eq $true) {
    ${env:scripty.cachePath} = "\\ed5depinfo\Logiciels\Android\scripts\cacheDev"
}
Else {
    ${env:scripty.cachePath} = "\\ed5depinfo\Logiciels\Android\scripts\cache"
}

$SDK_LOCATION = "$HOME\AppData\Local\Android"

Invoke-Zip "Sdk" $SDK_LOCATION\Sdk
Invoke-Copy "Sdk" $SDK_LOCATION\Sdk.7z ${env:scripty.cachePath}\Sdk.7z

Invoke-Download "Android Studio" $STUDIO_URL "android-studio" $true
Invoke-Download "IntelliJ IDEA" $IDEA_URL "idea" $true
Invoke-Download "Plugin Dart Android Studio" $DART_PLUGIN_URL_STUDIO "plugin-dart-android-studio" $true
Invoke-Download "Plugin Dart IntelliJ IDEA" $DART_PLUGIN_URL_IDEA "plugin-dart-idea" $true
Invoke-Download "Plugin Flutter Android Studio" $FLUTTER_PLUGIN_URL_STUDIO "plugin-flutter-studio" $true
Invoke-Download "Plugin Flutter IntelliJ IDEA" $FLUTTER_PLUGIN_URL_IDEA "plugin-flutter-idea" $true
Invoke-Download "Flutter SDK" $FLUTTER_SDK "flutter" $true
Invoke-Download "Android Command Line Tools" $CMD_LINE_TOOLS_URL "cmd-line-tools" $true
