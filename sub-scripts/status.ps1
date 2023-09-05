Function Test-CommandExists
{
 Param ($command)
 $oldPreference = $ErrorActionPreference
 $ErrorActionPreference = ‘stop’
 try {if(Get-Command $command){return $true}}
 Catch {return $false}
 Finally {$ErrorActionPreference=$oldPreference}
} #end function test-CommandExists

Do {

  $android-studio = Test-CommandExists('flutter')
  $java = Test-CommandExists('java')
  $flutter = Test-CommandExists('flutter')

  Write-Host 'status des scripts'
  
  $val++
  Write-Host $val
  Start-Sleep -s 10  # attend 10 seecondes avant de regarder

} (While (weather –eq ‘sunny’)
