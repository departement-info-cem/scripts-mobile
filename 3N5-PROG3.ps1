if ($args[0] -eq "H4X0R_M0D") {
    for (($i = 0); $i -lt 7; $i++) {
        Start-Process powershell -argument ".\sub-scripts\cmatrix.ps1"
    }
}

Start-Process powershell -argument ".\sub-scripts\android-sdk.ps1"
Start-Process powershell -argument ".\sub-scripts\android-studio.ps1"
Start-Process powershell -argument ".\sub-scripts\idea.ps1"