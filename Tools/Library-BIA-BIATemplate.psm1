###### ###### ###### ###### ####### ###### ###### ###### ######
###### ###### ###### Librairy of functions ###### ###### ######
###### ###### ###### ###### ####### ###### ###### ###### ######
$partialMarkerBegin = 'BIAToolKit - Begin Partial'
$partialMarkerEnd = 'BIAToolKit - End Partial'
$docsFolder = '.bia'


function RemoveFolderContents {
  param (
    [string]$Path,
	$Exclude
  )
  if (Test-Path $Path) {
    Write-Host "delete " $Path " folder" 
    Get-ChildItem -Path $Path $extension -Exclude $Exclude | ForEach-Object { Remove-Item -Path $_.FullName -Recurse -Force -Confirm:$false }
  }
}

function ReplaceProjectNameRecurse {
  param (
    [string]$oldName,
    [string]$newName,
    $Path,
    $ExcludeDir
  )
  foreach ($childDirectory in Get-ChildItem -Force -Path $Path -Directory -Exclude $ExcludeDir) {
    ReplaceProjectNameRecurse -oldName $oldName -newName $newName -Path $childDirectory.FullName -Exclude $ExcludeDir
  }
  Get-ChildItem -LiteralPath $Path -File -Include *.csproj, *.cs, *.sln, *.json, *.config, *.ps1, *.ts, *.html, *.yml | ForEach-Object { 
    $oldContent = [System.IO.File]::ReadAllText($_.FullName);
    $newContent = $oldContent.Replace($oldName, $newName);
    if ($oldContent -ne $newContent) {
      Write-Host $_.FullName
      [System.IO.File]::WriteAllText($_.FullName, $newContent)
    }
  }
}

# Formats JSON in a nicer format than the built-in ConvertTo-Json does.
function Format-Json([Parameter(Mandatory, ValueFromPipeline)][String] $json) {
  $indent = 0;
  ($json -Split '\n' |
    % {
      if ($_ -match '[\}\]]') {
        # This line contains  ] or }, decrement the indentation level
        $indent--
      }
      $line = (' ' * $indent * 2) + $_.TrimStart().Replace(':  ', ': ')
      if ($_ -match '[\{\[]') {
        # This line contains [ or {, increment the indentation level
        $indent++
      }
      $line
  }) -Join "`n"
}

# Returns all line numbers containing the value passed as a parameter.
function GetLineNumber($pattern, $file) {
  $LineNumber = Select-String -Path $file -Pattern $pattern | Select-Object -ExpandProperty LineNumber
  return $LineNumber
}

