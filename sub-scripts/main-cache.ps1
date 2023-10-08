$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

$cachecache = "\\ed5depinfo\Logiciels\Android\scripts\cachecache\"
$tempcache = $HOME + "\tempcache\"

Remove-Item -LiteralPath $tempcache -Force -Recurse

[void](New-Item -type directory -Path $cachecache -Force)
[void](New-Item -type directory -Path $tempcache -Force)à

function AskWithDefault {
    [parameter(Mandatory = $true)]
        [String]
        $Prompt,
        [parameter(Mandatory = $true)]
        [String]
        $DefaultValue
    $fullPrompt = $Prompt + " ... valeur par défaut sera " + $DefaultValue    
    $userAnswer = Read-Host -Prompt ‘$ [some-value]’

    if ([string]::IsNullOrWhiteSpace($Interesting))
    {
        $userAnswer = $DefaultValue
    }
    Write-Host 'On continue avec ' + $userAnswer
}

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
$jdkURL = AskWithDefault "Quelle URL pour le JDK?" $CORRETTO_URL



GoGetIt $jdkURL "jdk.zip"

# TODO build SDK b

Read-Host -Prompt 'Ici il faut partir Android Studio pour procéder aux install du SDK, appuyez sur ENTER quand fait'
Write-Host 'On va maintenant procéder au zippage du SDK et depot dans la cache'

#GoGetIt $STUDIO_URL "android-studio.zip"
#GoGetIt $FLUTTER_PLUGIN_URL_STUDIO "plugin-flutter-android-studio.zip"
#GoGetIt $DART_PLUGIN_URL_STUDIO "plugin-dart-android-studio.zip"
#GoGetIt $FLUTTER_INTL_PLUGIN_URL_STUDIO "plugin-flutter-intl-android-studio.zip"

#GoGetIt $IDEA_URL "idea.zip"

#GoGetIt $FLUTTER_SDK "flutter.zip"
