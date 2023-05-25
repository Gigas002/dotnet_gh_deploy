<#
.SYNOPSIS
    Script for bundling current repo into .zip file, excluding .gitignore files

.EXAMPLE
    ./script.ps1 -p "publish" -continious-tag "continious" -b "Directory.Build.props"

.LINK
    https://github.com/senketsu03/dotnet_gh_deploy
    https://git-scm.com/docs/git-archive
#>

[CmdletBinding(PositionalBinding = $false)]
param (
    # publish path
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("p", "publish-path")]
    [string] $publishPath = "publish",

    # continious tag
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("continious-tag")]
    [string] $continiousTag = "continious",

    # Directory.Build.props path
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("b", "build-props-path")]
    [string] $buildPropsPath = "Directory.Build.props"
)

#region Constants

Set-Variable ArchiveName -Option ReadOnly -Value "src"

#endregion

Write-Host "Started compressing the repo into .zip..." -ForegroundColor Yellow

$versionPrefix, $versionSuffix, $_, $_ = ./read_version.ps1 -p $buildPropsPath

if ("$versionSuffix") {
    $version = "$continiousTag"
}
else {
    $version = "v$versionPrefix"
}

New-Item -Path "$publishPath" -Type Directory -Force
$output = "$publishPath/$ArchiveName-$version.zip"
git archive HEAD -o "$output" --worktree-attributes -v

Write-Host "Finished compressing the repo into $output.zip" -ForegroundColor Green
