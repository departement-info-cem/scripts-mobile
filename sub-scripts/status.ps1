$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8


. "$PSScriptRoot\urls-et-versions.ps1"
. "$PSScriptRoot\fonctions.ps1"

Invoke-Env-Reload

Do {

  $androidstudio = (Test-Path '$HOME\android-studio\bin')
  $java = Test-CommandExists('java')
  $flutter = Test-CommandExists('flutter')

  Write-Host 'status des scripts'
  Write-Host ' android studio'$androidstudio
  Write-Host ' flutter'$flutter
  Write-Host ' Java'$java
  
  Start-Sleep -s 5  # attend 10 seecondes avant de regarder

} While ($true)
