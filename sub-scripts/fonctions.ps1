
$scope = "User"
$debug = $false
#$scope = "Machine"

$sevenZipPath = "${env:ProgramFiles}\7-Zip\7z.exe"

function Check-Or-Install-Java() {
    #if (Test-CommandExists "javac") {
    # Write-Host "On a un JDK ici ${env:JAVA_HOME}"
    #} else {
    # nécessite Java
    Write-Host "JDK non installé ..."
    Invoke-Download "Corretto Java Dev Kit" $CORRETTO_URL "jdk" $false
    Invoke-Install "JDK" "$HOME\jdk" "jdk.zip"
    $jdkVersion = (Get-ChildItem $HOME\jdk | Select-Object -First 1).Name
    Add-Env "JAVA_HOME" "$HOME\jdk\$jdkVersion"
    Append-Env "Path" "$HOME\jdk\$jdkVersion\bin"
    #}
}

Function Start-Script() {
    Param ($script)
    # TODO if debug, we should add noexit option and not minimize
    If ($debug -eq $true) {
        Start-Process powershell -ArgumentList "-noexit","$script"

    }
    Else {
        Start-Process -WindowStyle Minimized  powershell -argument "$script"
    }
    #Start-Process powershell -argument "${env:scripty.scriptPath}\flutter-installe.ps1"
}

    
 Function Wait-Until-File-Exists
 {
    Param ($file)
    While (!(Test-Path $file  -ErrorAction SilentlyContinue))
    {
        Write-Host '   Attend que le fichier '$file' existe'
        Start-Sleep -s 2
    }
    Write-Host '   Trouve que le fichier '$file' existe'
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
    #Write-Host "looking for $value in $name"
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
        #Write-Host "nouvelle valeur $new_value"
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
    #Invoke-Copy $Name ${env:scripty.cachePath}\$ZipName ${env:scripty.localTempPath}\$ZipName
    $tempPath = "${env:scripty.localTempPath}$ZipName"
    if(-Not (Test-Path $tempPath)  ) {
        Write-Host '   Fichier manquant pour Dézippage   '$tempPath -ForegroundColor Red
    } else {
        Write-Host '    '$tempPath' va etre copié dans '$InstallLocation -ForegroundColor Green
        Invoke-Unzip $Name $tempPath $InstallLocation
    }
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


    if(Test-Path $Destination) {
        Write-Host '    ✔️ '$Name' deja la.' -ForegroundColor Green
    } else {
        Write-Host '    ❌ '$Name' va etre copié.' -ForegroundColor Red
        Write-Host '    👍 Copie de'$Name' débuté.' -ForegroundColor Blue
        Copy-Item  $Source -Destination $Destination
    }
}

function hasCache() {
    return ${env:scripty.cachePath} -ne ${env:scripty.localTempPath}
}

function Local7Zip(){
    if (-Not ( Test-Path ${env:ProgramFiles}\7-Zip\7z.exe)) {
        Write-Host "Je ne trouve pas de 7zip sur l'ordi, je le télécharge dans temp"
        Invoke-WebRequest 'https://www.7-zip.org/a/7z2301-x64.exe' -OutFile "${env:scripty.localTempPath}\7z.exe"
        $sevenZipPath = "${env:scripty.localTempPath}\7z.exe"
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

    if (-Not ( Test-Path $sevenZipPath)) {
        # pas de 7zip, c'Est plus lent
        # TODO install 7 zip locally if not present
        #Invoke-WebRequest 'https://www.7-zip.org/a/7z2301-x64.exe' -OutFile "${env:scripty.localTempPath}\7z.exe"

        #& ${env:scripty.localTempPath}\7z.exe x "$Source" "-o$($Destination)" -y
        Expand-Archive "${env:scripty.localTempPath}$ZipName" -DestinationPath $Destination
    }
    else {
        if (${env:scripty.debug} -eq $true) {
            & $sevenZipPath x "$Source" "-o$($Destination)" -y
        }
        else {
            & $sevenZipPath x "$Source" "-o$($Destination)" -y -bso0 -bsp0
        }
    }
    $tagFile = "$HOME\temp\fini$ZipName.txt"
    Out-File -FilePath $tagFile
    #New-Item -Name  -ItemType File
    Write-Host '    > Extraction terminée:'$tagFile -ForegroundColor Blue

}

# Source : https://stackoverflow.com/a/9701907
function Add-Shortcut([string]$source_exe, [string]$name) {
    $WshShell = New-Object -ComObject WScript.Shell
    $Shortcut = $WshShell.CreateShortcut("$HOME\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\$name.lnk")
    $Shortcut.TargetPath = $source_exe
    $Shortcut.Save()
}

function Invoke-CopyFromCache-Or-Download {
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
    $cacheLocation = "${env:scripty.cachePath}\$ZipName"
    $cacheLocation = "${env:scripty.cachePath}\$ZipName"
    
    if ( -Not ( Test-Path $cacheLocation) -or $ForceRedownload) {
        Write-Host '    👍 Téléchargement de'$Name' débuté.' -ForegroundColor Blue
        Set-Location ${env:scripty.cachePath}
        $tempLocation = "${env:scripty.localTempPath}$ZipName"
        Write-Host 'De '$Url' vers '$tempLocation
        $ProgressPreference = 'Continue'
        $done = $false
        # TODO strange bug when doing it with android studio --- test with regular Invoke-Download?
        Start-BitsTransfer -Source $Url -Destination $tempLocation
        $ProgressPreference = 'Continue'
                
        if (Test-Path $tempLocation ) {
            Write-Host '    ✔️ '$Name' téléchargé.'$tempLocation -ForegroundColor Green
            Copy-Item  $tempLocation -Destination $cacheLocation
        }
        else {
            Set-Location $HOME
            Write-Host '    ❌ '$Name' n''a pas pu être téléchargé.' -ForegroundColor Red
            # TODO throw an exception
            exit
        }
    }
    else {
        Write-Host '    ✔️ '$Name' est déjà présent dans '$cacheLocation'.' -ForegroundColor Green
        $destination = "${env:scripty.localTempPath}$ZipName"
        #Copy it where it should
        Invoke-Copy $Name $cacheLocation $destination
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

# TODO cannot remove existing flutter PATH as it is defined in the Machine part of the PATH
function Remove-Env([string]$name, [string]$value) {
    $path = [System.Environment]::GetEnvironmentVariable(
            "$name",
            'User'
    )
    # Remove unwanted elements
    $path = ($path.Split(';') | Where-Object { $_ -ne '$value' }) -join ';'
    Write-Host $path
    # Set it
    [System.Environment]::SetEnvironmentVariable(
            "$name",
            $path,
            'User'
    )
}
