$RelativePathToBIAPackage = "..\..\BIADemo\DotNet\BIAPackage"
$SolutionName = "BIADemo"
$ProjectPrefix = "TheBIADevCompany." + $SolutionName

function EnsurePackageReferenceWithoutVersion {
    param([string]$projectFile, [string]$packageName)

    if (-not (Test-Path $projectFile)) {
        return
    }

    $xml = New-Object System.Xml.XmlDocument
    $xml.PreserveWhitespace = $true
    $xml.Load($projectFile)

    $namespaceUri = $xml.DocumentElement.NamespaceURI
    if ([string]::IsNullOrEmpty($namespaceUri)) {
        $namespaceManager = $null
        $packageXPath = "//PackageReference[@Include='${packageName}']"
        $itemGroupXPath = "//ItemGroup[PackageReference]"
    }
    else {
        $namespaceManager = New-Object System.Xml.XmlNamespaceManager($xml.NameTable)
        $namespaceManager.AddNamespace("ns", $namespaceUri)
        $packageXPath = "//ns:PackageReference[@Include='${packageName}']"
        $itemGroupXPath = "//ns:ItemGroup[ns:PackageReference]"
    }

    if ($null -eq $namespaceManager) {
        $packageNodes = $xml.SelectNodes($packageXPath)
        $itemGroups = $xml.SelectNodes($itemGroupXPath)
    }
    else {
        $packageNodes = $xml.SelectNodes($packageXPath, $namespaceManager)
        $itemGroups = $xml.SelectNodes($itemGroupXPath, $namespaceManager)
    }

    $hasChanges = $false

    foreach ($existingNode in $packageNodes) {
        $versionAttribute = $existingNode.Attributes["Version"]
        if ($null -ne $versionAttribute) {
            $existingNode.Attributes.RemoveNamedItem("Version") | Out-Null
            $hasChanges = $true
        }
    }

    if ($packageNodes.Count -gt 0) {
        if ($hasChanges) {
            $xml.Save($projectFile)
        }

        return
    }

    if ($itemGroups.Count -gt 0) {
        $targetItemGroup = $itemGroups[0]
    }
    else {
        if ([string]::IsNullOrEmpty($namespaceUri)) {
            $targetItemGroup = $xml.CreateElement("ItemGroup")
        }
        else {
            $targetItemGroup = $xml.CreateElement("ItemGroup", $namespaceUri)
        }

        $lineEnding = [Environment]::NewLine

        $xml.DocumentElement.AppendChild($xml.CreateTextNode($lineEnding + "  ")) | Out-Null
        $xml.DocumentElement.AppendChild($targetItemGroup) | Out-Null
        $xml.DocumentElement.AppendChild($xml.CreateTextNode($lineEnding)) | Out-Null
    }

    $closingWhitespace = $null
    if ($targetItemGroup.HasChildNodes) {
        $closingWhitespace = $targetItemGroup.LastChild
        if ($closingWhitespace.NodeType -ne [System.Xml.XmlNodeType]::Whitespace) {
            $closingWhitespace = $null
        }
    }

    if ($null -ne $closingWhitespace) {
        $targetItemGroup.RemoveChild($closingWhitespace) | Out-Null
    }

    if ([string]::IsNullOrEmpty($namespaceUri)) {
        $packageReference = $xml.CreateElement("PackageReference")
    }
    else {
        $packageReference = $xml.CreateElement("PackageReference", $namespaceUri)
    }

    $packageReference.SetAttribute("Include", $packageName)

    $lineEndingForItem = [Environment]::NewLine + "    "
    $targetItemGroup.AppendChild($xml.CreateTextNode($lineEndingForItem)) | Out-Null
    $targetItemGroup.AppendChild($packageReference) | Out-Null
    $targetItemGroup.AppendChild($xml.CreateTextNode([Environment]::NewLine + "  ")) | Out-Null

    $xml.Save($projectFile)
}

function RemoveProjectReferenceFromSolution {
    param([string]$layerProject, [string]$layerPackage)
	
    $SlnFile = "$SolutionName.sln"
    $BIAProjectFile = "$RelativePathToBIAPackage\BIA.Net.Core.$layerPackage\BIA.Net.Core.$layerPackage.csproj"
    $ProjectFile = ".\$ProjectPrefix.$layerProject\$ProjectPrefix.$layerProject.csproj"
	
    # Remove the library from solution
    dotnet sln $SlnFile remove $BIAProjectFile
    if ($layerProject -ne "") {
        # Remove the library reference
        dotnet remove $ProjectFile reference $BIAProjectFile
        # Restore the NuGet package reference without duplicating version metadata
        EnsurePackageReferenceWithoutVersion $ProjectFile "BIA.Net.Core"
    }
}

# Remove all individual BIA.Net.Core.* projects from solution and project references
RemoveProjectReferenceFromSolution "Crosscutting.Common" "Common"
RemoveProjectReferenceFromSolution "Domain.Dto" "Domain.Dto"
RemoveProjectReferenceFromSolution "Domain" "Domain"
RemoveProjectReferenceFromSolution "Application" "Application"
RemoveProjectReferenceFromSolution "Infrastructure.Data" "Infrastructure.Data"
RemoveProjectReferenceFromSolution "Infrastructure.Service" "Infrastructure.Service"
RemoveProjectReferenceFromSolution "Crosscutting.Ioc" "Ioc"
RemoveProjectReferenceFromSolution "Crosscutting.Ioc" "Presentation.Common"
RemoveProjectReferenceFromSolution "Presentation.Api" "Presentation.Api"
RemoveProjectReferenceFromSolution "Test" "Test"
RemoveProjectReferenceFromSolution "WorkerService" "WorkerService"

# Remove the library from solution
dotnet sln "$SolutionName.sln" remove "$RelativePathToBIAPackage\NuGetPackage\NuGetPackage.csproj"

# Remove the unified BIA.Net.Core project from the solution
dotnet sln "$SolutionName.sln" remove "$RelativePathToBIAPackage\BIA.Net.Core\BIA.Net.Core.csproj"

function UpdateDirectoryBuildPropsAnalyzersReferences {
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

    if ($null -ne $analyzersNugetsItemGroup) { 
        $biaNetAnalyzersNode = $analyzersNugetsItemGroup.PackageReference | Where-Object { $_.Include -eq "BIA.Net.Analyzers" }
        if (-not $biaNetAnalyzersNode) {
            $nugetPackageReference = $xmlContent.CreateElement("PackageReference")
            $nugetPackageReference.SetAttribute("Include", "BIA.Net.Analyzers")

            $privateAssets = $xmlContent.CreateElement("PrivateAssets")
            $privateAssets.InnerText = "all"
            $nugetPackageReference.AppendChild($privateAssets)

            $includeAssets = $xmlContent.CreateElement("IncludeAssets")
            $includeAssets.InnerText = "runtime; build; native; contentfiles; analyzers; buildtransitive"
            $nugetPackageReference.AppendChild($includeAssets)

            $analyzersNugetsItemGroup.AppendChild($nugetPackageReference)
        }
    }

    # Save the updated Directory.Build.props
    $xmlContent.Save($propsFilePath)
}

# Remove Analyzer projects
function RemoveAnalyzerProjectToSolution {
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
