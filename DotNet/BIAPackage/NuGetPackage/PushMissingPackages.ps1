param([String]$files,[String]$apiKey, [String]$source)
# cd "[YourPath]\BIADemo\BIAPackage\NuGetPackage"
# Push all package :
# .\PushMissingPackages.ps1 ".\*\*.nupkg" VSTS "[Your Feed]"
# Push one package example :
# .\PushMissingPackages.ps1 ".\BIA.Net.Core.Infrastructure.Service\BIA.Net.Core.Infrastructure.Service.3.0.2.nupkg" VSTS "[Your Feed]"

$filesList = Get-ChildItem -Path $files
$filesList | ForEach-Object{
    $packageName = $_.FullName;

    .\nuget.exe push -Source $source -ApiKey $apiKey $packageName
}