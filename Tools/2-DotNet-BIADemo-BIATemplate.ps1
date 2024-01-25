# $oldName = Read-Host "old project name ?"
$oldName = 'BIADemo'
# $newName = Read-Host "new project name ?"
$newName = 'BIATemplate'

$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
$newPath = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath("$scriptPath\..\..\$newName\DotNet")
$oldPath = Resolve-Path -Path "$scriptPath\..\..\$oldName\DotNet"

Write-Host "old name: " $oldName
Write-Host "new name: " $newName

###### ###### ###### Functions ###### ###### ######
# Returns all line numbers containing the value passed as a parameter.
function GetLineNumber($pattern, $file) {
  $LineNumber = Select-String -Path $file -Pattern $pattern | Select-Object -ExpandProperty LineNumber
  return $LineNumber
}

# Deletes a set of lines whose number is between $start and $end.
function DeleteLine($start, $end, $file) {
  $i = 0
  $start--
  $end--
  $fileRel = Resolve-Path -Path "$file" -Relative
  Write-Verbose "Delete lines $start to $end in file $fileRel" -Verbose
  (Get-Content $file) | Where-Object {
	(($i -ne $start - 1 -or $_.Trim() -ne '') -and 
    ($i -lt $start -or $i -gt $end))
    $i++
  } | set-content $file
}

# Deletes lines between // Begin BIADemo and // End BIADemo 
function RemoveCodeExample {
  Get-ChildItem -File -Recurse -include *.csproj, *.cs, *.sln, *.json | Where-Object { $_.FullName -NotLike "*/bin/*" -and $_.FullName -NotLike "*/obj/*" } | ForEach-Object { 
    $lineBegin = @()
    $file = $_.FullName
  
    $searchWord = 'Begin BIADemo'
    $starts = GetLineNumber -pattern $searchWord -file $file
    $lineBegin += $starts
  
    $searchWord = 'End BIADemo'
    $ends = GetLineNumber -pattern $searchWord -file $file
    $lineBegin += $ends
  
    if ($lineBegin -and $lineBegin.Length -gt 0) {
      $lineBegin = $lineBegin | Sort-Object
      for ($i = $lineBegin.Length - 1; $i -gt 0; $i = $i - 2) {
        $start = [int]$lineBegin[$i - 1]
        $end = [int]$lineBegin[$i]
        DeleteLine -start $start -end $end -file $file
      }
    }
  }
}

# Deletes comment // Except BIADemo 
function RemoveCommentExceptBIADemo {
  ReplaceProjectName -oldName "// Except BIADemo " -newName ""
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
  $isEmpty = $currentChildren -eq $null
  if ($isEmpty) {
    $fileRel = Resolve-Path -Path "$Path" -Relative
    Write-Verbose "Removing empty folder '${fileRel}'." -Verbose
    Remove-Item -Force -LiteralPath $Path
  }
}

function RenameFile {
  param (
    [string]$oldName,
    [string]$newName
  )
  Get-ChildItem -File -Recurse | ForEach-Object { if ($_.Name -ne $_.Name.replace($oldName, $newName)) { Rename-Item -Path $_.PSPath -NewName $_.Name.replace($oldName, $newName) } }
}

function RenameFolder {
  param (
    [string]$oldName,
    [string]$newName
  )
  Get-ChildItem -Directory -Recurse | ForEach-Object { if ($_.Name -ne $_.Name.replace($oldName, $newName)) { Rename-Item -Path $_.PSPath -NewName $_.Name.replace($oldName, $newName) } }
}

function RemoveItemFolder {
  param (
    [string]$path
  )
  if (Test-Path $path) {
    $fileRel = Resolve-Path -Path "$path" -Relative
    Write-Verbose "Delete $fileRel" -Verbose
    Remove-Item $path -Recurse -Force -Confirm:$false
  }
  else {
    Write-Host "Error $path not found"
  }
}

