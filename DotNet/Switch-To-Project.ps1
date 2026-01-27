$RelativePathToBIAPackage = "..\..\BIADemo\DotNet\BIAPackage"
$SolutionName = "BIADemo"
$ProjectPrefix = "TheBIADevCompany." + $SolutionName

function AddProjectToSlnf {
    param([string]$slnfFile, [string]$projectPath)
    
    if (-not (Test-Path $slnfFile)) {
        return
    }
    
    $slnfContent = Get-Content $slnfFile -Raw | ConvertFrom-Json
    
    # Check if project already exists
    if ($slnfContent.solution.projects -notcontains $projectPath) {
        $slnfContent.solution.projects = @($slnfContent.solution.projects) + $projectPath
        $slnfContent.solution.projects = @($slnfContent.solution.projects | Sort-Object)
        
        # Save with proper formatting
        $json = $slnfContent | ConvertTo-Json -Depth 10
        [System.IO.File]::WriteAllText($slnfFile, $json, [System.Text.Encoding]::UTF8)
    }
}

function AddBIAProjectToSolution {
    param([string]$layerProject, [string]$layerPackage)
	
    $SlnFile = "$SolutionName.sln"
    $BIAProjectFile = "$RelativePathToBIAPackage\BIA.Net.Core.$layerPackage\BIA.Net.Core.$layerPackage.csproj"
    $ProjectFile = ".\$ProjectPrefix.$layerProject\$ProjectPrefix.$layerProject.csproj"
	
    # Add the library project to the solution
    dotnet sln $SlnFile add  -s "BIAPackage" $BIAProjectFile
    if ($layerProject -ne "") {
        # Add the library reference to the executable project
        dotnet add $ProjectFile reference $BIAProjectFile
        # Remove the NuGet package reference
        dotnet remove $ProjectFile package BIA.Net.Core.$layerPackage
    }
    
    # Add to .slnf files
    $slnfProjectPath = "BIAPackage\BIA.Net.Core.$layerPackage\BIA.Net.Core.$layerPackage.csproj"
    AddProjectToSlnf "${SolutionName}_DeployDB.slnf" $slnfProjectPath
    AddProjectToSlnf "${SolutionName}_WithoutDeployDB.slnf" $slnfProjectPath
}

AddBIAProjectToSolution "Crosscutting.Common" "Common"
AddBIAProjectToSolution "Domain.Dto" "Domain.Dto"
AddBIAProjectToSolution "Domain" "Domain"
AddBIAProjectToSolution "Application" "Application"
AddBIAProjectToSolution "Infrastructure.Data" "Infrastructure.Data"
AddBIAProjectToSolution "Infrastructure.Service" "Infrastructure.Service"
AddBIAProjectToSolution "Crosscutting.Ioc" "Ioc"
AddBIAProjectToSolution "Crosscutting.Ioc" "Presentation.Common"
AddBIAProjectToSolution "Presentation.Api" "Presentation.Api"
AddBIAProjectToSolution "Test" "Test"
AddBIAProjectToSolution "WorkerService" "WorkerService"

# Add the library project to the solution
dotnet sln "$SolutionName.sln" add -s "BIAPackage" "$RelativePathToBIAPackage\NuGetPackage\NuGetPackage.csproj"

# Add NuGetPackage to .slnf files
AddProjectToSlnf "${SolutionName}_DeployDB.slnf" "BIAPackage\NuGetPackage\NuGetPackage.csproj"
AddProjectToSlnf "${SolutionName}_WithoutDeployDB.slnf" "BIAPackage\NuGetPackage\NuGetPackage.csproj"

