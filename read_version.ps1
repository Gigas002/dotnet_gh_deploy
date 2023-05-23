<#
.DESCRIPTION
    Script to read project version from Directory.Build.props file

.EXAMPLE
    ./script.ps1 -p "Directory.Build.props"

.OUTPUTS
    tuple: versionPrefix, versionSuffix, buildVersion, dockerTag

.LINK
    https://github.com/senketsu03/dotnet_gh_deploy
#>

[CmdletBinding(PositionalBinding = $false)]
param (
    # Directory.Build.props path
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("p")]
    [string] $path = "Directory.Build.props"
)

#region Constants

Set-Variable VersionPrefixPath -Option ReadOnly -Value "/Project/PropertyGroup/VersionPrefix"
Set-Variable VersionSuffixPath -Option ReadOnly -Value "/Project/PropertyGroup/VersionSuffix"
Set-Variable AssemblyVersionPath -Option ReadOnly -Value "/Project/PropertyGroup/AssemblyVersion"

#endregion

#region Read version

$versionPrefix = (Select-Xml -Path $path -XPath $VersionPrefixPath).Node.InnerText
$versionSuffix = (Select-Xml -Path $path -XPath $VersionSuffixPath).Node.InnerText
$assemblyVersion = (Select-Xml -Path $path -XPath $AssemblyVersionPath).Node.InnerText
$buildVersion = $assemblyVersion.Split('.')[-1]
$dockerTag = ""

if ("$versionSuffix") {
    $dockerTag = "latest"
}
else {
    $dockerTag = "v$versionPrefix"
}

#endregion

return $versionPrefix, $versionSuffix, $buildVersion, $dockerTag
