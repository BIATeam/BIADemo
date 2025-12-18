$BIAPackagePath = split-path -parent $MyInvocation.MyCommand.Definition
$Configuration = "Release"

# Resolve path
$packageFullPath = Resolve-Path -Path $BIAPackagePath -ErrorAction SilentlyContinue
if (-not $packageFullPath) {
    Write-Error "Directory '$BIAPackagePath' not found."
    exit 1
}
$packageFullPath = $packageFullPath.Path

Write-Host "Searching for BIA.Net projects in '$packageFullPath'..."
$projects = Get-ChildItem -Path $packageFullPath -Recurse -Filter "BIA.Net.*.csproj" -File -ErrorAction SilentlyContinue

if (-not $projects -or $projects.Count -eq 0) {
    Write-Warning "No 'BIA.Net.*' projects found in $packageFullPath"
    exit 0
}

$successCount = 0
$failureCount = 0

foreach ($proj in $projects) {
    $projName = $proj.BaseName
    
    Write-Host "------------------------------------------------------------"
    Write-Host "Building and packing project: $($proj.Name)"
    Write-Host "Configuration: $Configuration"
    
    # Define output directory for this specific project
    $outputDir = Join-Path -Path $packageFullPath -ChildPath "NuGetPackage\$projName"
    
    try {
        # Step 1: Build the project first
        Write-Host "Building..."
        $buildCmd = @('build', $proj.FullName, '-c', $Configuration)
        $buildOutput = & dotnet @buildCmd 2>&1
        
        if ($LASTEXITCODE -ne 0) {
            Write-Host "✗ Build failed for $($proj.Name) (ExitCode: $LASTEXITCODE)." -ForegroundColor Red
            Write-Host $buildOutput -ForegroundColor Red
            $failureCount++
            continue
        }
        
        # Step 2: Pack without rebuilding
        Write-Host "Packing..."
        $packCmd = @(
            'pack',
            $proj.FullName,
            '-c', $Configuration,
            '-o', $outputDir,
            '--no-build'
        )
        
        $output = & dotnet @packCmd 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "✓ Pack succeeded for $($proj.Name)."
            $successCount++
        }
        else {
            Write-Host "✗ Pack failed for $($proj.Name) (ExitCode: $LASTEXITCODE)." -ForegroundColor Red
            Write-Host $output -ForegroundColor Red
            $failureCount++
        }
    }
    catch {
        Write-Host "✗ Exception while packing $($proj.Name): $_" -ForegroundColor Red
        $failureCount++
    }
}

Write-Host "------------------------------------------------------------"
Write-Host "Done. Success: $successCount, Failures: $failureCount"
Write-Host "NuGet packages (.nupkg) are in: $packageFullPath\NuGetPackage"
