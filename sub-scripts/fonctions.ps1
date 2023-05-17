

function Check-Or-Install-Java() {
   #if (Test-CommandExists "javac") {
   # Write-Host "On a un JDK ici ${env:JAVA_HOME}"
   #} else {
    # nécessite Java
    Write-Host "On a pas ouch un JDK"
    Invoke-Download "Corretto Java Dev Kit" $CORRETTO_URL "jdk"
    Invoke-Install "Corretto Java Dev Kit" "$HOME\jdk" "jdk"
    Add-Env "JAVA_HOME" "$HOME\jdk\jdk17.0.7_7"
    Append-Env "Path" "$HOME\jdk\jdk17.0.7_7\bin"

   #}
}


# per https://devblogs.microsoft.com/scripting/use-a-powershell-function-to-see-if-a-command-exists/
function Test-CommandExists([string]$name){
 $oldPreference = $ErrorActionPreference
 $ErrorActionPreference = ‘stop’
 try {
    if(Get-Command $name){return $true}
 }
 Catch {
    return $false
 }
 Finally {$ErrorActionPreference=$oldPreference}
} 

function Get-Env-Contains([string]$name, [string]$value) {
    Write-Host "looking for $value in $name"
    Write-Host [System.Environment]::GetEnvironmentVariable($name, "User")
    return [System.Environment]::GetEnvironmentVariable($name, "User") -like "*$value*"
}

function Invoke-Env-Reload() {
    $env:Path = [System.Environment]::GetEnvironmentVariable("Path", "Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path", "User")
    $env:ANDROID_SDK_ROOT = [System.Environment]::GetEnvironmentVariable("ANDROID_SDK_ROOT", "User")
    $env:ANDROID_HOME = [System.Environment]::GetEnvironmentVariable("ANDROID_HOME", "User")
}



function Append-Env([string]$name, [string]$value) {
    Write-Host "Ajout de $value a $name"
    if (-Not (Get-Env-Contains $name $value) ) {
        Write-Host '    👍 Ajout de'$value' à'$name'.' -ForegroundColor Blue
        $new_value = [Environment]::GetEnvironmentVariable("$name", "User")
        if (-Not ($new_value -eq $null)) {
            $new_value += [IO.Path]::PathSeparator
        }
        $new_value += $value + ";"
        Write-Host "nouvelle valeur $new_value"
        [Environment]::SetEnvironmentVariable( "$name", $new_value, "User" )
        if (Get-Env-Contains $name $new_value) {
            Invoke-Env-Reload
            Write-Host '    ✔️  '$value' ajouté à '$name'.'  -ForegroundColor Green
        }
        else {
            Write-Host '    ❌ '$value' n''a pas été ajouté à '$name'.' -ForegroundColor Red
            exit
        }
    }
    else {
        Write-Host '    ✔️ '$value' déjà ajouté à '$name'.'  -ForegroundColor Green
    }
}


function Invoke-Install() {
    Param(
        [parameter(Mandatory = $true)]
        [String]
        $Name,
        [parameter(Mandatory = $true)]
        [String]
        $InstallLocation,
        [parameter(Mandatory = $true)]
        [String]
        $ZipName
    )
    Write-Host '    👍 Extraction de'$Name' débuté.' -ForegroundColor Blue
    $ZIP_LOCATION = Get-ChildItem ${env:scripty.cachePath}\"$ZipName.zip"
    Copy-Item  $ZIP_LOCATION -Destination "${env:scripty.localTempPath}$ZipName.zip"
    $ProgressPreference = 'SilentlyContinue'
    # regarder si on a 7zip, sinon on utilise le dezippeur de PowerShell

    if (-Not ( Test-Path ${env:ProgramFiles}\7-Zip\7z.exe)){
        # pas de 7zip, c'Est plus lent
        Expand-Archive "${env:scripty.localTempPath}$ZipName.zip" -DestinationPath $InstallLocation
        $ProgressPreference = 'Continue'
    }
    else 
    {
        & ${env:ProgramFiles}\7-Zip\7z.exe x "${env:scripty.localTempPath}\$ZipName.zip" "-o$($InstallLocation)" -y 
        $ProgressPreference = 'Continue'
    }
    
    
}



function Add-Env([string]$name, [string]$value) {
    if (-not [Environment]::GetEnvironmentVariable("$name", "User")) {
        $env:FOO = 'bar' 
        [Environment]::SetEnvironmentVariable($name, $value, 'User')
        Write-Host '    ✔️ '$value' dans '$name'.'  -ForegroundColor Green
    }
    
    else {
        $existing = [Environment]::GetEnvironmentVariable("$name", "User")
        Write-Host '    X '$name' existe déjà et vaut '$existing'.'  -ForegroundColor Red
    }
}

# Source : https://stackoverflow.com/a/9701907
function Add-Shortcut([string]$source_exe, [string]$name) {
    $WshShell = New-Object -ComObject WScript.Shell
    $Shortcut = $WshShell.CreateShortcut("$HOME\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\$name.lnk")
    $Shortcut.TargetPath = $source_exe
    $Shortcut.Save()
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
        $ZipName
    )
    if ( -Not ( Test-Path ${env:scripty.cachePath}\$ZipName.zip)) {
        Write-Host '    👍 Téléchargement de'$Name' débuté.' -ForegroundColor Blue
        Set-Location ${env:scripty.cachePath}
        $ProgressPreference = 'SilentlyContinue'
        $done = $false
        try { 
           # tester optimisation performance avec  Start-BitsTransfer -Source $url -Destination $dest 
           $response = Invoke-WebRequest  $Url -OutFile "$ZipName.zip"
           $StatusCode = $Response.StatusCode
           $done = $true
        } 
        catch {
           $StatusCode = $_.Exception.Response.StatusCode.value__
           Write-Host "Erreur avec $StatusCode"
        }
        Write-Host "ca a marché $done  ou pas $StatusCode"
        $ProgressPreference = 'Continue'
                
        if (Test-Path ${env:scripty.cachePath}/$ZipName.zip ) {
            Write-Host '    ✔️ '$Name' téléchargé.' -ForegroundColor Green
        }
        else {
            Set-Location $HOME
            Write-Host '    ❌ '$Name' n''a pas pu être téléchargé.' -ForegroundColor Red
            exit
        }
    }
    else {
        Write-Host '    ✔️ '$Name' est déjà téléchargé.' -ForegroundColor Green
    }
}
