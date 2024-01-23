# $oldName = Read-Host "old project name ?"
$oldName = 'BIADemo'
# $newName = Read-Host "new project name ?"
$newName = 'BIATemplate'

$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
$newPath = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath("$scriptPath\..\..\$newName\Angular")
$oldPath = Resolve-Path -Path "$scriptPath\..\..\$oldName\Angular"

Write-Host "old name: " $oldName
Write-Host "new name: " $newName


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
  Write-Host "start " $start "end " $end "file " $file
  (Get-Content $file) | Where-Object {
	(($i -ne $start -1 -or $_.Trim() -ne '') -and 
    ($i -lt $start -or $i -gt $end))
    $i++
  } | set-content $file 
}

# Deletes lines between // Begin BIADemo and // End BIADemo 
function RemoveCodeExample {
    param(
        $Path,
		$ExcludeDir
    )
	foreach ($childDirectory in Get-ChildItem -Force -Path $Path -Directory -Exclude $ExcludeDir) {
        RemoveCodeExample -Path $childDirectory.FullName -Exclude $ExcludeDir
    }	
	
  Get-ChildItem -Path $Path -File | Where-Object { $_.FullName -NotLike "*.ps1" -and $_.FullName -NotLike "*.md" } | ForEach-Object { 
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

function RemoveBIADemoOnlyFiles {
    param(
        $Path,
		$ExcludeDir
    )
	foreach ($childDirectory in Get-ChildItem -Force -Path $Path -Directory -Exclude $ExcludeDir) {
        RemoveBIADemoOnlyFiles -Path $childDirectory.FullName -Exclude $ExcludeDir
    }
	foreach ($childFile in Get-ChildItem -Path $Path -File | Where-Object { Select-String "// BIADemo only" $_ -Quiet } ) { 
		$file = $childFile.FullName
		$fileRel = Resolve-Path -Path "$file" -Relative
		$searchWord = '// BIADemo only'
		$starts = GetLineNumber -pattern $searchWord -file $file
		if ($starts -eq 1)
		{
			Write-Verbose "Remove $fileRel" -Verbose
			Remove-Item -Force -LiteralPath $file
		}
	}
}

function RemoveEmptyFolder {
    param(
        $Path,
		$ExcludeDir
    )
    foreach ($childDirectory in Get-ChildItem -Force -Path $Path -Directory -Exclude $ExcludeDir) {
        RemoveEmptyFolder -Path $childDirectory.FullName -Exclude $ExcludeDir
    }
    $currentChildren = Get-ChildItem -Force -LiteralPath $Path
    $isEmpty = $currentChildren -eq $null
    if ($isEmpty) {
	 	$fileRel = Resolve-Path -Path "$Path" -Relative
        Write-Verbose "Removing empty folder '${fileRel}'." -Verbose
        Remove-Item -Force -LiteralPath $Path
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

function ReplaceProjectName {
  param (
    [string]$oldName,
    [string]$newName,
	$Path,
	$ExcludeDir
  )
  foreach ($childDirectory in Get-ChildItem -Force -Path $Path -Directory -Exclude $ExcludeDir) {
	ReplaceProjectName -oldName $oldName -newName $newName -Path $childDirectory.FullName -Exclude $ExcludeDir
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

RemoveFolderContents -path "$newPath" -Exclude ('dist', 'node_modules', '.angular')

Write-Host "Copy from $oldPath to $newPath"
Copy-Item -Path (Get-Item -Path "$oldPath\*" -Exclude ('dist', 'node_modules', '.angular')).FullName -Destination $newPath -Recurse -Force

Set-Location -Path $newPath

New-Item -ItemType Directory -Path '.\docs'


$myJson = Get-Content "$oldPath\..\BIAToolKit.json" -Raw | ConvertFrom-Json 

# Get settings for type
$settingsCrudPlane = [System.Linq.Enumerable]::First($myJson, [Func[object,bool]]{ param($x) $x.Type -eq "CRUD" })
$settingsCrudPlaneFullCode = [System.Linq.Enumerable]::Last($myJson, [Func[object,bool]]{ param($x) $x.Type -eq "CRUD" })
$settingsOption = [System.Linq.Enumerable]::FirstOrDefault($myJson, [Func[object,bool]]{ param($x) $x.Type -eq "Option" })
$settingsTeam = [System.Linq.Enumerable]::FirstOrDefault($myJson, [Func[object,bool]]{ param($x) $x.Type -eq "Team" })

$featureCrudPlane = $settingsCrudPlane.Feature
$featureCrudPlaneFullCode = $settingsCrudPlaneFullCode.Feature
$featureOption = $settingsOption.Feature
$featureTeam = $settingsTeam.Feature

# Copy files
ForEach($contains in $settingsCrudPlane.Contains)
{
    Write-Host "Copy-Item -Path "$oldPath\$contains" -Destination "$newPath\docs\$featureCrudPlane\$contains" -Recurse -Force"
    Copy-Item -Path "$oldPath\$contains" -Destination "$newPath\docs\$featureCrudPlane\$contains" -Recurse -Force
}

ForEach($contains in $settingsCrudPlaneFullCode.Contains)
{
    Write-Host " Copy-Item -Path "$oldPath\$contains" -Destination "$newPath\docs\$featureCrudPlaneFullCode\$contains" -Recurse -Force"
    Copy-Item -Path "$oldPath\$contains" -Destination "$newPath\docs\$featureCrudPlaneFullCode\$contains" -Recurse -Force
}

ForEach($contains in $settingsOption.Contains)
{
    Write-Host "Copy-Item -Path "$oldPath\$contains" -Destination "$newPath\docs\$featureOption\$contains" -Recurse -Force"
    Copy-Item -Path "$oldPath\$contains" -Destination "$newPath\docs\$featureOption\$contains" -Recurse -Force
}

ForEach($contains in $settingsTeam.Contains)
{
    Write-Host " Copy-Item -Path "$oldPath\$contains" -Destination "$newPath\docs\$featureTeam\$contains" -Recurse -Force"
    Copy-Item -Path "$oldPath\$contains" -Destination "$newPath\docs\$featureTeam\$contains" -Recurse -Force
}


# Extract partial
ForEach($partial in $settingsCrudPlane.Partial)
{
    $index = $partial.lastIndexOf('\')
    $fileName = $partial.Substring($index + 1)
    $filePath = $partial.Substring(0, $index)

    Write-Host "ExtractPartial $featureCrudPlane $filePath $fileName"
    ExtractPartial $featureCrudPlane $filePath $fileName
}

# Create Zip
$zipNameCrudPlane = $settingsCrudPlane.ZipName
$zipNameCrudPlaneFullCode = $settingsCrudPlaneFullCode.ZipName
$zipNameOption = $settingsOption.ZipName
$zipNameTeam = $settingsTeam.ZipName

Write-Host "Zip $featureCrudPlane"
compress-archive -path ".\docs\$featureCrudPlane\*"         -destinationpath ".\docs\$zipNameCrudPlane"         -compressionlevel optimal
Write-Host "Zip $featureCrudPlaneFullCode"
compress-archive -path ".\docs\$featureCrudPlaneFullCode\*" -destinationpath ".\docs\$zipNameCrudPlaneFullCode" -compressionlevel optimal
Write-Host "Zip $featureOption"
compress-archive -path ".\docs\$featureOption\*"            -destinationpath ".\docs\$zipNameOption"            -compressionlevel optimal
Write-Host "Zip $featureTeam"
compress-archive -path ".\docs\$featureTeam\*"              -destinationpath ".\docs\$zipNameTeam"              -compressionlevel optimal

# Delete temp folders
RemoveFolder  -path ".\docs\$featureCrudPlane"
RemoveFolder  -path ".\docs\$featureCrudPlaneFullCode"
RemoveFolder  -path ".\docs\$featureOption"
RemoveFolder  -path ".\docs\$featureTeam"


#Write-Host "RemoveFolder dist"
#RemoveFolder -path 'dist'
#Write-Host "RemoveFolder node_modules"
#RemoveFolder -path 'node_modules'

Write-Host "RemoveFolder src\app\features\aircraft-maintenance-companies"
RemoveFolder -path 'src\app\features\aircraft-maintenance-companies'

Write-Host "RemoveFolder src\app\features\planes"
RemoveFolder -path 'src\app\features\planes'
Write-Host "RemoveFolder src\app\features\planes-full-code"
RemoveFolder -path 'src\app\features\planes-full-code'
Write-Host "RemoveFolder src\app\features\planes-specific"
RemoveFolder -path 'src\app\features\planes-specific'
Write-Host "RemoveFolder src\app\features\planes-types"
RemoveFolder -path 'src\app\features\planes-types'
Write-Host "RemoveFolder src\app\features\airports"
RemoveFolder -path 'src\app\features\airports'
Write-Host "RemoveFolder src\app\features\hangfire"
RemoveFolder -path 'src\app\features\hangfire'

Write-Host "RemoveFolder src\app\domains\airport-option"
RemoveFolder -path 'src\app\domains\airport-option'
Write-Host "RemoveFolder src\app\domains\plane-type-option"
RemoveFolder -path 'src\app\domains\plane-type-option'

#Write-Host "RemoveFolder src\assets\bia\primeng\sass"
#RemoveFolder -path 'src\assets\bia\primeng\sass'
#RemoveFolderContentss -path 'src\assets\bia\primeng\layout\css' -extension '*.scss'
#RemoveFolderContentss -path 'src\assets\bia\primeng\theme' -extension '*.scss'

RemoveFolder -path 'src\assets\bia\primeng\bia'
RemoveFolder -path 'src\assets\bia\primeng\layout\images'

Write-Host "Remove BIA demo only files"
RemoveBIADemoOnlyFiles -Path $newPath -ExcludeDir ('dist', 'node_modules', '.angular')

Write-Host "Remove Empty Folder"
RemoveEmptyFolder "." -Path $newPath -ExcludeDir ('dist', 'node_modules', '.angular', 'PublishProfiles','RepoContract')

Write-Host "Remove code example partial files"
RemoveCodeExample -Path $newPath -ExcludeDir ('dist', 'node_modules', '.angular', 'docs', 'scss' )

Write-Host "replace project name"
ReplaceProjectName -oldName $oldName -newName $newName -Path $newPath  -ExcludeDir ('dist', 'node_modules', '.angular', 'scss')
ReplaceProjectName -oldName $oldName.ToLower() -newName $newName.ToLower() -Path $newPath  -ExcludeDir ('dist', 'node_modules', '.angular', 'scss')

# Write-Host "npm install"
# npm install
# Write-Host "ng build --aot"
# ng build --aot


$a = Get-Content $newPath'\angular.json' -raw | ConvertFrom-Json
$a.projects.BIATemplate.architect.build.options.serviceWorker=$false
$a | ConvertTo-Json -depth 32 | Format-Json |set-content $newPath'\angular.json'


Set-Location -Path $scriptPath


# Write-Host "Prepare the zip."
# compress-archive -path '.\Angular' -destinationpath '..\BIADemo\Docs\Templates\VX.Y.Z\BIA.AngularTemplate.X.Y.Z.zip' -compressionlevel optimal -Force


Write-Host "Finish"
pause
