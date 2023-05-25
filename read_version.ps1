<#
.DESCRIPTION
    Script to read project version from Directory.Build.props file

.EXAMPLE
    ./script.ps1 -b "Directory.Build.props" -docker-continious-tag "latest"

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
    [Alias("b", "build-props-path")]
    [string] $buildPropsPath = "Directory.Build.props",

    # docker continious tag
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("docker-continious-tag")]
    [string] $dockerContiniousTag = "latest"
)

#region Constants

Set-Variable VersionPrefixPath -Option ReadOnly -Value "/Project/PropertyGroup/VersionPrefix"
Set-Variable VersionSuffixPath -Option ReadOnly -Value "/Project/PropertyGroup/VersionSuffix"
Set-Variable AssemblyVersionPath -Option ReadOnly -Value "/Project/PropertyGroup/AssemblyVersion"

#endregion

#region Read version

$versionPrefix = (Select-Xml -Path $buildPropsPath -XPath $VersionPrefixPath).Node.InnerText
$versionSuffix = (Select-Xml -Path $buildPropsPath -XPath $VersionSuffixPath).Node.InnerText
$assemblyVersion = (Select-Xml -Path $buildPropsPath -XPath $AssemblyVersionPath).Node.InnerText
$buildVersion = $assemblyVersion.Split('.')[-1]
$dockerTag = ""

if ("$versionSuffix") {
    $dockerTag = "$dockerContiniousTag"
}
else {
    $dockerTag = "v$versionPrefix"
}

#endregion

Write-Host "prefix: $versionPrefix, suffix: $versionSuffix, build: $buildVersion, docker: $dockerTag" -ForegroundColor Yellow

return $versionPrefix, $versionSuffix, $buildVersion, $dockerTag
