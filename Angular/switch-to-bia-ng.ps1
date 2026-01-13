$SourceFrontEnd = "."

# Prompt user whether to use published npm package or local dist package
$userChoice = Read-Host "Use npm package @bia-team/bia-ng instead of local dist package? (y/N)"
if ($userChoice -match '^[yY]') {
  $UseNpmPackage = $true
}
else {
  $UseNpmPackage = $false
}
$ExcludeDir = ('dist', 'node_modules', 'docs', 'scss', '.git', '.vscode', '.angular', '.dart_tool', '.bia')

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

$projectPackageJsonPath = "package.json"

if (Test-Path -Path $projectPackageJsonPath -PathType Leaf) {
  # Read the package.json file
  $projectPackageJsonContent = Get-Content -Path $projectPackageJsonPath -Raw | ConvertFrom-Json
  
  # Switch imports to bia-ng imports
  ReplaceInProject -Source $SourceFrontEnd -OldRegexp "(import\s*{\s*[^}]+\s*}\s*from\s*)[']packages\/(bia-ng\/[^']+)\/public-api['];" -NewRegexp '$1''@bia-team/$2'';' -Include *.ts
  # if ($UseNpmPackage) {
  ReplaceInProject -Source $SourceFrontEnd -OldRegexp "((templateUrl:|styleUrls:\s*\[|styleUrl:)[\s]*'[\S]*\/)packages\/bia-ng\/shared\/(\S*')" -NewRegexp '$1node_modules/@bia-team/bia-ng/templates/$3' -Include *.ts
  # Also update references inside styles (SCSS/CSS) from local package to installed package
  ReplaceInProject -Source $SourceFrontEnd -OldRegexp "packages\/bia-ng\/scss" -NewRegexp 'node_modules/@bia-team/bia-ng/styles' -Include *.scss
  ReplaceInProject -Source $SourceFrontEnd -OldRegexp "packages\/bia-ng\/scss" -NewRegexp 'node_modules/@bia-team/bia-ng/styles' -Include *.css
  ReplaceInProject -Source $SourceFrontEnd -OldRegexp "packages\/bia-ng\/scss" -NewRegexp 'node_modules/@bia-team/bia-ng/styles' -Include *angular.json
  # }
  # else {
  #   ReplaceInProject -Source $SourceFrontEnd -OldRegexp "((templateUrl:|styleUrls:\s*\[|styleUrl:)[\s]*'[\S]*\/)packages\/bia-ng\/shared\/(\S*')" -NewRegexp '$1dist/bia-ng/templates/$3' -Include *.ts 
  #   # Also update references inside styles (SCSS/CSS) from local package to installed package
  #   ReplaceInProject -Source $SourceFrontEnd -OldRegexp "packages\/bia-ng\/scss" -NewRegexp 'dist/bia-ng/styles' -Include *.scss
  #   ReplaceInProject -Source $SourceFrontEnd -OldRegexp "packages\/bia-ng\/scss" -NewRegexp 'dist/bia-ng/styles' -Include *.css
  #   ReplaceInProject -Source $SourceFrontEnd -OldRegexp "packages\/bia-ng\/scss" -NewRegexp 'dist/bia-ng/styles' -Include *angular.json
  # }

  $frameworkVersionFile = Join-Path -Path $SourceFrontEnd -ChildPath "packages\bia-ng\shared\framework-version.ts"

  # Extract the version from framework-version.ts
  $frameworkVersionContent = Get-Content -Path $frameworkVersionFile -Raw
  $frameworkVersionMatch = [regex]::Match($frameworkVersionContent, 'FRAMEWORK_VERSION\s*=\s*''([\S]+)''')
  $frameworkVersion = $frameworkVersionMatch.Groups[1].Value

  Write-Output "Bia Demo framework version : $frameworkVersion"

  if ($null -eq $projectPackageJsonContent.dependencies) {
    $projectPackageJsonContent.dependencies = @{}
  }

  # Check if the "bia-ng" dependency does not exist
  if (-not ($projectPackageJsonContent.dependencies.PSObject.Properties | Where-Object { $_.Name -eq "bia-ng" })) {
    # Initialize dependencies as an ordered dictionary if it is null
    if ($null -eq $projectPackageJsonContent.dependencies) {
      $projectPackageJsonContent.dependencies = [ordered]@{}
    }

    # Convert dependencies to a dictionary for easier manipulation
    $dependencies = [ordered]@{}
    $projectPackageJsonContent.dependencies.PSObject.Properties | ForEach-Object {
      $dependencies[$_.Name] = $_.Value
    }

    # Create a new ordered dictionary to hold the dependencies in alphabetical order
    $orderedDependencies = [ordered]@{}

    # Find the correct position to insert "bia-ng" in alphabetical order
    $inserted = $false

    foreach ($dep in ($dependencies.Keys | Sort-Object)) {
      if (-not $inserted -and "@bia-team/bia-ng" -lt $dep) {
        if ($UseNpmPackage) {
          $orderedDependencies["@bia-team/bia-ng"] = $frameworkVersion
        }
        else {
          $orderedDependencies["@bia-team/bia-ng"] = 'file:./dist/bia-ng'
        }
        $inserted = $true
      }
      $orderedDependencies[$dep] = $dependencies[$dep]
    }

    # If "bia-ng" hasn't been inserted yet, it should be added at the end
    if (-not $inserted) {
      if ($UseNpmPackage) {
        $orderedDependencies["@bia-team/bia-ng"] = $frameworkVersion
      }
      else {
        $orderedDependencies["@bia-team/bia-ng"] = 'file:./dist/bia-ng'
      }
    }

    # Update the dependencies in the package.json content
    $projectPackageJsonContent.dependencies = $orderedDependencies

    # Convert the object back to JSON and write it back to the package.json file
    $projectPackageJsonContent | ConvertTo-Json -Depth 10 | Set-Content -Path $projectPackageJsonPath

    Write-Output "The '@bia-team/bia-ng' dependency has been added with version $frameworkVersion in package.json."
  }
  else {
    Write-Output "The '@bia-team/bia-ng' dependency already exists in package.json."
  }
}

# Define the path to the folder you want to delete
# $folderPath = Join-Path -Path "." -ChildPath "packages\bia-ng"

# if (Test-Path -Path $projectPackageJsonPath -PathType Leaf) {
#   # Read the package.json file
#   $projectPackageJsonContent = Get-Content -Path $projectPackageJsonPath -Raw | ConvertFrom-Json

#   # Check user's choice: if using npm package, remove local `packages\bia-ng` folder
#   if ($UseNpmPackage) {
#     # Check if the folder exists
#     if (Test-Path -Path $folderPath) {
#       # Delete the folder and all its contents
#       Remove-Item -Path $folderPath -Recurse -Force
#       Write-Output "The folder $folderPath has been deleted successfully."
#     }
#     else {
#       Write-Output "The folder $folderPath does not exist."
#     }
#   }
#   else {
#     Write-Output "Local 'packages\bia-ng' preserved (using local dist package)."
#   }
# }
# else {
#   Write-Output "The package.json file does not exist at $packageJsonPath."
# }

# Clean imports
npm run clean

Write-Host "Finish"

pause