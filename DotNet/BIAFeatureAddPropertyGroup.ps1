$DEPLOYDB_answer = Read-Host "Do you want the feature DEPLOYDB ? (Y/N)"
$HANGFIRE_answer = Read-Host "Do you want the feature HANGFIRE ? (Y/N)"
$FRONT_FEATURE_answer = Read-Host "Do you want the feature FRONT_FEATURE ? (Y/N)"

$selected_features = @()

if ($DEPLOYDB_answer -eq 'Y') {
    $selected_features += 'BIA_DEPLOYDB'
}

if ($HANGFIRE_answer -eq 'Y') {
    $selected_features += 'BIA_HANGFIRE'
}

if ($FRONT_FEATURE_answer -eq 'Y') {
    $selected_features += 'BIA_FRONT_FEATURE'
}

$features_list = $selected_features -join ';'

Write-Host "You have chosen the following features: $features_list"

$parentDirectory = Split-Path -Parent $PSScriptRoot
$csprojFiles = Get-ChildItem -Path $parentDirectory -Filter "TheBIADevCompany*.csproj" -Recurse

foreach ($file in $csprojFiles) {

    [xml]$content = Get-Content -Path $file.FullName

    $nodeToFind = $content.Project.PropertyGroup | Where-Object { $_.Condition -eq '''$(Configuration)|$(Platform)''==''Debug|AnyCPU''' }

    if ($null -ne $nodeToFind.Condition) {
        $nodeToFind = $content.Project.CreateElement("PropertyGroup")
        $nodeToFind.SetAttribute('Condition', '''$(Configuration)|$(Platform)''==''Debug|AnyCPU''')
    }
    
    $nodeToFind.DefineConstants = '$(DefineConstants);' + $features_list
    $content.Save($file.FullName)
}