function UpdateDirectoryBuildPropsAnalyzersReferences {
    $propsFilePath = "Directory.Build.props"

    # Load the content of Directory.Build.props
    [xml]$xmlContent = Get-Content $propsFilePath

    # Remove existing NuGet package reference to the Analyzer
    $analyzersNugetsItemGroup = $xmlContent.Project.ItemGroup |
    Where-Object {
        $_.Label -eq "AnalyzersNugets"
    }
    Select-Object -First 1

    if ($analyzersNugetsItemGroup -ne $null) { 
        $packageReference = $analyzersNugetsItemGroup.PackageReference |
        Where-Object {
            $_.Include -eq "BIA.Net.Analyzers"
        }
        Select-Object -First 1

        if ($packageReference -ne $null) {
            $analyzersNugetsItemGroup.RemoveChild($packageReference)
            $xmlContent.Save($propsFilePath)
        }
    }

    # Add ProjectReference entries for Analyzers and CodeFixes
    $itemGroup1 = $xmlContent.CreateElement("ItemGroup")
    $itemGroup1.SetAttribute("Label", "AnalyzerReferencesProject")
    $itemGroup1.SetAttribute("Condition", "!`$(MSBuildProjectDirectory.Contains('BIAPackage')) and !`$(MSBuildProjectDirectory.Contains('Migrations'))")

    $analyzerReference1 = $xmlContent.CreateElement("ProjectReference")
    $analyzerReference1.SetAttribute("Include", "..\BIAPackage\BIA.Net.Analyzers\BIA.Net.Analyzers\BIA.Net.Analyzers.csproj")
    $analyzerReference1.SetAttribute("PrivateAssets", "all")
    $analyzerReference1.SetAttribute("ReferenceOutputAssembly", "false")
    $analyzerReference1.SetAttribute("OutputItemType", "Analyzer")
    $itemGroup1.AppendChild($analyzerReference1)

    $analyzerCodeFixReference1 = $xmlContent.CreateElement("ProjectReference")
    $analyzerCodeFixReference1.SetAttribute("Include", "..\BIAPackage\BIA.Net.Analyzers\BIA.Net.Analyzers.CodeFixes\BIA.Net.Analyzers.CodeFixes.csproj")
    $analyzerCodeFixReference1.SetAttribute("PrivateAssets", "all")
    $analyzerCodeFixReference1.SetAttribute("ReferenceOutputAssembly", "false")
    $analyzerCodeFixReference1.SetAttribute("OutputItemType", "Analyzer")
    $itemGroup1.AppendChild($analyzerCodeFixReference1)

    $xmlContent.Project.AppendChild($itemGroup1)

    $itemGroup2 = $xmlContent.CreateElement("ItemGroup")
    $itemGroup2.SetAttribute("Label", "AnalyzerReferencesBiaPackage")
    $itemGroup2.SetAttribute("Condition", "`$(MSBuildProjectDirectory.Contains('BIAPackage')) and !`$(MSBuildProjectDirectory.Contains('BIA.Net.Analyzers'))")

    $analyzerReference2 = $xmlContent.CreateElement("ProjectReference")
    $analyzerReference2.SetAttribute("Include", "..\BIA.Net.Analyzers\BIA.Net.Analyzers\BIA.Net.Analyzers.csproj")
    $analyzerReference2.SetAttribute("PrivateAssets", "all")
    $analyzerReference2.SetAttribute("ReferenceOutputAssembly", "false")
    $analyzerReference2.SetAttribute("OutputItemType", "Analyzer")
    $itemGroup2.AppendChild($analyzerReference2)

    $analyzerCodeFixReference2 = $xmlContent.CreateElement("ProjectReference")
    $analyzerCodeFixReference2.SetAttribute("Include", "..\BIA.Net.Analyzers\BIA.Net.Analyzers.CodeFixes\BIA.Net.Analyzers.CodeFixes.csproj")
    $analyzerCodeFixReference2.SetAttribute("PrivateAssets", "all")
    $analyzerCodeFixReference2.SetAttribute("ReferenceOutputAssembly", "false")
    $analyzerCodeFixReference2.SetAttribute("OutputItemType", "Analyzer")
    $itemGroup2.AppendChild($analyzerCodeFixReference2)

    $xmlContent.Project.AppendChild($itemGroup2)

    # Save the updated Directory.Build.props
    $xmlContent.Save($propsFilePath)
}

# Add Analyzer projects
function AddAnalyzerProjectToSolution {
    param([string]$analyzerProjectName)
    
    $SlnFile = "$SolutionName.sln"
    $AnalyzerProjectFile = "$RelativePathToBIAPackage\BIA.Net.Analyzers\$analyzerProjectName\$analyzerProjectName.csproj"
    
    # Add the analyzer project to the solution
    dotnet sln $SlnFile add -s "BIAPackage\BIA.Net.Analyzers" $AnalyzerProjectFile
    
    # Add to .slnf files
    $slnfProjectPath = "BIAPackage\BIA.Net.Analyzers\$analyzerProjectName\$analyzerProjectName.csproj"
    AddProjectToSlnf "${SolutionName}_DeployDB.slnf" $slnfProjectPath
    AddProjectToSlnf "${SolutionName}_WithoutDeployDB.slnf" $slnfProjectPath
}

# Add Analyzer projects to the solution
AddAnalyzerProjectToSolution "BIA.Net.Analyzers"
AddAnalyzerProjectToSolution "BIA.Net.Analyzers.CodeFixes"
AddAnalyzerProjectToSolution "BIA.Net.Analyzers.Package"
AddAnalyzerProjectToSolution "BIA.Net.Analyzers.Vsix"

# Run the function to update Directory.Build.props references to Analyzers
UpdateDirectoryBuildPropsAnalyzersReferences
