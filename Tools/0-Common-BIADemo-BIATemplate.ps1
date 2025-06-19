$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
Import-Module -Force $scriptPath/Library-BIA-BIATemplate.psm1

# $oldName = Read-Host "old project name ?"
$oldName = 'BIADemo'
# $newName = Read-Host "new project name ?"
$newName = 'BIATemplate'

Write-Host "old name: " $oldName
Write-Host "new name: " $newName

$newPath = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath("$scriptPath\..\..\$newName")
$oldPath = Resolve-Path -Path "$scriptPath\..\..\$oldName"

###### ###### ###### Process .bia ###### ###### ######
$jsonFileName = 'BIAToolKit_FeatureSetting.json'
$docsFolder = '.bia'

RemoveFolderContents -path "$newPath\$docsFolder"

Write-Host "Copy from $oldPath\$docsFolder to $newPath\$docsFolder"
Copy-Item -Path (Get-Item -Path "$oldPath\$docsFolder\$jsonFileName").FullName -Destination "$newPath\$docsFolder\$jsonFileName" -Recurse -Force

###### ###### ###### Process .vscode ###### ###### ######
RemoveFolderContents -path "$newPath\.vscode"

Write-Host "Copy from $oldPath\.vscode to $newPath\.vscode"
Copy-Item -Path (Get-Item -Path "$oldPath\.vscode\*").FullName -Destination "$newPath\.vscode" -Recurse -Force

Write-Host "replace project name"
ReplaceProjectNameRecurse -oldName $oldName -newName $newName -Path "$newPath\.vscode" 

Write-Host "Finish"
pause