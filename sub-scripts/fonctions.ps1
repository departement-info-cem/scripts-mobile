
$scope = "User"
#$scope = "Machine"

function Check-Or-Install-Java() {
    #if (Test-CommandExists "javac") {
    # Write-Host "On a un JDK ici ${env:JAVA_HOME}"
    #} else {
    # nécessite Java
    Write-Host "On a pas ouch un JDK"
    Invoke-Download "Corretto Java Dev Kit" $CORRETTO_URL "jdk" $false
    Invoke-Install "Corretto Java Dev Kit" "$HOME\jdk" "jdk.zip"
    $jdkVersion = (Get-ChildItem $HOME\jdk | Select-Object -First 1).Name
    Add-Env "JAVA_HOME" "$HOME\jdk\$jdkVersion"
    Append-Env "Path" "$HOME\jdk\$jdkVersion\bin"
    #}
}


# per https://devblogs.microsoft.com/scripting/use-a-powershell-function-to-see-if-a-command-exists/
function Test-CommandExists([string]$name) {
    $oldPreference = $ErrorActionPreference
    $ErrorActionPreference = ‘stop’
    try {
        if (Get-Command $name) { return $true }
    }
    Catch {
        return $false
    }
    Finally { $ErrorActionPreference = $oldPreference }
} 

function Get-Env-Contains([string]$name, [string]$value) {
    Write-Host "looking for $value in $name"
    Write-Host [System.Environment]::GetEnvironmentVariable($name, $scope)
    return [System.Environment]::GetEnvironmentVariable($name, $scope) -like "*$value*"
}

function Invoke-Env-Reload() {
    $env:Path = [System.Environment]::GetEnvironmentVariable("Path", "Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path", "User")
    $env:ANDROID_SDK_ROOT = [System.Environment]::GetEnvironmentVariable("ANDROID_SDK_ROOT", $scope)
    $env:JAVA_HOME = [System.Environment]::GetEnvironmentVariable("JAVA_HOME", $scope)
    $env:ANDROID_HOME = [System.Environment]::GetEnvironmentVariable("ANDROID_HOME", $scope)
}

# ajoute la variable d'environnement si elle n'existe pas
function Add-Env([string]$name, [string]$value) {
    if (-not [Environment]::GetEnvironmentVariable("$name", $scope)) {
        $env:FOO = 'bar'
        [Environment]::SetEnvironmentVariable($name, $value, $scope)
        Write-Host '    ✔️ '$value' dans '$name'.'  -ForegroundColor Green
    }
    else {
        $existing = [Environment]::GetEnvironmentVariable("$name", $scope)
        Write-Host '    ✔️ '$name' existe déjà et vaut '$existing'.'  -ForegroundColor Red
    }
    Invoke-Env-Reload
}

# ajoute a la suite de la variable (en général path) qui contient une liste
function Append-Env([string]$name, [string]$value) {
    Write-Host "Ajout de $value a $name"
    if (-Not (Get-Env-Contains $name $value) ) {
        Write-Host '    👍 Ajout de'$value' à'$name'.' -ForegroundColor Blue
        $new_value = [Environment]::GetEnvironmentVariable("$name", $scope)
        if (-Not ($new_value -eq $null)) {
            $new_value += [IO.Path]::PathSeparator
        }
        $new_value += $value + ";"
        Write-Host "nouvelle valeur $new_value"
        [Environment]::SetEnvironmentVariable( "$name", $new_value, $scope )
        if (Get-Env-Contains $name $new_value) {
            Invoke-Env-Reload
            Write-Host '    ✔️  '$value' ajouté à '$name'.'  -ForegroundColor Green
        }
        else {
            Write-Host '    ❌ '$value' n''a pas été ajouté à '$name'.' -ForegroundColor Red
            Invoke-Env-Reload
            exit
        }
    }
    else {
        Write-Host '    ✔️ '$value' déjà ajouté à '$name'.'  -ForegroundColor Green
        Invoke-Env-Reload
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
    Invoke-Copy $Name ${env:scripty.cachePath}\$ZipName ${env:scripty.localTempPath}\$ZipName
    Invoke-Unzip $Name ${env:scripty.localTempPath}\$ZipName $InstallLocation
}

function Invoke-Copy() {
    Param(
        [parameter(Mandatory = $true)]
        [String]
        $Name,
        [parameter(Mandatory = $true)]
        [String]
        $Source,
        [parameter(Mandatory = $true)]
        [String]
        $Destination
    )
    Write-Host '    👍 Copie de'$Name' débuté.' -ForegroundColor Blue
    
    Copy-Item  $Source -Destination $Destination

    if(Test-Path $Destination) {
        Write-Host '    ✔️ '$Name' copié.' -ForegroundColor Green
    } else {
        Write-Host '    ❌ '$Name' n''a pas pu être copié.' -ForegroundColor Red
    }
}

function Invoke-Unzip() {
    Param(
        [parameter(Mandatory = $true)]
        [String]
        $Name,
        [parameter(Mandatory = $true)]
        [String]
        $Source,
        [parameter(Mandatory = $true)]
        [String]
        $Destination
    )
    Write-Host '    👍 Extraction de'$Name' débuté.' -ForegroundColor Blue

    if (-Not ( Test-Path ${env:ProgramFiles}\7-Zip\7z.exe)) {
        # pas de 7zip, c'Est plus lent
        Expand-Archive "${env:scripty.localTempPath}$ZipName" -DestinationPath $Destination
    }
    else {
        if (${env:scripty.debug} -eq $true) {
            & ${env:ProgramFiles}\7-Zip\7z.exe x "$Source" "-o$($Destination)" -y
        }
        else {
            & ${env:ProgramFiles}\7-Zip\7z.exe x "$Source" "-o$($Destination)" -y -bso0 -bsp0
        }
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
        $ZipName,
        [parameter(Mandatory = $true)]
        [bool]
        $ForceRedownload
    )
    if ( -Not ( Test-Path ${env:scripty.cachePath}\$ZipName.zip) -or $ForceRedownload) {
        Write-Host '    👍 Téléchargement de'$Name' débuté.' -ForegroundColor Blue

        Set-Location ${env:scripty.cachePath}
        Start-BitsTransfer -Source $Url -Destination "${env:scripty.cachePath}\$ZipName.zip" 

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

function replaceInFile([string] $filePath, [string] $toReplace, [string] $replacement) {
    # Read the file content using the Get-Content
    $filecontent = Get-Content -Path $filePath -Raw
    $modifiedContent = $filecontent.Replace($toReplace, $replacement)
    # Save the replace line in a file  
    Set-Content -Path $filePath -Value $modifiedContent
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

    if (${env:scripty.debug} -eq $true) {
        & ${env:ProgramFiles}\7-Zip\7z.exe a $SrcDir $SrcDir -y
    }
    else {
        & ${env:ProgramFiles}\7-Zip\7z.exe a $SrcDir $SrcDir -y -bso0 -bsp0
    }

    $parent = Split-Path -Path $SrcDir -Parent
    $dirName = Split-Path -Path $SrcDir -Leaf

    if(Test-Path $parent\$dirname.7z) {
        Write-Host '    ✔️ '$Name' compressé.' -ForegroundColor Green
    } else {
        Write-Host '    ❌ '$Name' n''a pas pu être compressé.' -ForegroundColor Red
    }
}