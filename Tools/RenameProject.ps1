$rootPath = "D:\Sources\Azure.DevOps.TheBIADevCompany\DigitalManufacturing"
#$rootPath = "D:\Sources\Azure.DevOps.TheBIADevCompany\Test"

$oldName = 'BIADemo'
$oldCompagnyName = 'TheBIADevCompany'
#$oldRealSelector = read-host "old project name? "
$newName = 'BIADemo'
$newCompagnyName = 'TheBIADevCompany'
#$newRealSelector = read-host "new project name? "


Write-Host "Old name: " $oldName
Write-Host "New name: " $newName

$oldPath = "$rootPath\$oldName" 
$newPath = "$rootPath\$newName"

if ($oldPath -eq $newPath)
{
	$newPath = $newPath + "_renamed"
}

function ReplaceInFiles
{        
    param([string]$oldString, [string]$newString, [string]$filter)

    if ($oldString -ne $newString)
    {
	    write-host "Old string : $oldString > New string: $newString"
	    Get-ChildItem -Path $newPath -File -Recurse -Include $filter | Where-Object { Select-String "$oldString" $_ -Quiet } | ForEach-Object { ((Get-Content -path $_.PSPath -Raw) -creplace $oldString, $newString) | Set-Content -NoNewline -Path $_.PSPath }
    }
}

function TransformProject
{
    param([string]$oldSelector, [string]$newSelector)
		
	#$TextInfo = (Get-Culture).TextInfo
	#$oldClassName = $TextInfo.ToTitleCase("$oldSelector").replace('-', '')
	#$newClassName = $TextInfo.ToTitleCase("$newSelector").replace('-', '')
	#$oldClassNamePlural = $TextInfo.ToTitleCase("$oldSelectorPlural").replace('-', '')
	#$newClassNamePlural = $TextInfo.ToTitleCase("$newSelectorPlural").replace('-', '')

	# file name
	write-host "Replace file name"
	Get-ChildItem -Path $newPath -File -Recurse | Where-Object { $_.Name -match $oldSelector }| ForEach-Object { Rename-Item -Path $_.PSPath -NewName $_.Name.replace($oldSelector, $newSelector) } 

	write-host "Replace folder name"
	# folder name	write-host "replace folder name singular"
	Get-ChildItem -Path $newPath  -Directory -Recurse | ForEach-Object { if ($_.Name -ne $_.Name.replace($oldSelector, $newSelector)) { Rename-Item -Path $_.PSPath -NewName $_.Name.replace($oldSelector, $newSelector) } }

	$secureNameSpace = $newSelector.substring(0,1).toupper()+$newSelector.substring(1)
	write-host "Replace namespace TheBIADevCompany.$oldSelector in TheBIADevCompany.$secureNameSpace"
	ReplaceInFiles "namespace TheBIADevCompany.$oldSelector" "namespace TheBIADevCompany.$secureNameSpace" "*"
	ReplaceInFiles "using TheBIADevCompany.$oldSelector" "using TheBIADevCompany.$secureNameSpace" "*"
}
function TransformCompany
{
    param([string]$oldSelector, [string]$newSelector)
		
	# file name
	write-host "Replace file name"
	Get-ChildItem -Path $newPath -File -Recurse | Where-Object { $_.Name -match $oldSelector }| ForEach-Object { Rename-Item -Path $_.PSPath -NewName $_.Name.replace($oldSelector, $newSelector) } 

	write-host "Replace folder name"
	# folder name	write-host "replace folder name singular"
	Get-ChildItem -Path $newPath  -Directory -Recurse | ForEach-Object { if ($_.Name -ne $_.Name.replace($oldSelector, $newSelector)) { Rename-Item -Path $_.PSPath -NewName $_.Name.replace($oldSelector, $newSelector) } }

	#$secureNameSpace = $newSelector.substring(0,1).toupper()+$newSelector.substring(1)
	write-host "Replace $oldSelector in $newSelector"
	ReplaceInFiles "$oldSelector" "$newSelector" "*"
}


#if (Test-Path "$newPath\.git") {
	Write-Host "Remove $newPath"
	#Remove-Item -Force -path "$newPath\*" -Exclude ('.git')
	Remove-Item -Force -Recurse (Get-Item -Path "$newPath\*" -Exclude ('.git')).FullName
#}
#else
#{
# if ($oldName -ne $newName)
# {
 	#New-Item -Path "$newPath" -Type Directory
	#Write-Host "You should clone the repository in $newPath before launch the script ."
	#Exit
# }
#}

if ($oldPath -ne $newPath)
{
	Write-Host "Copy from .$oldPath to $newPath"
	#Copy-Item $oldPath '.' -Recurse
	Copy-Item -Path (Get-Item -Path "$oldPath\*" -Exclude ('.git', 'Angular', 'BIAPackage')).FullName -Destination $newPath -Recurse -Force
	if (Test-Path "$oldPath\Angular") {
		New-Item -Path "$newPath\Angular" -Type Directory
		Copy-Item -Path (Get-Item -Path "$oldPath\Angular\*" -Exclude ('node_modules', 'dist')).FullName -Destination "$newPath\Angular" -Recurse -Force
	}
}


if ($oldName -ne $newName)
{
    Write-Host "Transform project $oldName in $newName"
	TransformProject $oldName $newName
}


# Replace the company specificity (to adapt)
ReplaceInFiles "@thebiadevcompanygroup.com" "@the-mail-domain.bia" "*.json"
ReplaceInFiles ".electrical-power.thebiadevcompany" ".the-deploy-domain-name.bia" "*.json"
ReplaceInFiles ".devops.thebiadevcompany" ".the-shared-tools-domain-name.bia" "*.json"
TransformCompany "TheBIADevTeam" "TheBIADevTeam"
ReplaceInFiles "eu.labinal.snecma" "the-user-domain1-name.bia" "*.json"
ReplaceInFiles """EU""" """DOMAIN_BIA_1""" "*.json"
ReplaceInFiles "na.labinal.snecma" "the-user-domain2-name.bia" "*.json"
ReplaceInFiles """NA""" """DOMAIN_BIA_2""" "*.json"
ReplaceInFiles "corp.zodiac.lan" "the-user-domain3-name.bia" "*.json"
ReplaceInFiles """CORP""" """DOMAIN_BIA_3""" "*.json"
ReplaceInFiles "dmeu" "the-bia-dev-team" "*.json"
ReplaceInFiles """RD1RF1""" """DOMAIN_BIA_SRV""" "*.json"
ReplaceInFiles "rd1.rf1" "the-server-domain-name.bia" "*.json"
ReplaceInFiles "RD1RF1\\\\RD1-SEP-DM-" "DOMAIN_BIA_SRV\\PREFIX-" "*.json"



if ($oldCompagnyName -ne $newCompagnyName)
{
    Write-Host "Transform company $oldCompagnyName in $newCompagnyName"
	TransformCompany $oldCompagnyName $newCompagnyName
	TransformCompany $oldCompagnyName.ToLower() $newCompagnyName.ToLower()
}
# Security if the new name contain the old name
#$SecuredSelector = "zzzzzzzzzz" + "zzzzzzzzzz"; #splited in 2 to not be replace by current script

#TransformCrud $oldRealSelector $SecuredSelector
#TransformCrud $SecuredSelector $newRealSelector

Write-Host "Finish"
pause






