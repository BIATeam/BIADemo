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
Get-ChildItem $targetDir -filter $filter -recurse | ? { ($_.fullname -match $filterInclude) -and ($_.fullname -notmatch $filterExclude) } | Remove-Item

Write-Host "Copy form $sourceDir to $targetDir"
Get-ChildItem $sourceDir -filter $filter -recurse | ? { ($_.fullname -match $filterInclude) -and ($_.fullname -notmatch $filterExclude) } |`
    foreach {
    $targetFile = $targetDir + $_.FullName.SubString($sourceDir.Path.Length);

    Write-Host "Copy file to $targetFile"
    New-Item -ItemType File -Path $targetFile -Force;
    Copy-Item $_.FullName -destination $targetFile
}

$oldpath= "$sourcedir\angular\src\assets\bia\primeng\sass"
$newpath= "$targetdir\angular\src\assets\bia\primeng\sass"
get-item -path "$oldpath\*" -exclude ('overrides') |`
    foreach{
        $target = $newpath + $_.fullname.substring($oldpath.length);
		if (test-path $target) {
			write-host "delete " $target " folder"
			remove-item $target -recurse -force -confirm:$false
		}
        copy-item $_.fullname -destination $target -recurse -force
    }

write-host "finish"
pause