# Deletes a set of lines whose number is between $start and $end.
function DeleteLine($start, $end, $file) {
  $fileRel = Resolve-Path -Path "$file" -Relative
  Write-Verbose "Delete lines $start to $end in file $fileRel" -Verbose
  $i = 0
  $start--
  $end--
  
  (Get-Content $file) | Where-Object {
	(($i -ne $start - 1 -or $_.Trim() -ne '') -and 
    ($i -lt $start -or $i -gt $end))
    $i++
  } | set-content $file
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

function RemoveCodeBetweenMarkers {
    param (
    [string]$file,
    [string]$marker
  )
    $lineBegin = @()
  
    $searchWord = "Begin $marker"
    $starts = GetLineNumber -pattern $searchWord -file $file
    $lineBegin += $starts
  
    $searchWord = "End $marker"
    $ends = GetLineNumber -pattern $searchWord -file $file
    $lineBegin += $ends
  
    if ($lineBegin -and $lineBegin.Length -gt 0) {
        Write-Host "Remove code between markers $marker :"
        $lineBegin = $lineBegin | Sort-Object
        for ($i = $lineBegin.Length - 1; $i -gt 0; $i = $i - 2) {
        $start = [int]$lineBegin[$i - 1]
        $end = [int]$lineBegin[$i]
        DeleteLine -start $start -end $end -file $file
        }
    }
}

function CopyModel {
  param (
    [string]$modelName,
    [string]$folderpath,
    [string]$fileName
  )
  $destinationFolder = ".\$docsFolder\$modelName\$folderpath"
  If (!(Test-Path -path $destinationFolder)) { New-Item -ItemType Directory -Path $destinationFolder }
  $destinationFile = $destinationFolder + '\' + $fileName
  $sourceFile = '.\' + $folderpath + '\' + $fileName
  Copy-Item -path $sourceFile -Destination $destinationFile

  RemoveCodeBetweenMarkers $destinationFile "Partial"
  RemoveCodeBetweenMarkers $destinationFile "BIADemo"

  $searchWord = '// BIADemo only'
  $starts = GetLineNumber -pattern $searchWord -file $destinationFile
  if ($starts -eq 1) {
    Write-Host "Remove '// BIADemo only' line :"
    DeleteLine -start 1 -end 1 -file $destinationFile
  }
}

function CopyFileFolder() {
  param (
    [string]$include, 
    [string]$feature,
    [string]$oldPath,
    [string]$newPath
  )

  if($include.EndsWith('*'))
  {
    # Directory found
    $dirPath = $include.Replace('*', '')
    Write-Host "Copy-Item -Path "$oldPath\$dirPath" -Destination "$newPath\$docsFolder\$feature\$dirPath" -Recurse -Force"
    Copy-Item -Path "$oldPath\$dirPath" -Destination "$newPath\$docsFolder\$feature\$dirPath" -Recurse -Force
    Get-ChildItem -File -Recurse -Path "$newPath\$docsFolder\$feature\$dirPath" | Where-Object { $_.FullName -NotLike "*/bin/*" -and $_.FullName -NotLike "*/obj/*" -and $_.FullName -NotLike "*.ps1" -and $_.FullName -NotLike "*.md" } | ForEach-Object {
        RemoveCodeBetweenMarkers $_.FullName "Partial"
        RemoveCodeBetweenMarkers $_.FullName "BIADemo"
    }
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
    Write-Host "RemoveItemFolder -path $newPath\$docsFolder\$feature\$dirPath"
    RemoveItemFolder -path "$newPath\$docsFolder\$feature\$dirPath"
  } 
  else
  {       
    # File found
    Write-Host "Remove-Item "$newPath\$docsFolder\$feature\$exclude" -Force"
    Remove-Item "$newPath\$docsFolder\$feature\$exclude" -Force
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

  $destinationFolder = ".\$docsFolder\$feature\$filePath"
  If (!(Test-Path -path $destinationFolder)) { New-Item -ItemType Directory -Path $destinationFolder }

  $destinationFile = "$destinationFolder\$fileName.partial"
  $sourceFile = ".\$filePath\$fileName"

  Write-Host "ExtractPartial $destinationFile $sourceFile"
  ExtractPartial $destinationFile $sourceFile
}

function ExtractPartial {
  param (
    [string]$destinationFile,
    [string]$sourceFile
  )

  # Search occurences of markers
  [int[]]$startIndexes = GetLineNumber -pattern $partialMarkerBegin -file $sourceFile
  [int[]]$endIndexes = GetLineNumber -pattern $partialMarkerEnd -file $sourceFile
  if($startIndexes.Count -ne $endIndexes.Count)
  {
    Write-Error "File '$sourceFile' not correctly formated: partial marker count incorrect (start : [$startIndexes], end: [$endIndexes])."
    return
  }

  # Create empty file
  New-Item -Name $destinationFile -ItemType File -Force

  # Extract lines between markers
  [string[]]$content = Get-Content -Path $sourceFile
  For($i=0; $i -lt $startIndexes.Count; $i++) 
  {
    Add-Content -Path $destinationFile -Value $content[($startIndexes[$i]-1)..($endIndexes[$i]-1)]
  }
}

function GenerateZipArchive(){
    param(
        [object]$settings,
        [string]$oldPath,
        [string]$newPath
    )

    $feature = $settings.Feature
    Write-Host "Feature: $feature"

    # Copy files/folders
    ForEach($include in $settings.Contains.Include)
    {
        Write-Host "CopyFileFolder -include $include -feature $feature"
        CopyFileFolder -include $include -feature $feature -oldPath $oldPath -newPath $newPath
    }

    # Remove files/folders
    ForEach($exclude in $settings.Contains.Exclude)
    {
        Write-Host "RemoveFileFolder -exclude $exclude -feature $feature"
        RemoveFileFolder -exclude $exclude -feature $feature
    }

    # Extract partial
    ForEach($partial in $settings.Partial)
    {
        Write-Host "ExtractPartialFile -partial $partial -feature $feature"
        ExtractPartialFile -partial $partial -feature $feature
    }

    # Create Zip
    $zipName = $settings.ZipName
    Write-Host "Zip $feature to $zipName" 
    compress-archive -path ".\$docsFolder\$feature\*" -destinationpath ".\$docsFolder\$zipName" -compressionlevel optimal

    # Delete temp folder
    Write-Host "RemoveFolder -path .\$docsFolder\$feature"
    RemoveFolder -path ".\$docsFolder\$feature" 
}