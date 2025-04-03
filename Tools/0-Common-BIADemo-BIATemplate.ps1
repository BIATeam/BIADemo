$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
Import-Module -Force $scriptPath/Library-BIA-BIATemplate.psm1

# $oldName = Read-Host "old project name ?"
$oldName = 'BIADemo'
# $newName = Read-Host "new project name ?"
$newName = 'BIATemplate'

$jsonFileName = 'BIAToolKit.json'
$docsFolder = '.bia'


###### ###### ###### Process .vscode ###### ###### ######
$newPath = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath("$scriptPath\..\..\$newName\.vscode")
$oldPath = Resolve-Path -Path "$scriptPath\..\..\$oldName\.vscode"

Write-Host "old name: " $oldName
Write-Host "new name: " $newName


RemoveFolderContents -path "$newPath"

Write-Host "Copy from $oldPath to $newPath"
Copy-Item -Path (Get-Item -Path "$oldPath\*").FullName -Destination $newPath -Recurse -Force

Write-Host "replace project name"
ReplaceProjectNameRecurse -oldName $oldName -newName $newName -Path $newPath 

Write-Host "Finish"
pause