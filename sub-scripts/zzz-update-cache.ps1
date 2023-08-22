. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

$CACHE = "\\ed5depinfo\Logiciels\Android\scripts\cacheDev"

$STUDIO_URL = 'https://redirector.gvt1.com/edgedl/android/studio/ide-zips/2022.3.1.18/android-studio-2022.3.1.18-windows.zip' # https://developer.android.com/studio
$IDEA_URL = 'https://download.jetbrains.com/idea/ideaIU-2023.2.win.zip' # https://www.jetbrains.com/idea/download/other.html
$CMD_LINE_TOOLS_URL = 'https://dl.google.com/android/repository/commandlinetools-win-10406996_latest.zip' # https://developer.android.com/studio#command-tools
$FLUTTER_PLUGIN_URL_IDEA = 'https://plugins.jetbrains.com/plugin/download?rel=true&updateId=372033' # https://plugins.jetbrains.com/plugin/9212-flutter/versions
$DART_PLUGIN_URL_IDEA = 'https://plugins.jetbrains.com/plugin/download?rel=true&updateId=365479' # https://plugins.jetbrains.com/plugin/6351-dart/versions
$FLUTTER_PLUGIN_URL_STUDIO = 'https://plugins.jetbrains.com/plugin/download?rel=true&updateId=372031' # https://plugins.jetbrains.com/plugin/9212-flutter/versions
$DART_PLUGIN_URL_STUDIO = 'https://plugins.jetbrains.com/plugin/download?rel=true&updateId=332609' # https://plugins.jetbrains.com/plugin/6351-dart/versions
$FLUTTER_SDK = 'https://storage.googleapis.com/flutter_infra_release/releases/stable/windows/flutter_windows_3.13.0-stable.zip' # https://docs.flutter.dev/get-started/install/windows

$SDK_LOCATION = "$HOME\AppData\Local\Android"

Invoke-Zip "Sdk" $SDK_LOCATION\Sdk
Copy-Item $SDK_LOCATION\Sdk.7z -Destination $CACHE\Sdk.7z

Invoke-Download "Android Studio" $STUDIO_URL "android-studio" $CACHE
Invoke-Download "IntelliJ IDEA" $IDEA_URL "idea" $CACHE
Invoke-Download "Plugin Dart Android Studio" $DART_PLUGIN_URL_STUDIO "plugin-dart-android-studio" $CACHE
Invoke-Download "Plugin Dart IntelliJ IDEA" $DART_PLUGIN_URL_IDEA "plugin-dart-idea" $CACHE
Invoke-Download "Plugin Flutter Android Studio" $FLUTTER_PLUGIN_URL_STUDIO "plugin-flutter-studio" $CACHE
Invoke-Download "Plugin Flutter IntelliJ IDEA" $FLUTTER_PLUGIN_URL_IDEA "plugin-flutter-idea" $CACHE
Invoke-Download "Flutter SDK" $FLUTTER_SDK "flutter" $CACHE
Invoke-Download "Android Command Line Tools" $CMD_LINE_TOOLS_URL "cmd-line-tools" $CACHE
