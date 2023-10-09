$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

# TODO put everything in 7zip more compressed format

$cachecache = "\\ed5depinfo\Logiciels\Android\scripts\cachecache\"
$tempcache = $HOME + "\tempcache\"

Try{
    Remove-Item -LiteralPath $tempcache -Force -Recurse
}Catch{

}


[void](New-Item -type directory -Path $cachecache -Force)
[void](New-Item -type directory -Path $tempcache -Force)


function Local7Zip(){
    if (-Not ( Test-Path ${env:ProgramFiles}\7-Zip\7z.exe)) {
        Write-Host "Je ne trouve pas de 7zip sur l'ordi, je le télécharge dans temp"
        Invoke-WebRequest 'https://www.7-zip.org/a/7zr.exe' -OutFile "${env:scripty.localTempPath}\7z.exe"
        $sevenZipPath = "${env:scripty.localTempPath}\7z.exe"
    }
}


function AskWithDefault {
    Param(
        [parameter(Mandatory = $true)]
        [String]
        $Prompt,
        [parameter(Mandatory = $true)]
        [String]
        $DefaultValue
    )
    $fullPrompt = $Prompt + " ... valeur par défaut sera " + $DefaultValue    
    $userAnswer = Read-Host -Prompt $fullPrompt

    if ([string]::IsNullOrWhiteSpace($Interesting))
    {
        $userAnswer = $DefaultValue
    }
    Write-Host 'On continue avec ' + $userAnswer
    return $userAnswer
}

function GoGetIt {
    Param(
        [parameter(Mandatory = $true)]
        [String]
        $Url,
        [parameter(Mandatory = $true)]
        [String]
        $Directory,
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
    $tempFolder = $tempcache + $Directory + "\"
    Expand-Archive $localTempPath -DestinationPath $tempcache$Directory"\"
    # Delete original Zip
    # Remove-Item -Path $localTempPath
    $newZipname = "original" + $ZipName
    Rename-Item -Path $localTempPath -NewName $newZipname
    # Zip it back
    $source = $tempcache + $Directory + "\*"
    #$dest = $tempcache + 'zzz' + $ZipName
    $dest = $localTempPath
    Compress-Archive -Path $source -DestinationPath $dest
    # Delete temp folder
    # Remove-Item -Path $tempFolder -Recurse
    # Push it to the cachecache
}
$jdkURL = AskWithDefault "Quelle URL pour le JDK?" $CORRETTO_URL
$asURL = AskWithDefault "Quelle URL pour Android Studio?" $STUDIO_URL

$asDartURL = AskWithDefault "Plugin dart?" $DART_PLUGIN_URL_STUDIO
$asFlutterURL = AskWithDefault "Plugin flutter?" $FLUTTER_PLUGIN_URL_STUDIO
$asFlutterIntlURL = AskWithDefault "Plugin flutter?" $FLUTTER_INTL_PLUGIN_URL_STUDIO

$ijURL = AskWithDefault "Quelle URL pour Intellij?" $IDEA_URL
$flutterURL = AskWithDefault "Quelle URL pour Flutter?" $FLUTTER_SDK



GoGetIt $jdkURL "jdk" "jdk.zip"
GoGetIt $asURL "android-studio" "android-studio.zip"
GoGetIt $asDartURL "asDart" "plugin-dart-android-studio.zip"
GoGetIt $asFlutterURL "asFlutter" "plugin-flutter-android-studio.zip"
GoGetIt $asFlutterIntlURL "asFlutterIntl" "plugin-flutter-intl-android-studio.zip"

GoGetIt $ijURL "idea" "idea.zip"
GoGetIt $flutterURL "flutter" "flutter.zip"

#vExpand-Archive $tempcache'\android-studio.zip' -DestinationPath $HOME"as\"

$androidBinPath = $HOME"\tempcache\android-studio\android-studio\bin\studio64.exe"
#Start-Process 
# TODO build SDK b so unzip Android Studio and start it.

Read-Host -Prompt 'Ici il faut partir Android Studio pour procéder aux install du SDK, appuyez sur ENTER quand fait'
Write-Host 'On va maintenant procéder au zippage du SDK et depot dans la cache'
Invoke-Zip  "$tempcache\Sdk.7z" "$HOME\AppData\Local\Android\Sdk"



