# $DEPLOYDB_answer = Read-Host "Do you want the feature DEPLOYDB ? (Y/N)"
# $HANGFIRE_answer = Read-Host "Do you want the feature HANGFIRE ? (Y/N)"
$SERVICE_API_answer = Read-Host "Do you want the feature BACK_TO_BACK_AUTH ? (Y/N)"
$FRONT_FEATURE_answer = Read-Host "Do you want the feature FRONT_FEATURE ? (Y/N)"
$USE_DATABASE_answer = Read-Host "Do you want the feature BIA_USE_DATABASE ? (Y/N)"
$USER_CUSTOM_FIELDS_BACK_answer = Read-Host "Do you want the feature USER_CUSTOM_FIELDS_BACK ? (Y/N)"
$USER_CUSTOM_FIELDS_FRONT_answer = Read-Host "Do you want the feature USER_CUSTOM_FIELDS_FRONT ? (Y/N)"

$selected_features = @()

# if ($DEPLOYDB_answer -eq 'Y') {
#     $selected_features += 'BIA_DEPLOYDB'
# }

# if ($HANGFIRE_answer -eq 'Y') {
#     $selected_features += 'BIA_HANGFIRE'
# }

if ($SERVICE_API_answer -eq 'Y') {
    $selected_features += 'BIA_BACK_TO_BACK_AUTH'
}

if ($FRONT_FEATURE_answer -eq 'Y') {
    $selected_features += 'BIA_FRONT_FEATURE'
}

if ($USE_DATABASE_answer -eq 'Y') {
    $selected_features += 'BIA_USE_DATABASE'
}

if ($USER_CUSTOM_FIELDS_BACK_answer -eq 'Y') {
    $selected_features += 'BIA_USER_CUSTOM_FIELDS_BACK'
}

if ($USER_CUSTOM_FIELDS_FRONT_answer -eq 'Y') {
    $selected_features += 'BIA_USER_CUSTOM_FIELDS_FRONT'
}


$features_list = $selected_features -join ';'

Write-Host "You have chosen the following features: $features_list"

$parentDirectory = Split-Path (Split-Path -Parent $PSScriptRoot)
$parentDirectory = Join-Path -Path $parentDirectory -ChildPath "DotNet"
$csprojFiles = Get-ChildItem -Path $parentDirectory -Filter "TheBIADevCompany*.csproj" -Recurse

foreach ($file in $csprojFiles) {

    [xml]$content = Get-Content -Path $file.FullName

    $nodeToFind = $content.Project.PropertyGroup | Where-Object { $_.Condition -eq '''$(Configuration)|$(Platform)''==''Debug|AnyCPU''' }

    if ($null -eq $nodeToFind) {
        $nodeToFind = $content.CreateElement("PropertyGroup")
        $nodeToFind.SetAttribute('Condition', '''$(Configuration)|$(Platform)''==''Debug|AnyCPU''')
        $defineConstants = $content.CreateElement("DefineConstants")
        $nodeToFind.AppendChild($defineConstants)
        $content.Project.AppendChild($nodeToFind)
    }
    
    $nodeToFind.DefineConstants = '$(DefineConstants);' + $features_list
    
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
