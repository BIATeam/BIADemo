$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
Import-Module $scriptPath/Library-BIA-BIATemplate.psm1

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

###### ###### ###### Start process ###### ###### ######
RemoveFolder -path $newPath

Write-Host "Copy from $oldPath to $newPath"
Copy-Item -Path $oldPath -Destination $newPath -Recurse -Force

$biaPackage = $newPath + "\BIAPackage"
RemoveFolder -path $biaPackage

Set-Location -Path $newPath

Write-Host "Zip plane"

# Read Json settings to generate archive
$myJson = Get-Content "$oldPath\$jsonFileName" -Raw | ConvertFrom-Json 
ForEach($settings in $myJson)
{
    GenerateZipArchive -settings $settings -settingsName $jsonFileName
}
Copy-Item -Path "$oldPath\$jsonFileName" -Destination "$newPath\$docsFolder\$jsonFileName" -Force

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
# compress-archive -path $targetDir -destinationpath "..\BIADemo\$docsFolder\Templates\VX.Y.Z\BIA.DotNetTemplate.X.Y.Z.zip" -compressionlevel optimal -Force
# RemoveItemFolder -path 'Tmp'

write-host "finish"
pause
