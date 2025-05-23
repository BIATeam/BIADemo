$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
Import-Module -Force $scriptPath/Library-BIA-BIATemplate.psm1

$biaDemoProjectPath = "$scriptPath\.."
$biaToolKitProjectPath = "$scriptPath\..\..\BIAToolKit"
if(!(Test-Path $biaToolKitProjectPath)) {
  Write-Error "Unable to find BIAToolKit sources at $biaToolKitProjectPath"
  exit
}

function Get-BiaDemoVersion() {
  $constantFilePath = "$biaDemoProjectPath\DotNet\TheBIADevCompany.BIADemo.Crosscutting.Common\Constants.cs"
  $content = Get-Content $constantFilePath
  $line = $content | Where-Object { $_ -match 'public const string FrameworkVersion =' }
  if ($line -match '"([^"]+)"') {
      $value = $matches[1]
      return $value
  }
}

function Remove-CodeExample {
  param (
    [string]$path
  )
  Get-ChildItem -Path $path -File -Recurse | ForEach-Object { 
    RemoveCodeBetweenMarkers -file $_.FullName -marker "BIADemo"
  }
}

function Remove-Line {
  param (
    [string]$path,
    [string]$lineContent
  )
  Get-ChildItem -Path $path -File -Recurse | ForEach-Object { 
    $file = $_.FullName
    $lines = GetLineNumber -pattern $lineContent -file $file
    if($lines -and $lines.Length -gt 0) {
      foreach($line in $lines) {
        DeleteLine -start $line -end $line -file $file
      }
    }
  }
}

$biaDemoVersion = Get-BiaDemoVersion
Write-Host "Archiving BIADemo v.$biademoVersion into BIAToolkit" -ForegroundColor Yellow

$biaToolKitVersionTargetPath = $biaToolKitProjectPath + "\BIADemoVersions\$biademoVersion"
if(Test-Path $biaToolKitVersionTargetPath) {
  Remove-Item $biaToolKitVersionTargetPath -Force -Recurse
}

Write-Host "Copy BIADemo to BIAToolKit..." -ForegroundColor Blue
robocopy "$biaDemoProjectPath\DotNet" "$biaToolKitVersionTargetPath\DotNet" /E /XD BIAPackage .vs .vscode bin obj | Out-Null
robocopy "$biaDemoProjectPath\Angular" "$biaToolKitVersionTargetPath\Angular" /E /XD dist node_modules .angular .dart_tool .vscode | Out-Null

Write-Host "Remove code example..." -ForegroundColor Blue
Remove-CodeExample -path $biaToolKitVersionTargetPath

Write-Host "Remove Except BIADemo lines..." -ForegroundColor Blue
Remove-Line "// Except BIADemo" -Path $biaToolKitVersionTargetPath

Write-Host "Remove BIADemo Only lines..." -ForegroundColor Blue
Remove-Line "// BIADemo only" -Path $biaToolKitVersionTargetPath

Write-Host "Archiving..." -ForegroundColor Blue
$biaDemoArchivePath = $biaToolKitVersionTargetPath + "\..\BIADemo_$biaDemoVersion.zip"
$biaDemoExtarctedArchivePath = $biaToolKitVersionTargetPath + "\..\BIADemo_$biaDemoVersion"
Write-Host "Target archive : $biaDemoArchivePath" -ForegroundColor DarkGray
if(Test-Path $biaDemoArchivePath) {
  Remove-Item $biaDemoArchivePath -Force
}
if(Test-Path $biaDemoExtarctedArchivePath) {
  Remove-Item $biaDemoExtarctedArchivePath -Force -Recurse
}

Compress-Archive -Path "$biaToolKitVersionTargetPath\*" -DestinationPath $biaDemoArchivePath
Remove-Item $biaToolKitVersionTargetPath -Force -Recurse

Write-Host "Finished !" -ForegroundColor Yellow
