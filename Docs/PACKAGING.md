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




## Prepare the VSExtension (No more use after V3.4.1)
- In BIATemplate project extract the tempate for all projects:
  - Use Ctrl+E, Ctrl+X to dislay wizard
  - Select the project (should be done for all projects)
  - Click next
  - Uncheck the 2 combo
  - Click Finish
- In *...\BIAVSExtension\BIAProjectCreator\RefreshKit.ps1** check than the path ($RepSource) is correct 
- Launch **...\BIAVSExtension\BIAProjectCreator\RefreshKit.ps1** (it refresh the files in BIAVSExtension\BIAProjectCreator\BIA.ProjectCreatorTemplateV3 from the template extrated)
- Open the solution **..\BIAVSExtension\BIAVSExtension.sln**
- If project have been added in the BIATemplate update **...\BIAVSExtension\BIAProjectCreator\BIA.ProjectCreatorTemplateV3\BIA.vstemplate**
- Change the version number in the name tag of the file **...\BIAVSExtension\BIAProjectCreator\BIA.ProjectCreatorTemplateV3\BIA.vstemplate**
- Change the version number (add a .0 to be on 4 digits) in **...\BIAVSExtension\BIAProjectCreator\BIA.ProjectCreator\source.extension.vsixmanifest**
- Check that all the Additional Files and Docs are added to the project and have the properties: Build action = Content, Include In VSIX = true-. If not verify the BIA.ProjectCreator.csproj if should content:
```XML
  <ItemGroup>
    <Content Include="AdditionalFiles\*" >
      <IncludeInVSIX>true</IncludeInVSIX>
    </Content>
    <Content Include="AdditionalFiles\*\*" >
      <IncludeInVSIX>true</IncludeInVSIX>
    </Content>
    <Content Include="AdditionalFiles\*\*\*" >
      <IncludeInVSIX>true</IncludeInVSIX>
    </Content>
    <Content Include="AdditionalFiles\*\*\*\*" >
      <IncludeInVSIX>true</IncludeInVSIX>
    </Content>
    <Content Include="AdditionalFiles\*\*\*\*\*" >
      <IncludeInVSIX>true</IncludeInVSIX>
    </Content>
    <Content Include="AdditionalFiles\*\*\*\*\*\*">
      <IncludeInVSIX>true</IncludeInVSIX>
    </Content>
    <Content Include="AdditionalFiles\*\*\*\*\*\*\*" >
      <IncludeInVSIX>true</IncludeInVSIX>
    </Content>
    <Content Include="AdditionalFiles\*\*\*\*\*\*\*\*" >
      <IncludeInVSIX>true</IncludeInVSIX>
    </Content>
    <None Include="source.extension.vsixmanifest">
      <SubType>Designer</SubType>
    </None>
  </ItemGroup>
```
- You can test the project BIAProjectCreator in debug ... when it start, create a new project and select BIA V... Template.
- Rebuild solution in release.
- COMMIT the BIAVSExtension solution.

## Store the Templates in BIADemo repository
- In folder ..\BIADemo\Docs\Templates create a new folder VX.Y.Z with (X.Y.Z = version)
- Move the generated file **...\BIADemo\Docs\Templates\BIA.ProjectCreator.vsix** to  **...\BIADemo\Docs\Templates\VX.Y.Z\BIA.ProjectCreator.X.Y.Z.vsix**
- Zip the **...\Templates\Angular** folder (after delete node_modules) to  **...\BIADemo\Docs\Templates\VX.Y.Z\BIA.AngularTemplate.X.Y.Z.zip**
- COMMIT the BIADemo repository.

## Deliver the version
- Set the tag of the version in the 2 repository BIADemo and BIATemplate
- Mail all developer to informe than a new version is available.

## Prepare Migration

- Follow those steps: [PREPARE MIGRATION](./PREPARE%20MIGRATION.md)