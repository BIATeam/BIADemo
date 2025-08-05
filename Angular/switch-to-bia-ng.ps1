$SourceFrontEnd = "."

$ExcludeDir = ('dist', 'node_modules', 'docs', 'scss', '.git', '.vscode', '.angular', '.dart_tool', 'bia-shared', 'bia-features', 'bia-domains', 'bia-core', '.bia')

function ReplaceInProject {
  param (
    [string]$Source,
    [string]$OldRegexp,
    [string]$NewRegexp,
    [string]$Include

  )
  Write-Host "ReplaceInProject $OldRegexp by $NewRegexp";
  #Write-Host $Source;
  #Write-Host $OldRegexp;
  #Write-Host $NewRegexp;
  #Write-Host $Filter;
  ReplaceInProjectRec -Source $Source -OldRegexp $OldRegexp -NewRegexp $NewRegexp -Include $Include
}

function ReplaceInProjectRec {
  param (
    [string]$Source,
    [string]$OldRegexp,
    [string]$NewRegexp,
    [string]$Include
  )
  foreach ($childDirectory in Get-ChildItem -Force -Path $Source -Directory -Exclude $ExcludeDir) {
    ReplaceInProjectRec -Source $childDirectory.FullName -OldRegexp $OldRegexp -NewRegexp $NewRegexp -Include $Include
  }
	
  Get-ChildItem -LiteralPath $Source -File -Filter $Include | ForEach-Object {
    $oldContent = [System.IO.File]::ReadAllText($_.FullName);
    $found = $oldContent | select-string -Pattern $OldRegexp
    if ($found.Matches) {
      $newContent = $oldContent -Replace $OldRegexp, $NewRegexp 
      $match = $newContent -match '#capitalize#([a-z])'
      if ($match) {
        [string]$lower = $Matches[1]
        [string]$upper = $lower.ToUpper()
        [string]$newContent = $newContent -Replace "#capitalize#([a-z])", $upper 
      }
      if ($oldContent -cne $newContent) {
        Write-Host "     => " $_.FullName
        [System.IO.File]::WriteAllText($_.FullName, $newContent)
      }
    }
  }
}

# Switch imports to bia-ng imports
ReplaceInProject -Source $SourceFrontEnd -OldRegexp "(import\s*{\s*[^}]+\s*}\s*from\s*)[']packages\/(bia-ng\/[^']+)\/public-api['];" -NewRegexp '$1''$2'';' -Include *.ts
ReplaceInProject -Source $SourceFrontEnd -OldRegexp "((templateUrl:|styleUrls: \[)[\s]*'[\S]*\/)packages\/bia-ng\/shared\/([\S]*')" -NewRegexp '$1node_modules/bia-ng/templates/$3' -Include *.ts


$frameworkVersionFile = Join-Path -Path $SourceFrontEnd -ChildPath "packages\bia-ng\shared\framework-version.ts"

# Extract the version from framework-version.ts
$frameworkVersionContent = Get-Content -Path $frameworkVersionFile -Raw
$frameworkVersionMatch = [regex]::Match($frameworkVersionContent, 'FRAMEWORK_VERSION\s*=\s*''([\S]+)''')
$frameworkVersion = $frameworkVersionMatch.Groups[1].Value

Write-Output "Bia Demo framework version : $frameworkVersion"

$projectPackageJsonPath = "package.json"

if (Test-Path -Path $projectPackageJsonPath -PathType Leaf) {
    # Read the package.json file
    $packageJsonContent = Get-Content -Path $projectPackageJsonPath -Raw | ConvertFrom-Json

    if ($null -eq $packageJsonContent.dependencies) {
        $packageJsonContent.dependencies = @{}
    }

    # Check if the "bia-ng" dependency does not exist
    if (-not ($packageJsonContent.dependencies.PSObject.Properties | Where-Object { $_.Name -eq "bia-ng" })) {
        # Add the "bia-ng" dependency
        $packageJsonContent.dependencies | Add-Member -NotePropertyName "bia-ng" -NotePropertyValue $frameworkVersion

        # Convert the object back to JSON and write it back to the package.json file
        $packageJsonContent | ConvertTo-Json -Depth 10 | Set-Content -Path $projectPackageJsonPath

        Write-Output "The 'bia-ng' dependency has been added with version $frameworkVersion in package.json."
    } else {
        Write-Output "The 'bia-ng' dependency already exists in package.json."
    }
}

# Define the path to the folder you want to delete
$folderPath = Join-Path -Path "." -ChildPath "packages\bia-ng"

if (Test-Path -Path $projectPackageJsonPath -PathType Leaf) {
    # Read the package.json file
    $projectPackageJsonContent = Get-Content -Path $projectPackageJsonPath -Raw | ConvertFrom-Json

    # Check if the name in package.json is not "BIADemo"
     if ($projectPackageJsonContent.name -ne "biademo") {
        # Check if the folder exists
        if (Test-Path -Path $folderPath) {
            # Delete the folder and all its contents
            Remove-Item -Path $folderPath -Recurse -Force
            Write-Output "The folder $folderPath has been deleted successfully."
        } else {
            Write-Output "The folder $folderPath does not exist."
        }
    } else {
        Write-Output "The name in package.json is 'biademo'. No deletion performed."
    }
} else {
    Write-Output "The package.json file does not exist at $packageJsonPath."
}

# Clean imports
npm run clean

Write-Host "Finish"
pause
