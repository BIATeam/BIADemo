$parentDirectory = Split-Path (Split-Path -Parent $PSScriptRoot)
$parentDirectory = Join-Path -Path $parentDirectory -ChildPath "DotNet"
$csprojFiles = Get-ChildItem -Path $parentDirectory -Filter "TheBIADevCompany*.csproj" -Recurse

foreach ($file in $csprojFiles) {
    [xml]$content = Get-Content -Path $file.FullName

    $nodeToFind = $content.Project.ItemGroup | Where-Object { $_.Label -eq "Bia_ItemGroup_BIA_FRONT_FEATURE" }

    $data = @'
    <ItemGroup Label="Bia_ItemGroup_BIA_FRONT_FEATURE" Condition="!$([System.Text.RegularExpressions.Regex]::IsMatch('$(DefineConstants)', '\bBIA_FRONT_FEATURE\b'))">
    <!--BIADEMO-->
<Compile Remove="**\*Aircraft*.cs" />
<Compile Remove="**\*Airport*.cs" />
<Compile Remove="**\*BiaDemo*.cs" />
<Compile Remove="**\*Engine*.cs" />
<Compile Remove="**\*Example*.cs" />
<Compile Remove="**\*Plane*.cs" />
<Compile Remove="**\*Worker*.cs" />
<Compile Remove="**\*HangfiresController*.cs" />
<!--BIATEMPLATE-->
<Compile Remove="**\*Audit*.cs" />
<Compile Remove="**\*Error*.cs" />
<Compile Remove="**\*IdentityProvider*.cs" />
<Compile Remove="**\*LogsController*.cs" />
<Compile Remove="**\*Mapper*.cs" />
<Compile Remove="**\*Member*.cs" />
<Compile Remove="**\*ModelBuilder*.cs" />
<Compile Remove="**\*Notification*.cs" />
<Compile Remove="**\*Query*.cs" />
<Compile Remove="**\*SearchExpressionService*.cs" />
<Compile Remove="**\*Site*.cs" />
<Compile Remove="**\*Synchronize*.cs" />
<Compile Remove="**\*Team*.cs" />
<Compile Remove="**\*Translation*.cs" />
<Compile Remove="**\*View*.cs" />
<Compile Remove="**\Role.cs" />
<Compile Remove="**\RoleAppService.cs" />
<Compile Remove="**\IRoleAppService.cs" />
<Compile Remove="**\RolesController.cs" />
<Compile Remove="**\User.cs" />
<Compile Remove="**\UsersController.cs" />
<Compile Remove="**\UserAppService.cs" />
<Compile Remove="**\IUserAppService.cs" />
<Compile Remove="**\UserExtensions.cs" />
<Compile Remove="**\UserSelectBuilder.cs" />
<Compile Remove="**\UserSpecification.cs" />
<Compile Remove="**\WakeUpTask.cs" />
<Compile Remove="**\LanguagesController.cs" />
<Compile Remove="**\LdapDomainsController.cs" />
</ItemGroup>
'@

    $newNode = New-Object System.Xml.XmlDocument
    $newNode.LoadXml($data);
    
    if ($nodeToFind) {
        $nodeToFind.ParentNode.ReplaceChild($content.ImportNode($newNode.DocumentElement, $true), $nodeToFind)
    }
    else {
        $content.Project.AppendChild($content.ImportNode($newNode.DocumentElement, $true));
    }

    $content.Save($file.FullName)

    # $Settings = New-Object System.Xml.XmlWriterSettings
    # $Settings.Indent = $true
    # $Settings.OmitXmlDeclaration = $true
    # $Settings.Encoding = [System.Text.Encoding]::UTF8

    # $Writer = [System.Xml.XmlWriter]::Create($file.FullName, $Settings)
    # $content.Save($Writer)
    # $Writer.Flush()
    # $Writer.Close()
}
