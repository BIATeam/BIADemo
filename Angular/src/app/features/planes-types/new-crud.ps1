$oldRealSelector = 'plane-type'
$oldRealSelectorPlural = 'planes-types'
$newRealSelector = read-host "new crud name? (singular)"
$newRealSelectorPlural = read-host "new crud name? (plural)"
$oldRealSelector = $oldRealSelector.ToLower();
$newRealSelector = $newRealSelector.ToLower();
$oldRealSelectorPlural = $oldRealSelectorPlural.ToLower();
$newRealSelectorPlural = $newRealSelectorPlural.ToLower();

function ReplaceInFiles
{
    param([string]$oldString, [string]$newString)
	write-host "old " $oldString "new " $newString
	Get-ChildItem -File -Recurse | ForEach-Object { ((Get-Content -path $_.PSPath -Raw) -creplace $oldString, $newString) | Set-Content -NoNewline -Path $_.PSPath }
}

function TransformCrud
{
    param([string]$oldSelector, [string]$oldSelectorPlural, [string]$newSelector, [string]$newSelectorPlural)
		

	$TextInfo = (Get-Culture).TextInfo
	$oldClassName = $TextInfo.ToTitleCase("$oldSelector").replace('-', '')
	$newClassName = $TextInfo.ToTitleCase("$newSelector").replace('-', '')
	$oldClassNamePlural = $TextInfo.ToTitleCase("$oldSelectorPlural").replace('-', '')
	$newClassNamePlural = $TextInfo.ToTitleCase("$newSelectorPlural").replace('-', '')

	# file name
	write-host "replace file name plural"
	Get-ChildItem -File -Recurse | ForEach-Object { Rename-Item -Path $_.PSPath -NewName $_.Name.replace($oldSelectorPlural, $newSelectorPlural) } 
	write-host "replace file name singular"
	Get-ChildItem -File -Recurse | ForEach-Object { Rename-Item -Path $_.PSPath -NewName $_.Name.replace($oldSelector, $newSelector) } 

	# folder name
	write-host "replace folder name plural"
	Get-ChildItem -Directory -Recurse | ForEach-Object { if ($_.Name -ne $_.Name.replace($oldSelectorPlural, $newSelectorPlural)) { Rename-Item -Path $_.PSPath -NewName $_.Name.replace($oldSelectorPlural, $newSelectorPlural) } }
	write-host "replace folder name singular"
	Get-ChildItem -Directory -Recurse | ForEach-Object { if ($_.Name -ne $_.Name.replace($oldSelector, $newSelector)) { Rename-Item -Path $_.PSPath -NewName $_.Name.replace($oldSelector, $newSelector) } }

	# ----------------------------------------
	ReplaceInFiles $oldClassNamePlural $newClassNamePlural
	ReplaceInFiles $oldClassName $newClassName

	if ($newSelector.Contains('-') -or $oldSelector.Contains('-'))
	{
		$oldPrefixPlural = $oldClassNamePlural.substring(0,1).tolower()+$oldClassNamePlural.substring(1);    
		$newPrefixPlural = $newClassNamePlural.substring(0,1).tolower()+$newClassNamePlural.substring(1);    
		$oldPrefix = $oldClassName.substring(0,1).tolower()+$oldClassName.substring(1);    
		$newPrefix = $newClassName.substring(0,1).tolower()+$newClassName.substring(1);    

		# No replace in path (after /), not in string concatenated (after -) and not in string (after ')
		$old = '(?<![-/''])' + $oldPrefixPlural
		$new = $newPrefixPlural
		ReplaceInFiles $old $new

		# No replace in path (after /), not in string concatenated (after -) and not in string (after ')
		$old = '(?<![-/''])' + $oldPrefix
		$new = $newPrefix
		ReplaceInFiles $old $new
	 
		#Replace prefixe in some string (not done before) when it is before . except in path (/ before)
		$old = '(?<![/])' + $oldPrefix + '(?=\.)'
		$new = $newPrefix
		ReplaceInFiles $old $new
		
		# Special path for pluck('...') becauce it is name of properties
		$old = '(?<=pluck\('')' + $oldPrefixPlural + '(?=''\))'
		$new = $newPrefixPlural
		ReplaceInFiles $old $new
		
		$old = '(?<=pluck\('')' + $oldPrefix + '(?=''\))'
		$new = $newPrefix
		ReplaceInFiles $old $new
	}

	ReplaceInFiles $oldSelectorPlural $newSelectorPlural

	ReplaceInFiles $oldSelector $newSelector
}

# Security if the new name contain the old name
$SecuredSelector = "zzzzzzzzzz" + "zzzzzzzzzz"; #splited in 2 to not be replace by current script
$SecuredSelectorPlural = $SecuredSelector + "s";

TransformCrud $oldRealSelector $oldRealSelectorPlural $SecuredSelector $SecuredSelectorPlural
TransformCrud $SecuredSelector $SecuredSelectorPlural $newRealSelector $newRealSelectorPlural 

Write-Host "Finish"
pause






