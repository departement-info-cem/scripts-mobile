$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

$cachecache = "\\ed5depinfo\Logiciels\Android\scripts\cachecache\"
$tempcache = $HOME + "\tempcache\"

Remove-Item -Path $tempcache -Recurse
[void](New-Item -type directory -Path $cachecache -Force)
[void](New-Item -type directory -Path $tempcache -Force)

function GoGetIt {
    Param(
        [parameter(Mandatory = $true)]
        [String]
        $Url,
        [parameter(Mandatory = $true)]
        [String]
        $ZipName
    )
    $localTempPath = $tempcache + $ZipName
    Write-Host '    👍 Téléchargement de '$Url' débuté vers '$localTempPath -ForegroundColor Blue
    Set-Location $tempcache
    $ProgressPreference = 'Continue'
    $done = $false
    # TODO strange bug when doing it with android studio --- test with regular Invoke-Download?
    Start-BitsTransfer -Source $Url -Destination $localTempPath
    $ProgressPreference = 'Continue'

    #Unzip it somewhere
    Expand-Archive $localTempPath -DestinationPath $tempcache"a\"
    # Delete original Zip
    Remove-Item -Path $localTempPath
    # Zip it back
    $source = $tempcache+"a\*"
    #$dest = $tempcache + 'zzz' + $ZipName
    $dest = $localTempPath
    Compress-Archive -Path $source -DestinationPath $dest
    # Delete temp folder
    Remove-Item -Path $tempcache"a\" -Recurse
    # Push it to the cachecache
}

#GoGetIt $CORRETTO_URL "jdk.zip"

#GoGetIt $STUDIO_URL "android-studio.zip"
GoGetIt $FLUTTER_PLUGIN_URL_STUDIO "plugin-flutter-android-studio.zip"
GoGetIt $DART_PLUGIN_URL_STUDIO "plugin-dart-android-studio.zip"
GoGetIt $FLUTTER_INTL_PLUGIN_URL_STUDIO "plugin-flutter-intl-android-studio.zip"


#GoGetIt $FLUTTER_SDK "flutter.zip"
