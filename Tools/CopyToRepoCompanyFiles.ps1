$sourceDir = 'D:\Sources\GitHub\BIA.Net\NetCore\BIADemo\DotNet'
$targetDir = 'D:\Sources\GitHub\BIACompanyFiles\V3.3.4\DotNet'

Remove-Item -Recurse -Force $targetDir
Get-ChildItem $sourceDir -filter "*.json" -recurse | ?{($_.fullname -match 'appsettings..*.json|bianetconfig..*.json')-and ($_.fullname -notmatch '\\bin\\|\\obj\\|.Example')}|`
    foreach{
        $targetFile = $targetDir + $_.FullName.SubString($sourceDir.Length);
        New-Item -ItemType File -Path $targetFile -Force;
        Copy-Item $_.FullName -destination $targetFile
    }
	
	