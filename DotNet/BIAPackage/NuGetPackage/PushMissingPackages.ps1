param(
    [String]$files,
    [String]$apiKey,
    [String]$source,
    [Switch]$Help
)

# Display help information
if ($Help -or [string]::IsNullOrWhiteSpace($files) -or [string]::IsNullOrWhiteSpace($apiKey) -or [string]::IsNullOrWhiteSpace($source)) {
    Write-Host @"
========================================
NuGet Package Push Script - Help
========================================

DESCRIPTION:
    Pushes NuGet packages (.nupkg) to a specified feed (Azure DevOps or nuget.org).
    The script automatically sets location to its own directory.

PARAMETERS:
    -files      Pattern to match .nupkg files (required)
    -apiKey     API key or PAT for authentication (required)
    -source     Feed URL or source name (required)
    -Help       Display this help message

EXAMPLES:
    # Push all packages to Azure DevOps feed
    .\PushMissingPackages.ps1 ".\*\*.nupkg" "VSTS" "[Your Feed Name]"

    # Push a single package to Azure DevOps
    .\PushMissingPackages.ps1 ".\BIA.Net.Core.Infrastructure.Service\BIA.Net.Core.Infrastructure.Service.3.0.2.nupkg" "VSTS" "[Your Feed Name]"

    # Push all packages of specific version 5.2.3 to nuget.org
    .\PushMissingPackages.ps1 ".\*\*5.2.3.nupkg" "<API_KEY>" "https://api.nuget.org/v3/index.json"

    # Display help
    .\PushMissingPackages.ps1 -Help

NOTES:
    - The script uses -SkipDuplicate flag to avoid errors on already published versions
    - API key is masked in logs for security
    - Exit code 1 if errors occur, 0 otherwise

========================================
"@ -ForegroundColor Cyan
    exit 0
}

# Set location to script directory
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $scriptPath
Write-Host "Working directory set to: $scriptPath`n" -ForegroundColor Gray

# Check for nuget.exe presence
$nugetPath = ".\nuget.exe"
if (-not (Test-Path $nugetPath)) {
    Write-Error "nuget.exe not found in current directory. Please download it from https://www.nuget.org/downloads"
    exit 1
}

# Mask API key for security in logs
$maskedApiKey = if ($apiKey.Length -gt 8) { $apiKey.Substring(0, 4) + "***" + $apiKey.Substring($apiKey.Length - 4) } else { "****" }

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "NuGet Package Push Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Pattern: $files" -ForegroundColor White
Write-Host "Source: $source" -ForegroundColor White
Write-Host "API Key: $maskedApiKey" -ForegroundColor White
Write-Host "========================================" -ForegroundColor Cyan

# Get list of packages to push
try {
    $filesList = Get-ChildItem -Path $files -Recurse -ErrorAction Stop
}
catch {
    Write-Error "Failed to find packages matching pattern: $files. Error: $_"
    exit 1
}

if ($filesList.Count -eq 0) {
    Write-Warning "No packages found matching pattern: $files"
    exit 0
}

Write-Host "`nFound $($filesList.Count) package(s) to push.`n" -ForegroundColor Green

# Initialize counters
$successCount = 0
$skippedCount = 0
$errorCount = 0
$currentIndex = 0

# Push packages
$filesList | ForEach-Object {
    $currentIndex++
    $packageName = $_.FullName
    $packageFileName = $_.Name
    
    Write-Host "[$currentIndex/$($filesList.Count)] Pushing $packageFileName..." -ForegroundColor Yellow
    
    try {
        $output = & $nugetPath push -Source $source -ApiKey $apiKey -SkipDuplicate $packageName 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            if ($output -match "already exists and cannot be modified") {
                Write-Host "  → Skipped (already exists)" -ForegroundColor DarkGray
                $skippedCount++
            }
            else {
                Write-Host "  → Success" -ForegroundColor Green
                $successCount++
            }
        }
        else {
            Write-Warning "  → Failed: $output"
            $errorCount++
        }
    }
    catch {
        Write-Warning "  → Error: $_"
        $errorCount++
    }
}

# Display summary
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "Push Summary" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Total packages: $($filesList.Count)" -ForegroundColor White
Write-Host "Successfully pushed: $successCount" -ForegroundColor Green
Write-Host "Skipped (duplicates): $skippedCount" -ForegroundColor DarkGray
Write-Host "Errors: $errorCount" -ForegroundColor $(if ($errorCount -gt 0) { "Red" } else { "White" })
Write-Host "========================================" -ForegroundColor Cyan

if ($errorCount -gt 0) {
    exit 1
}