$templateName = 'BIATemplate'
$companyFileName = 'BIACompanyFiles'
$version = "VX.Y.Z"


$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
$sourceDir = Resolve-Path -Path "$scriptPath\..\..\$templateName"
$targetDir = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath("$scriptPath\..\..\$companyFileName\$version")

$filter = "*.json"
$filterInclude = 'appsettings..*.json|bianetconfig..*.json'
$filterExclude = '\\bin\\|\\obj\\|.Example'

Write-Host "Remove in $targetDir"
Get-ChildItem $targetDir -filter $filter -recurse | ?{($_.fullname -match $filterInclude)-and ($_.fullname -notmatch $filterExclude)}| Remove-Item

Write-Host "Copy form $sourceDir to $targetDir"
Get-ChildItem $sourceDir -filter $filter -recurse | ?{($_.fullname -match $filterInclude)-and ($_.fullname -notmatch $filterExclude)}|`
    foreach{
        $targetFile = $targetDir + $_.FullName.SubString($sourceDir.Path.Length);

		Write-Host "Copy file to $targetFile"
		New-Item -ItemType File -Path $targetFile -Force;
        Copy-Item $_.FullName -destination $targetFile
    }

$oldPath= "$sourceDir\Angular\src\assets\bia\primeng\sass"
$newPath= "$targetDir\Angular\src\assets\bia\primeng\sass"
Get-Item -Path "$oldPath\*" -Exclude ('overrides') |`
    foreach{
        $target = $newPath + $_.FullName.SubString($oldPath.Length);
		if (Test-Path $target) {
			Write-Host "delete " $target " folder"
			Remove-Item $target -Recurse -Force -Confirm:$false
		}
        Copy-Item $_.FullName -destination $target -Recurse -Force
    }

write-host "finish"
pause