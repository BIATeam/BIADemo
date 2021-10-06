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
        Remove-Item $_.FullName
    }
	

$sourceDir = Resolve-Path -Path "$scriptPath\BIATemplateFiles"
$targetDir = Resolve-Path -Path "$scriptPath\..\..\$templateName"
Write-Host "Copy from $sourceDir to $targetDir"
Get-ChildItem -File $sourceDir -recurse |`
    foreach{
        $targetFile = $targetDir.Path + $_.FullName.SubString($sourceDir.Path.Length)

		Write-Host "Copy file " $_.FullName " to $targetFile"
		New-Item -ItemType File -Path $targetFile -Force | Out-Null
        Copy-Item $_.FullName -destination $targetFile
    }


