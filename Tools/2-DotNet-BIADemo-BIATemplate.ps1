$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
Import-Module -Force $scriptPath/Library-BIA-BIATemplate.psm1

# $oldName = Read-Host "old project name ?"
$oldName = 'BIADemo'
# $newName = Read-Host "new project name ?"
$newName = 'BIATemplate'

$jsonFileName = 'BIAToolKit.json'
$docsFolder = '.bia'

$newPath = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath("$scriptPath\..\..\$newName\DotNet")
$oldPath = Resolve-Path -Path "$scriptPath\..\..\$oldName\DotNet"

Write-Host "old name: " $oldName
Write-Host "new name: " $newName

###### ###### ###### Specific Functions ###### ###### ######
function ReplaceProjectName {
  param (
    [string]$oldName,
    [string]$newName
  )
  Get-ChildItem -File -Recurse -include *.csproj, *.cs, *.sln, *.json, *.config, *.sql | Where-Object { $_.FullName -NotLike "*/bin/*" -and $_.FullName -NotLike "*/obj/*" } | ForEach-Object { 
    $file = $_.FullName
    $oldContent = [System.IO.File]::ReadAllText($file);
    $newContent = $oldContent.Replace($oldName, $newName);
    if ($oldContent -ne $newContent) {
	  
      $fileRel = Resolve-Path -Path "$file" -Relative
      Write-Verbose "Replace in $fileRel" -Verbose
	  
      [System.IO.File]::WriteAllText($file, $newContent)
    }
  }
}

# Deletes comment // Except BIADemo 
function RemoveCommentExceptBIADemo {
  ReplaceProjectName -oldName "// Except BIADemo " -newName ""
}

# Deletes lines between : 
# // Begin BIADemo and // End BIADemo 
# // Begin BIAToolKit Generation Ignore and // End BIAToolKit Generation Ignore
function RemoveCodeExample {
  Get-ChildItem -File -Recurse -include *.csproj, *.cs, *.sln, *.json, *.sql | Where-Object { $_.FullName -NotLike "*/bin/*" -and $_.FullName -NotLike "*/obj/*" } | ForEach-Object { 
    RemoveCodeBetweenMarkers -file $_.FullName -marker "BIADemo"
    RemoveCodeBetweenMarkers -file $_.FullName -marker "BIAToolKit Generation Ignore"
  }
}

function RemoveBIADemoOnlyFiles {
  foreach ($childFile in Get-ChildItem -File -Recurse | Where-Object { Select-String "// BIADemo only" $_ -Quiet } ) { 
    $file = $childFile.FullName
    $fileRel = Resolve-Path -Path "$file" -Relative
    $searchWord = '// BIADemo only'
    $starts = GetLineNumber -pattern $searchWord -file $file
    if ($starts -eq 1) {
      Write-Verbose "Remove $fileRel" -Verbose
      Remove-Item -Force -LiteralPath $file
    }
  }
}

function RemoveEmptyFolder {
  param(
    $Path
  )
  foreach ($childDirectory in Get-ChildItem -Force -Path $Path -Directory -Exclude PublishProfiles, RepoContract) {
    RemoveEmptyFolder $childDirectory.FullName
  }
  $currentChildren = Get-ChildItem -Force -LiteralPath $Path
  $isEmpty = $null -eq $currentChildren
  if ($isEmpty) {
    $fileRel = Resolve-Path -Path "$Path" -Relative
    Write-Verbose "Removing empty folder '${fileRel}'." -Verbose
    Remove-Item -Force -LiteralPath $Path
  }
}

function CopyBiaFolder {
  param (
    $oldPath,
    $newPath
  )
  if ((Test-Path $oldPath) -and (Test-Path $newPath)) {
    $oldPathParent = Split-Path $oldPath
    $newPathParent = Split-Path $newPath
    $oldPathBia = "$oldPathParent\.bia"

    if (Test-Path $oldPathBia) {
      Copy-Item -Path $oldPathBia -Destination $newPathParent -Recurse -Force
    }
  }
}


