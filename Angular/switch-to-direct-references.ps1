$SourceFrontEnd = "."
$SourceBiaDemo = ".."

$sourcePath = Join-Path -Path $SourceBiaDemo -ChildPath "Angular\packages"
$destinationPath = $SourceFrontEnd

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

$projectPackageJsonPath = Join-Path -Path $SourceFrontEnd -ChildPath "package.json"

if (Test-Path -Path $projectPackageJsonPath -PathType Leaf) {
  # Read the package.json file
  $projectPackageJsonContent = Get-Content -Path $projectPackageJsonPath -Raw | ConvertFrom-Json
  # Check if the project is using a file: dependency for @bia-team/bia-ng
  $biaNgIsFile = $false
  if ($projectPackageJsonContent.dependencies -and $projectPackageJsonContent.dependencies.PSObject.Properties.Name -contains "@bia-team/bia-ng") {
    $biaNgVersionSpec = $projectPackageJsonContent.dependencies."@bia-team/bia-ng"
    if ($biaNgVersionSpec -like 'file:*') { $biaNgIsFile = $true }
  }
  if (-not $biaNgIsFile) {
    # Check if the source path exists
    if (Test-Path -Path $sourcePath) {
      # Path to the framework-version.ts file in the source folder
      $frameworkVersionFile = Join-Path -Path $sourcePath -ChildPath "bia-ng\shared\framework-version.ts"

      # Check if both files exist
      if ((Test-Path -Path $frameworkVersionFile -PathType Leaf) -And (Test-Path -Path $projectPackageJsonPath -PathType Leaf)) {
        # Extract the version from framework-version.ts
        $frameworkVersionContent = Get-Content -Path $frameworkVersionFile -Raw
        $frameworkVersionMatch = [regex]::Match($frameworkVersionContent, 'FRAMEWORK_VERSION\s*=\s*''([\S]+)''')
        $frameworkVersion = $frameworkVersionMatch.Groups[1].Value
        Write-Output "Bia Demo framework version: $frameworkVersion"

        # Extract the version of "bia-ng" from package.json
        $biaNgVersion = $projectPackageJsonContent.dependencies."@bia-team/bia-ng"
        Write-Output "Currently used bia-ng version: $biaNgVersion"

        # Compare the versions
        if ($frameworkVersion -eq $biaNgVersion) {
          # Copy the contents of the source path to the destination path
          Copy-Item -Path "$sourcePath\*" -Destination "$destinationPath\packages\bia-ng" -Recurse -Force
          Write-Output "Versions match. Copy of local bia-ng completed successfully."
        }
        else {
          Write-Output "Versions do not match. No copy operation performed."
          Write-Host "Finish"
          pause
          exit
        }
      }
      else {
        Write-Output "Cannot find bi-ng files. Check the path to your BIA Demo sources for the variable SourceBiaDemo"
        Write-Host "Finish"
        pause
        exit
      }
    }
    else {
      Write-Output "Source path does not exist: $sourcePath"
      Write-Host "Finish"
      pause
      exit
    }
  }

  # Switch imports to bia-ng imports
  ReplaceInProject -Source $SourceFrontEnd -OldRegexp "(import\s*{\s*[^}]+\s*}\s*from\s*)[']@bia-team\/(bia-ng\/[^']+)['];" -NewRegexp '$1''packages/$2/public-api'';' -Include *.ts
  # if ($biaNgIsFile) {
  #   ReplaceInProject -Source $SourceFrontEnd -OldRegexp "((templateUrl:|styleUrls:\s*\[|styleUrl:)[\s]*'[\S]*\/)dist\/bia-ng\/templates\/([\S]*')" -NewRegexp '$1packages/bia-ng/shared/$3' -Include *.ts 
  #   # Also update references inside styles (SCSS/CSS) from installed package back to local package
  #   ReplaceInProject -Source $SourceFrontEnd -OldRegexp "dist\/bia-ng\/styles" -NewRegexp 'packages/bia-ng/scss' -Include *.scss
  #   ReplaceInProject -Source $SourceFrontEnd -OldRegexp "dist\/bia-ng\/styles" -NewRegexp 'packages/bia-ng/scss' -Include *.css
  #   ReplaceInProject -Source $SourceFrontEnd -OldRegexp "dist\/bia-ng\/styles" -NewRegexp 'packages/bia-ng/scss' -Include *angular.json
  # }
  # else { 
  ReplaceInProject -Source $SourceFrontEnd -OldRegexp "((templateUrl:|styleUrls:\s*\[|styleUrl:)[\s]*'[\S]*\/)node_modules\/@bia-team\/bia-ng\/templates\/([\S]*')" -NewRegexp '$1packages/bia-ng/shared/$3' -Include *.ts
  # Also update references inside styles (SCSS/CSS) from installed package back to local package
  ReplaceInProject -Source $SourceFrontEnd -OldRegexp "node_modules\/@bia-team\/bia-ng\/styles" -NewRegexp 'packages/bia-ng/scss' -Include *.scss
  ReplaceInProject -Source $SourceFrontEnd -OldRegexp "node_modules\/@bia-team\/bia-ng\/styles" -NewRegexp 'packages/bia-ng/scss' -Include *.css
  ReplaceInProject -Source $SourceFrontEnd -OldRegexp "node_modules\/@bia-team\/bia-ng\/styles" -NewRegexp 'packages/bia-ng/scss' -Include *angular.json
  # }

  # Remove the "bia-ng" key from package.json
  if ($projectPackageJsonContent.dependencies.PSObject.Properties.Name -contains "@bia-team/bia-ng") {
    $projectPackageJsonContent.dependencies.PSObject.Properties.Remove("@bia-team/bia-ng")
    $projectPackageJsonContent | ConvertTo-Json -Depth 10 | Set-Content -Path $projectPackageJsonPath
    Write-Output "The '@bia-team/bia-ng' dependency has been removed from package.json."
  }

  # Clean imports
  npm run clean
}


Write-Host "Finish"

pause