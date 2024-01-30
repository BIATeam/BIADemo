$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
Import-Module $scriptPath/Library-BIA-BIATemplate.psm1

# $oldName = Read-Host "old project name ?"
$oldName = 'BIADemo'
# $newName = Read-Host "new project name ?"
$newName = 'BIATemplate'

$newPath = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath("$scriptPath\..\..\$newName\Angular")
$oldPath = Resolve-Path -Path "$scriptPath\..\..\$oldName\Angular"

Write-Host "old name: " $oldName
Write-Host "new name: " $newName

###### ###### ###### Start process ###### ###### ######
RemoveFolderContents -path "$newPath" -Exclude ('dist', 'node_modules', '.angular')

Write-Host "Copy from $oldPath to $newPath"
Copy-Item -Path (Get-Item -Path "$oldPath\*" -Exclude ('dist', 'node_modules', '.angular')).FullName -Destination $newPath -Recurse -Force

Set-Location -Path $newPath

New-Item -ItemType Directory -Path '.\docs'

# Read Json settings to generate archive
$myJson = Get-Content "$oldPath\..\BIAToolKit.json" -Raw | ConvertFrom-Json 
GenerateZipArchive -myJson $myJson -type "CRUD" -searchFirst $true 
GenerateZipArchive -myJson $myJson -type "CRUD" -searchFirst $false 
GenerateZipArchive -myJson $myJson -type "Option" -searchFirst $true 
GenerateZipArchive -myJson $myJson -type "Team" -searchFirst $true 

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
