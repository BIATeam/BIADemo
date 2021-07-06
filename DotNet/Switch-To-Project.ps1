$RelativePathToBIAPackage = "..\..\BIADemo\BIAPackage"
$SolutionName = "BIADemo"
$ProjectPrefix = "TheBIADevCompany." + $SolutionName

function AddBIAProjectToSolution
{
    param([string]$layerProject, [string]$layerPackage)
	
	$SlnFile = "$SolutionName.sln"
	$BIAProjectFile = "$RelativePathToBIAPackage\BIA.Net.Core.$layerPackage\BIA.Net.Core.$layerPackage.csproj"
	$ProjectFile = ".\$ProjectPrefix.$layerProject\$ProjectPrefix.$layerProject.csproj"
	
	# Add the library project to the solution
	dotnet sln $SlnFile add  -s "BIAPackage" $BIAProjectFile
	if ($layerProject -ne "")
	{
		# Add the library reference to the executable project
		dotnet add $ProjectFile reference $BIAProjectFile
		# Remove the NuGet package reference
		dotnet remove $ProjectFile package BIA.Net.Core.$layerPackage
	}
}

AddBIAProjectToSolution "Crosscutting.Common" "Common"
AddBIAProjectToSolution "Domain.Dto" "Domain.Dto"
AddBIAProjectToSolution "Domain" "Domain"
AddBIAProjectToSolution "Application" "Application"
AddBIAProjectToSolution "Infrastructure.Data" "Infrastructure.Data"
AddBIAProjectToSolution "Infrastructure.Service" "Infrastructure.Service"
AddBIAProjectToSolution "Crosscutting.Ioc" "Ioc"
AddBIAProjectToSolution "" "Presentation.Common"
AddBIAProjectToSolution "Presentation.Api" "Presentation.Api"
AddBIAProjectToSolution "Test" "Test"
AddBIAProjectToSolution "WorkerService" "WorkerService"

# Add the library project to the solution
dotnet sln "$SolutionName.sln" add -s "BIAPackage" "$RelativePathToBIAPackage\NuGetPackage\NuGetPackage.csproj"

