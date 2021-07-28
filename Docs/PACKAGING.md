# Package a new version of the Framework:

## Refine the BIADemo project
- In the .Net Part: put comments "// BIADemo only" at the beginning of each file which must not appear in the template
- Put behind comments "// Begin BIADemo" and "// End BIADemo" the parts of the files to make disappear in the template
- Remove all warnings.
- Change the framework version in 
  - **..\BIADemo\DotNet\TheBIADevCompany.BIADemo.Crosscutting.Common\Constants.cs**
  - **..\BIADemo\Angular\src\app\shared\bia-shared\framework-version.ts**
- Verify project version in
  - **..\BIADemo\DotNet\TheBIADevCompany.BIADemo.Crosscutting.Common\Constants.cs**
  - **..\BIADemo\Angular\src\environments\environment.ts**
  - **..\BIADemo\Angular\src\environments\environment.prod.ts**
- COMMIT BIADemo

## Compile the BIA packages:
- Change the version number of all BIA.Net.Core packages to match the version to be released.
- Compile the whole solution in release
- Publish all the packages (right click on each project, publish, "Copy to NuGetPackage folder", Publish)

## Switch the BIADemo project to nuget
- In the file **...\BIADemo\DotNet\Switch-To-Nuget.ps1** adapt the package version number in the line :
    ```
    dotnet add $ProjectFile package BIA.Net.Core.$layerPackage -v 3.4.*
    ```
- Start the script **...\BIADemo\DotNet\Switch-To-Nuget.ps1**
- Check that the solution compiles (need to have configured a local source nuget to ...\BIADemo\BIAPackage\NuGetPackage)
- test the BIADemo project.
- DO NOT COMMIT BIADemo here (this will block the build 24H because the packages are not published on nuget.org)

## Prepare BIATemplate:
- Launch **...\BIATemplate\DotNet-BIADemo-BIATemplate.ps1**
- Launch **...\BIATemplate\Angular-BIADemo-BIATemplate.ps1** (if some files are to exclude modify the script)
- Compile the solution BIATemplate, Test and verify the absence of warning.
- DO NOT COMMIT BIATemplate here (this will block the build 24H because the packages are not published on nuget.org)

## Prepare BIACompany Files:
- Launch **...\BIACompanyFiles\Tools\CopySettingsFromBIATemplateCompanyFiles.ps1**

## Test the project creation using the VX.Y.Z
- With the BIAToolKit create a project of the VX.Y.Z.
- Test it.
- If is is ok rename **...\BIACompanyFiles\VX.Y.Z** and **...\BIADemo\Docs\Templates\VX.Y.Z** with the good version name 

## Publish BIAPackage
- If everything is ok Publish the packages on nuget.org
- Wait the confirmation by mail of all packages
- COMMIT BIADemo, BIACompanyFiles and BIATemplate

## Publish the demo site:
- Launch in VSCode in **...BIADemo\Angular** folder:
```
npm run deploy
```

## Deliver the version
- Create a release of the version in the 3 repository BIADocs, BIADemo and BIATemplate
- Mail all developer to informe than a new version is available.

## Prepare Migration
- Follow those steps: [PREPARE MIGRATION](./PREPARE%20MIGRATION.md)