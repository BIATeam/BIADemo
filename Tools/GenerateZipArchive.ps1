Param(
    [object]$myJson,
    [string]$type,
    [bool]$searchFirst = $true
)

function CopyFileFolder() {
  param (
    [string]$include, 
    [string]$feature
  )

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

function RemoveFileFolder() {
  param (
    [string]$exclude, 
    [string]$feature
        #[string]$newPath
  )

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

function ExtractPartialFile(){
  param (
    [string]$partial,
    [string]$feature
  )

  $index = $partial.lastIndexOf('\')
  $fileName = $partial.Substring($index + 1)
  $filePath = $partial.Substring(0, $index)
  Write-Host "ExtractPartial $feature $filePath $fileName"
  ExtractPartial $feature $filePath $fileName
}

function ExtractPartial {
  param (
    [string]$modelName,
    [string]$folderpath,
    [string]$fileName
  )

  $destinationFolder = ".\docs\$modelName\$folderpath"
  If (!(Test-Path -path $destinationFolder)) { New-Item -ItemType Directory -Path $destinationFolder }

  $destinationFile = "$destinationFolder\$fileName.partial"
  $sourceFile = ".\$folderpath\$fileName"
  Copy-Item -path $sourceFile -Destination $destinationFile

  $searchBegin = 'BIAToolKit - Begin Partial'
  $searchEnd = 'BIAToolKit - End Partial'
   
  $start = GetLineNumber -pattern $searchBegin -file $destinationFile
  $end = GetLineNumber -pattern $searchEnd -file $destinationFile
  $lineNumber = (Get-Content $sourceFile).Length

  # Delete lines after marker end
  DeleteLine -start ($end+1) -end $lineNumber -file $destinationFile 
  # Delete lines before marker begin
  DeleteLine -start 1 -end ($start-1) -file $destinationFile
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

# Get settings for type
if($searchFirst -eq $true) {
    $settings = [System.Linq.Enumerable]::FirstOrDefault($myJson, [Func[object,bool]]{ param($x) $x.Type -eq $type })
}
else{
    $settings = [System.Linq.Enumerable]::LastOrDefault($myJson, [Func[object,bool]]{ param($x) $x.Type -eq $type })
}

$feature = $settings.Feature

# Copy files/folders
ForEach($include in $settings.Contains.Include)
{
    CopyFileFolder -include $include -feature $feature
}

# Remove files/folders
ForEach($exclude in $settings.Contains.Exclude)
{
    RemoveFileFolder -exclude $exclude -feature $feature
}

# Extract partial
ForEach($partial in $settings.Partial)
{
    ExtractPartialFile -partial $partial -feature $feature
}

# Create Zip
$zipName = $settings.ZipName
Write-Host "Zip $feature" 
compress-archive -path ".\docs\$feature\*" -destinationpath ".\docs\$zipName" -compressionlevel optimal

# Delete temp folders
RemoveFolder -path ".\docs\$feature" 
