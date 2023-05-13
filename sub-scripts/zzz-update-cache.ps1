$CACHE = "\\ed5depinfo\Logiciels\Android\scripts\cache"

$STUDIO_URL = 'https://redirector.gvt1.com/edgedl/android/studio/ide-zips/2022.2.1.18/android-studio-2022.2.1.18-windows.zip' # https://developer.android.com/studio
$IDEA_URL = 'https://download.jetbrains.com/idea/ideaIU-2023.1.win.zip' # https://www.jetbrains.com/idea/download/other.html
$CMD_LINE_TOOLS_URL = 'https://dl.google.com/android/repository/commandlinetools-win-9477386_latest.zip' # https://developer.android.com/studio#command-tools
$FLUTTER_PLUGIN_URL_IDEA = 'https://plugins.jetbrains.com/plugin/download?rel=true&updateId=314936' # https://plugins.jetbrains.com/plugin/9212-flutter/versions
$DART_PLUGIN_URL_IDEA = 'https://plugins.jetbrains.com/plugin/download?rel=true&updateId=307810' # https://plugins.jetbrains.com/plugin/6351-dart/versions
$FLUTTER_PLUGIN_URL_STUDIO = 'https://plugins.jetbrains.com/plugin/download?rel=true&updateId=314933' # https://plugins.jetbrains.com/plugin/9212-flutter/versions
$DART_PLUGIN_URL_STUDIO = 'https://plugins.jetbrains.com/plugin/download?rel=true&updateId=306468' # https://plugins.jetbrains.com/plugin/6351-dart/versions
$FLUTTER_SDK = 'https://storage.googleapis.com/flutter_infra_release/releases/stable/windows/flutter_windows_3.7.10-stable.zip' # https://docs.flutter.dev/get-started/install/windows

$SDK_LOCATION = "$HOME\AppData\Local\Android"

function Invoke-Download {
    Param(
        [parameter(Mandatory = $true)]
        [String]
        $Name,
        [parameter(Mandatory = $true)]
        [String]
        $Url,
        [parameter(Mandatory = $true)]
        [String]
        $DstFile,
        [parameter(Mandatory = $true)]
        [String]
        $Destination
    )

    Write-Host '    👍 Téléchargement de'$Name' débuté.' -ForegroundColor Blue

    $ProgressPreference = 'SilentlyContinue'
    Invoke-WebRequest $Url -OutFile "$Destination\$DstFile.zip"
    $ProgressPreference = 'Continue'
                
    if (Test-Path $Destination\$DstFile.zip ) {
        Write-Host '    ✔️ '$Name' téléchargé.' -ForegroundColor Green
    }
    else {
        Set-Location $INITIAL_DIR
        Write-Host '    ❌ '$Name' n''a pas pu être téléchargé.' -ForegroundColor Red
        exit
    }
}

function Invoke-Download {
    Param(
        [parameter(Mandatory = $true)]
        [String]
        $Name,
        [parameter(Mandatory = $true)]
        [String]
        $Url,
        [parameter(Mandatory = $true)]
        [String]
        $ZipName,
        [parameter(Mandatory = $true)]
        [String]
        $DOWNLOAD_DIR
    )
    if ( -Not ( Test-Path $DOWNLOAD_DIR\$ZipName.zip)) {
        Write-Host '    👍 Téléchargement de'$Name' débuté.' -ForegroundColor Blue
        Set-Location $DOWNLOAD_DIR
        $ProgressPreference = 'SilentlyContinue'
        Invoke-WebRequest $Url -OutFile "$ZipName.zip"
        $ProgressPreference = 'Continue'
                
        if (Test-Path $DOWNLOAD_DIR\$ZipName.zip ) {
            Write-Host '    ✔️ '$Name' téléchargé.' -ForegroundColor Green
        }
        else {
            Set-Location $INITIAL_DIR
            Write-Host '    ❌ '$Name' n''a pas pu être téléchargé.' -ForegroundColor Red
            exit
        }
    }
    else {
        Write-Host '    ✔️ '$Name' est déjà téléchargé.' -ForegroundColor Green
    }
}

function Invoke-Zip() {
    Param(
        [parameter(Mandatory = $true)]
        [String]
        $Name,
        [parameter(Mandatory = $true)]
        [String]
        $SrcDir
    )
    Write-Host '    👍 Compression de'$Name' débuté.' -ForegroundColor Blue

    $ProgressPreference = 'SilentlyContinue'
    & ${env:ProgramFiles}\7-Zip\7z.exe a $SrcDir $SrcDir -y
    $ProgressPreference = 'Continue'
}

Invoke-Zip "Sdk" $SDK_LOCATION\Sdk
Copy-Item $SDK_LOCATION\Sdk.7z -Destination $CACHE\Sdk.7z

Invoke-Download "Android Studio" $STUDIO_URL "android-studio" $CACHE
Invoke-Download "IntelliJ IDEA" $IDEA_URL "idea" $CACHE
Invoke-Download "Flutter" $FLUTTER_SDK "flutter" $CACHE
Invoke-Download "Plugin Dart Android Studio" $DART_PLUGIN_URL_STUDIO "plugin-dart-android-studio" $CACHE
Invoke-Download "Plugin Dart IntelliJ IDEA" $DART_PLUGIN_URL_IDEA "plugin-dart-idea" $CACHE
Invoke-Download "Plugin Flutter Android Studio" $FLUTTER_PLUGIN_URL_STUDIO "plugin-flutter-studio" $CACHE
Invoke-Download "Plugin Flutter IntelliJ IDEA" $FLUTTER_PLUGIN_URL_IDEA "plugin-flutter-idea" $CACHE
Invoke-Download "Flutter SDK" $FLUTTER_SDK "flutter" $CACHE
Invoke-Download "Android Command Line Tools" $CMD_LINE_TOOLS_URL "cmd-line-tools" $CACHE