###### ###### ###### Start process ###### ###### ######
RemoveFolder -path $newPath

Write-Host "Copy from $oldPath to $newPath"
Copy-Item -Path $oldPath -Destination $newPath -Recurse -Force

CopyBiaFolder -oldPath $oldPath -newPath $newPath

$biaPackage = $newPath + "\BIAPackage"
RemoveFolder -path $biaPackage

Set-Location -Path $newPath

Write-Host "Remove .vs"
RemoveItemFolder -path '.vs'
Write-Host "Remove *\bin"
RemoveItemFolder -path '*\bin'
Write-Host "Remove *\obj"
RemoveItemFolder -path '*\obj'


Write-Host "Remove Migrations and keep .editconfig"
Remove-Item '*.BIADemo.Infrastructure.Data\Migrations\*.cs' -Recurse -Force -Confirm:$false
Remove-Item '*.BIADemo.Infrastructure.Data\Migrations\*.sql' -Recurse -Force -Confirm:$false

Write-Host "Remove BIA demo only files"
RemoveBIADemoOnlyFiles

Write-Host "Remove Empty Folder"
RemoveEmptyFolder "."

Write-Host "Remove code example partial files"
RemoveCodeExample

Write-Host "Remove comment except BIADemo"
RemoveCommentExceptBIADemo

Write-Host "Replace project name"
ReplaceProjectName -oldName $oldName -newName $newName
ReplaceProjectName -oldName $oldName.ToLower() -newName $newName.ToLower()

Write-Host "Rename File"
RenameFile -oldName $oldName -newName $newName
RenameFile -oldName $oldName.ToLower() -newName $newName.ToLower()

Write-Host "Rename Folder"
RenameFolder -oldName $oldName -newName $newName
RenameFolder -oldName $oldName.ToLower() -newName $newName.ToLower()

Write-Host "Replace project name in script"
$oldScriptVar = '"' + $oldName + '"'
$newScriptVar = '"' + $newName + '"'
ReplaceInScript $oldScriptVar $newScriptVar

Set-Location -Path $scriptPath

# Write-Host "Prepare the zip."
# $scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
# $sourceDir = Resolve-Path -Path "$scriptPath\DotNet"
# $targetDir = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath("$scriptPath\Tmp\DotNet")

# Write-Host "   Delete old path."
# RemoveItemFolder -path $targetDir
# Write-Host "   Copy files."

# $filter = "*"
# $filterExclude = '\\bin\\|\\obj\\|appsettings..*.json|bianetconfig..*.json|csproj.user'
# $filterInclude = 'appsettings.Example.*.json|bianetconfig.Example.*.json'

# Write-Host "Copy form $sourceDir to $targetDir"
# Get-ChildItem -File $sourceDir -filter $filter -recurse | ?{($_.fullname -match $filterInclude) -or ($_.fullname -notmatch $filterExclude)}|`
# foreach{
# $targetFile = $targetDir + $_.FullName.SubString($sourceDir.Path.Length);

# #Write-Host "Copy file " $_.FullName " to $targetFile"
# New-Item -ItemType File -Path $targetFile -Force | Out-Null
# Copy-Item $_.FullName -destination $targetFile
# }


# Write-Host "Copy from .\BIATemplateFiles\"
# $sourceDir = Resolve-Path -Path "$scriptPath\BIATemplateFiles"
# Get-ChildItem -File $sourceDir -filter $filter -recurse |`
# foreach{
# $targetFile = $targetDir + $_.FullName.SubString($sourceDir.Path.Length);

# #Write-Host "Copy file " $_.FullName " to $targetFile"
# New-Item -ItemType File -Path $targetFile -Force | Out-Null
# Copy-Item $_.FullName -destination $targetFile
# }

# Write-Host "   Zip files."
# compress-archive -path $targetDir -destinationpath "..\BIADemo\$docsFolder\Templates\VX.Y.Z\BIA.DotNetTemplate.X.Y.Z.zip" -compressionlevel optimal -Force
# RemoveItemFolder -path 'Tmp'

write-host "finish"
pause
