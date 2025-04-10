$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
Import-Module -Force $scriptPath/Library-BIA-BIATemplate.psm1

function Get-BiaDemoVersion() {
  $constantFilePath = "$scriptPath\..\DotNet\TheBIADevCompany.BIADemo.Crosscutting.Common\Constants.cs"
  $content = Get-Content $constantFilePath
  $line = $content | Where-Object { $_ -match 'public const string FrameworkVersion =' }
  if ($line -match '"([^"]+)"') {
      $value = $matches[1]
      return $value
  }
}

$biaDemoVersion = Get-BiaDemoVersion
Write-Host "Archiving BIADemo v.$biademoVersion into BIAToolkit" -ForegroundColor Yellow

$biaToolKitProjectPath = "C:\sources\Github\BIAToolKit"
$biaToolKitTargetPath = $biaToolKitProjectPath + "\BIADemoVersions"
$biaDemoArchivePath = $biaToolKitTargetPath + "\BIADemo_$biaDemoVersion.zip"
Write-Host "Target archive : $biaDemoArchivePath" -ForegroundColor DarkGray