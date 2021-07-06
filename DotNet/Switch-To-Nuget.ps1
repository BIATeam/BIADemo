$RelativePathToBIAPackage = "..\..\BIADemo\BIAPackage"
$SolutionName = "BIADemo"
$ProjectPrefix = "TheBIADevCompany." + $SolutionName

function AddBIAPackageToSolution
{
    param([string]$layerProject, [string]$layerPackage)
	
	$SlnFile = "$SolutionName.sln"
	$BIAProjectFile = "$RelativePathToBIAPackage\BIA.Net.Core.$layerPackage\BIA.Net.Core.$layerPackage.csproj"
	$ProjectFile = ".\$ProjectPrefix.$layerProject\$ProjectPrefix.$layerProject.csproj"
	
	# Remove the library from solution
	dotnet sln $SlnFile remove $BIAProjectFile
	if ($layerProject -ne "")
	{
		# Remove the library reference
		dotnet remove $ProjectFile reference $BIAProjectFile
		# Restore the NuGet package reference
		dotnet add $ProjectFile package BIA.Net.Core.$layerPackage
	}
}

AddBIAPackageToSolution "Crosscutting.Common" "Common"
AddBIAPackageToSolution "Domain.Dto" "Domain.Dto"
AddBIAPackageToSolution "Domain" "Domain"
AddBIAPackageToSolution "Application" "Application"
AddBIAPackageToSolution "Infrastructure.Data" "Infrastructure.Data"
AddBIAPackageToSolution "Infrastructure.Service" "Infrastructure.Service"
AddBIAPackageToSolution "Crosscutting.Ioc" "Ioc"
AddBIAPackageToSolution "" "Presentation.Common"
AddBIAPackageToSolution "Presentation.Api" "Presentation.Api"
AddBIAPackageToSolution "Test" "Test"
AddBIAPackageToSolution "WorkerService" "WorkerService"

# Remove the library from solution
dotnet sln "$SolutionName.sln" remove "$RelativePathToBIAPackage\NuGetPackage\NuGetPackage.csproj"
