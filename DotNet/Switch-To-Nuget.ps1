$RelativePathToBIAPackage = "BIAPackage"
$SolutionName = "BIADemo"
$ProjectPrefix = "TheBIADevCompany." + $SolutionName

function AddBIAPackageToSolution {
	param([string]$layerProject, [string]$layerPackage)
	
	$SlnFile = "$SolutionName.sln"
	$BIAProjectFile = "$RelativePathToBIAPackage\BIA.Net.Core.$layerPackage\BIA.Net.Core.$layerPackage.csproj"
	$ProjectFile = ".\$ProjectPrefix.$layerProject\$ProjectPrefix.$layerProject.csproj"
	
	# Remove the library from solution
	dotnet sln $SlnFile remove $BIAProjectFile
	if ($layerProject -ne "") {
		# Remove the library reference
		dotnet remove $ProjectFile reference $BIAProjectFile
		# Restore the NuGet package reference
		dotnet add $ProjectFile package BIA.Net.Core.$layerPackage -v 4.0.7
	}
}

AddBIAPackageToSolution "Crosscutting.Common" "Common"
AddBIAPackageToSolution "Domain.Dto" "Domain.Dto"
AddBIAPackageToSolution "Domain" "Domain"
AddBIAPackageToSolution "Application" "Application"
AddBIAPackageToSolution "Infrastructure.Data" "Infrastructure.Data"
AddBIAPackageToSolution "Infrastructure.Service" "Infrastructure.Service"
AddBIAPackageToSolution "Crosscutting.Ioc" "Ioc"
AddBIAPackageToSolution "Crosscutting.Ioc" "Presentation.Common"
AddBIAPackageToSolution "Presentation.Api" "Presentation.Api"
AddBIAPackageToSolution "Test" "Test"
AddBIAPackageToSolution "WorkerService" "WorkerService"

# Remove the library from solution
dotnet sln "$SolutionName.sln" remove "$RelativePathToBIAPackage\NuGetPackage\NuGetPackage.csproj"

function UpdateDirectoryBuildPropsAnalyzersReferences
{
    $propsFilePath = "Directory.Build.props"

    # Load the content of Directory.Build.props
    [xml]$xmlContent = Get-Content $propsFilePath

    # Remove ItemGroup nodes with specific conditions
    $itemGroupsToRemove = $xmlContent.Project.ItemGroup |
    Where-Object {
        $_.Label -eq "AnalyzerReferencesProject" -or
        $_.Label -eq "AnalyzerReferencesBiaPackage"
    }
    $itemGroupsToRemove | ForEach-Object { $_.ParentNode.RemoveChild($_) }
    $xmlContent.Save($propsFilePath)

    # Add the NuGet PackageReference back
    $analyzersNugetsItemGroup = $xmlContent.Project.ItemGroup |
    Where-Object {
        $_.Label -eq "AnalyzersNugets"
    }
    Select-Object -First 1

    if($analyzersNugetsItemGroup -ne $null) { 
        $nugetPackageReference = $xmlContent.CreateElement("PackageReference")
        $nugetPackageReference.SetAttribute("Include", "BIA.Net.Analyzers")
        $nugetPackageReference.SetAttribute("Version", "4.0.7")

        $privateAssets = $xmlContent.CreateElement("PrivateAssets")
        $privateAssets.InnerText = "all"
        $nugetPackageReference.AppendChild($privateAssets)

        $includeAssets = $xmlContent.CreateElement("IncludeAssets")
        $includeAssets.InnerText = "runtime; build; native; contentfiles; analyzers; buildtransitive"
        $nugetPackageReference.AppendChild($includeAssets)

        $analyzersNugetsItemGroup.AppendChild($nugetPackageReference)
    }

    # Save the updated Directory.Build.props
    $xmlContent.Save($propsFilePath)
}

# Remove Analyzer projects
function RemoveAnalyzerProjectToSolution
{
    param([string]$analyzerProjectName)
    
    $SlnFile = "$SolutionName.sln"
    $AnalyzerProjectFile = "$RelativePathToBIAPackage\BIA.Net.Analyzers\$analyzerProjectName\$analyzerProjectName.csproj"
    
    # Add the analyzer project to the solution
    dotnet sln $SlnFile remove $AnalyzerProjectFile
}

# Remove Analyzer projects to the solution
RemoveAnalyzerProjectToSolution "BIA.Net.Analyzers"
RemoveAnalyzerProjectToSolution "BIA.Net.Analyzers.CodeFixes"
RemoveAnalyzerProjectToSolution "BIA.Net.Analyzers.Package"
RemoveAnalyzerProjectToSolution "BIA.Net.Analyzers.Vsix"

# Run the function to update Directory.Build.props references to Analyzers
UpdateDirectoryBuildPropsAnalyzersReferences
