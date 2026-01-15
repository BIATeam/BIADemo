$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
Import-Module -Force $scriptPath/Library-BIA-BIATemplate.psm1

$biaDemoProjectPath = "$scriptPath\.."
$biaToolKitProjectPath = "$scriptPath\..\..\BIAToolKit"
if (!(Test-Path $biaToolKitProjectPath)) {
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
    if ($lines -and $lines.Length -gt 0) {
      foreach ($line in $lines) {
        DeleteLine -start $line -end $line -file $file
      }
    }
  }
}

function Remove-PackageDependency {
  param (
    [string]$packageJsonPath,
    [string]$packageName
  )
  $content = Get-Content -Path $packageJsonPath -Raw
  $json = $content | ConvertFrom-Json
  
  if ($json.dependencies -and $json.dependencies.PSObject.Properties.Name -contains $packageName) {
    $json.dependencies.PSObject.Properties.Remove($packageName)
    $json | ConvertTo-Json -Depth 10 | Set-Content -Path $packageJsonPath
    return $true
  }
  return $false
}

function Add-PackageDependency {
  param (
    [string]$packageJsonPath,
    [string]$packageName,
    [string]$version
  )
  $content = Get-Content -Path $packageJsonPath -Raw
  $json = $content | ConvertFrom-Json
  
  if ($json.dependencies -and -not ($json.dependencies.PSObject.Properties.Name -contains $packageName)) {
    $json.dependencies | Add-Member -NotePropertyName $packageName -NotePropertyValue $version
    $json | ConvertTo-Json -Depth 10 | Set-Content -Path $packageJsonPath
    return $true
  }
  return $false
}

$biaDemoVersion = Get-BiaDemoVersion
Write-Host "Archiving BIADemo v.$biademoVersion into BIAToolkit" -ForegroundColor Yellow

$biaToolKitVersionTargetPath = $biaToolKitProjectPath + "\BIADemoVersions\$biademoVersion"
if (Test-Path $biaToolKitVersionTargetPath) {
  Write-Host "Delete existing version content in BIAToolKit folder..." -ForegroundColor Blue
  Remove-Item $biaToolKitVersionTargetPath -Force -Recurse
}

Write-Host "Apply bi-ng angular references to BIADemo..." -ForegroundColor Blue
Set-Location ($biaDemoProjectPath + "\Angular")
.\switch-to-bia-ng.ps1
Set-Location $scriptPath

Write-Host "Copy BIADemo to BIAToolKit..." -ForegroundColor Blue
robocopy "$biaDemoProjectPath\DotNet" "$biaToolKitVersionTargetPath\DotNet" /E /XD BIAPackage .vs .vscode bin obj | Out-Null
robocopy "$biaDemoProjectPath\Angular" "$biaToolKitVersionTargetPath\Angular" /E /XD dist node_modules .angular .dart_tool .vscode | Out-Null

Write-Host "Remove code example..." -ForegroundColor Blue
Remove-CodeExample -path $biaToolKitVersionTargetPath

Write-Host "Remove Except BIADemo lines..." -ForegroundColor Blue
Remove-Line "// Except BIADemo" -Path $biaToolKitVersionTargetPath

Write-Host "Remove BIADemo Only lines..." -ForegroundColor Blue
Remove-Line "// BIADemo only" -Path $biaToolKitVersionTargetPath

Write-Host "Remove @bia-team/bia-ng from package.json..." -ForegroundColor Blue
$packageJsonPath = "$biaToolKitVersionTargetPath\Angular\package.json"
$biaNgRemoved = Remove-PackageDependency -packageJsonPath $packageJsonPath -packageName "@bia-team/bia-ng"
if ($biaNgRemoved) {
  Write-Host "@bia-team/bia-ng removed from package.json" -ForegroundColor DarkGray
}

Write-Host "Archiving..." -ForegroundColor Blue
$biaDemoArchivePath = $biaToolKitVersionTargetPath + "\..\BIADemo_$biaDemoVersion.zip"
$biaDemoExtarctedArchivePath = $biaToolKitVersionTargetPath + "\..\BIADemo_$biaDemoVersion"
Write-Host "Target archive : $biaDemoArchivePath" -ForegroundColor DarkGray
if (Test-Path $biaDemoArchivePath) {
  Write-Host "Delete existing target archive..." -ForegroundColor Blue
  Remove-Item $biaDemoArchivePath -Force
}
if (Test-Path $biaDemoExtarctedArchivePath) {
  Write-Host "Delete existing target archive extracted content..." -ForegroundColor Blue
  Remove-Item $biaDemoExtarctedArchivePath -Force -Recurse
}

Write-Host "Creating archive..." -ForegroundColor Blue
Compress-Archive -Path "$biaToolKitVersionTargetPath\*" -DestinationPath $biaDemoArchivePath
Write-Host "Delete archived version content..." -ForegroundColor Blue
Remove-Item $biaToolKitVersionTargetPath -Force -Recurse

Write-Host "Restore @bia-team/bia-ng in package.json..." -ForegroundColor Blue
$originalPackageJsonPath = "$biaDemoProjectPath\Angular\package.json"
$originalContent = Get-Content -Path $originalPackageJsonPath -Raw
$originalJson = $originalContent | ConvertFrom-Json
if ($originalJson.dependencies.PSObject.Properties.Name -contains "@bia-team/bia-ng") {
  $biaNgVersion = $originalJson.dependencies."@bia-team/bia-ng"
  Add-PackageDependency -packageJsonPath $packageJsonPath -packageName "@bia-team/bia-ng" -version $biaNgVersion
  Write-Host "@bia-team/bia-ng restored in archive package.json" -ForegroundColor DarkGray
}

Write-Host "Restore direct angular references to BIADemo..." -ForegroundColor Blue
Set-Location ($biaDemoProjectPath + "\Angular")
.\switch-to-direct-references.ps1
Set-Location $scriptPath

Write-Host "Finished !" -ForegroundColor Yellow