function ReplaceProjectName {
  param (
    [string]$oldName,
    [string]$newName
  )
  Get-ChildItem -File -Recurse -include *.csproj, *.cs, *.sln, *.json, *.config | Where-Object { $_.FullName -NotLike "*/bin/*" -and $_.FullName -NotLike "*/obj/*" } | ForEach-Object { 
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

function ReplaceInScript {
  param (
    [string]$oldName,
    [string]$newName
  )
  Get-ChildItem -File -Recurse -include *.ps1 | Where-Object { $_.FullName -NotLike "*/bin/*" -and $_.FullName -NotLike "*/obj/*" } | ForEach-Object { 
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

function RemoveFolder {
  param (
    [string]$path
  )
  if (Test-Path $path) {
    Write-Host "delete " $path " folder"
    Remove-Item $path -Recurse -Force -Confirm:$false
  }
}

function CopyModel {
  param (
    [string]$modelName,
    [string]$folderpath,
    [string]$fileName
  )
  $destinationFolder = '.\docs\' + $modelName + '\' + $folderpath
  If (!(Test-Path -path $destinationFolder)) { New-Item -ItemType Directory -Path $destinationFolder }
  $destinationFile = $destinationFolder + '\' + $fileName
  $sourceFile = '.\' + $folderpath + '\' + $fileName
  Copy-Item -path $sourceFile -Destination $destinationFile

  $searchWord = '// BIADemo only'
  $starts = GetLineNumber -pattern $searchWord -file $destinationFile
  if ($starts -eq 1) {
    DeleteLine -start 1 -end 1 -file $destinationFile
  }
}

function ExtractPartial {
  param (
    [string]$modelName,
    [string]$folderpath,
    [string]$fileName
  )
  $destinationFolder = '.\docs\' + $modelName + '\' + $folderpath
  If (!(Test-Path -path $destinationFolder)) { New-Item -ItemType Directory -Path $destinationFolder }
  $destinationFile = $destinationFolder + '\' + $fileName + ".partial"
  $sourceFile = '.\' + $folderpath + '\' + $fileName
  Copy-Item -path $sourceFile -Destination $destinationFile

  $searchBegin = 'BIAToolKit - Begin Partial'
  $searchEnd = 'BIAToolKit - End Partial'
   
  $start = GetLineNumber -pattern $searchBegin -file $destinationFile
  $end = GetLineNumber -pattern $searchEnd -file $destinationFile
  $lineNumber = (Get-Content $sourceFile).Length

  DeleteLine -start ($end+1) -end $lineNumber -file $destinationFile 
  DeleteLine -start 1 -end ($start-1) -file $destinationFile
}

###### ###### ###### Start process ###### ###### ######
RemoveFolder -path $newPath

Write-Host "Copy from $oldPath to $newPath"
Copy-Item -Path $oldPath -Destination $newPath -Recurse -Force

$biaPackage = $newPath + "\BIAPackage"
RemoveFolder -path $biaPackage

Set-Location -Path $newPath

Write-Host "Zip plane"

# Read Json settings
$myJson = Get-Content "$oldPath\..\BIAToolKit.json" -Raw | ConvertFrom-Json 

# Get settings for "WebApi" type
$settings = [System.Linq.Enumerable]::FirstOrDefault($myJson, [Func[object,bool]]{ param($x) $x.Type -eq "WebApi" })
$feature = $settings.Feature

# Copy files/folders
ForEach($include in $settings.Contains.Include)
{
    if($include.EndsWith('*'))
    {
        # Directory found
        $dirPath = $include.Replace('*', '')
        Write-Host "Copy-Item -Path "$oldPath\$dirPath" -Destination "$newPath\docs\$feature\$dirPath" -Recurse -Force"
        Copy-Item -Path "$oldPath\$dirPath" -Destination "$newPath\docs\$feature\$dirPath" -Recurse -Force
    } 
    else
    {       
        # File found
        $index = $include.lastIndexOf('\')
        $fileName = $include.Substring($index + 1)
        $filePath = $include.Substring(0, $index)
        Write-Host "CopyModel $feature $filePath $fileName"
        CopyModel $feature $filePath $fileName
    }
}

# Remove files/folders
ForEach($exclude in $settings.Contains.Exclude)
{
    if($exclude.EndsWith('*'))
    {
        # Directory found
        $dirPath = $exclude.Replace('*', '')
        Write-Host "RemoveItemFolder -path '$newPath\docs\$feature\$dirPath'"
        RemoveItemFolder -path "$newPath\docs\$feature\$dirPath"
    } 
    else
    {       
        # File found
        Write-Host "Remove-Item "$newPath\docs\$feature\$exclude" -Force"
        Remove-Item "$newPath\docs\$feature\$exclude" -Force
    }
}

# Extract partial
ForEach($partial in $settings.Partial)
{
    $index = $partial.lastIndexOf('\')
    $fileName = $partial.Substring($index + 1)
    $filePath = $partial.Substring(0, $index)

    Write-Host "ExtractPartial $feature $filePath $fileName"
    ExtractPartial $feature $filePath $fileName
}

# Create Zip
$zipName = $settings.ZipName
compress-archive -path ".\docs\$feature\*" -destinationpath ".\docs\$zipName" -compressionlevel optimal
Remove-Item ".\docs\$feature" -Recurse -Force -Confirm:$false

Write-Host "Remove .vs"
RemoveItemFolder -path '.vs'
Write-Host "Remove *\bin"
RemoveItemFolder -path '*\bin'
Write-Host "Remove *\obj"
RemoveItemFolder -path '*\obj'


Write-Host "Remove Migrations and keep .editconfig"
Remove-Item '*.BIADemo.Infrastructure.Data\Migrations\*.cs' -Recurse -Force -Confirm:$false

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
# compress-archive -path $targetDir -destinationpath '..\BIADemo\Docs\Templates\VX.Y.Z\BIA.DotNetTemplate.X.Y.Z.zip' -compressionlevel optimal -Force
# RemoveItemFolder -path 'Tmp'

write-host "finish"
pause
