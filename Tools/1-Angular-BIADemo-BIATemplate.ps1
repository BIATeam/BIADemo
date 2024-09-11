$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
Import-Module $scriptPath/Library-BIA-BIATemplate.psm1

# $oldName = Read-Host "old project name ?"
$oldName = 'BIADemo'
# $newName = Read-Host "new project name ?"
$newName = 'BIATemplate'

$jsonFileName = 'BIAToolKit.json'
$docsFolder = '.bia'

$newPath = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath("$scriptPath\..\..\$newName\Angular")
$oldPath = Resolve-Path -Path "$scriptPath\..\..\$oldName\Angular"

Write-Host "old name: " $oldName
Write-Host "new name: " $newName

###### ###### ###### Specific Functions ###### ###### ######
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
    if ($starts -eq 1) {
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

###### ###### ###### Start process ###### ###### ######
RemoveFolderContents -path "$newPath" -Exclude ('dist', 'node_modules', '.angular')

Write-Host "Copy from $oldPath to $newPath"
Copy-Item -Path (Get-Item -Path "$oldPath\*" -Exclude ('dist', 'node_modules', '.angular')).FullName -Destination $newPath -Recurse -Force

Set-Location -Path $newPath

#New-Item -ItemType Directory -Path $docsFolder

# Read Json settings to generate archive
$myJson = Get-Content "$oldPath\$jsonFileName" -Raw | ConvertFrom-Json 
ForEach ($settings in $myJson) {
  GenerateZipArchive -settings $settings -settingsName $jsonFileName -oldPath $oldPath -newPath $newPath
}

Write-Host "Copy-Item -Path $oldPath\$jsonFileName -Destination $newPath\$docsFolder\$jsonFileName -Force"
Copy-Item -Path "$oldPath\$jsonFileName" -Destination "$newPath\$docsFolder\$jsonFileName" -Force

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
Write-Host "RemoveFolder src\app\domains\country-option"
RemoveFolder -path 'src\app\domains\country-option'

#Write-Host "RemoveFolder src\assets\bia\primeng\sass"
#RemoveFolder -path 'src\assets\bia\primeng\sass'
#RemoveFolderContentss -path 'src\assets\bia\primeng\layout\css' -extension '*.scss'
#RemoveFolderContentss -path 'src\assets\bia\primeng\theme' -extension '*.scss'

RemoveFolder -path 'src\assets\bia\primeng\bia'
RemoveFolder -path 'src\assets\bia\primeng\layout\images'

Write-Host "Remove BIA demo only files"
RemoveBIADemoOnlyFiles -Path $newPath -ExcludeDir ('dist', 'node_modules', '.angular')

Write-Host "Remove Empty Folder"
RemoveEmptyFolder "." -Path $newPath -ExcludeDir ('dist', 'node_modules', '.angular', 'PublishProfiles', 'RepoContract')

Write-Host "Remove code example partial files"
RemoveCodeExample -Path $newPath -ExcludeDir ('dist', 'node_modules', '.angular', $docsFolder, 'scss' )

Write-Host "replace project name"
ReplaceProjectName -oldName $oldName -newName $newName -Path $newPath  -ExcludeDir ('dist', 'node_modules', '.angular', 'scss')
ReplaceProjectName -oldName $oldName.ToLower() -newName $newName.ToLower() -Path $newPath  -ExcludeDir ('dist', 'node_modules', '.angular', 'scss')

$a = Get-Content $newPath'\angular.json' -raw | ConvertFrom-Json
$a.projects.BIATemplate.architect.build.options.serviceWorker = $false
$a | ConvertTo-Json -depth 32 | Format-Json | set-content $newPath'\angular.json'

Write-Host "npm install"
npm install
Write-Host "npm run clean"
npm run clean

Set-Location -Path $scriptPath


# Write-Host "Prepare the zip."
# compress-archive -path '.\Angular' -destinationpath "..\BIADemo\$docsFolder\Templates\VX.Y.Z\BIA.AngularTemplate.X.Y.Z.zip" -compressionlevel optimal -Force


Write-Host "Finish"
pause