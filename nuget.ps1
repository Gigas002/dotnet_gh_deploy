<#
.DESCRIPTION
    Script for publishing nuget packages

.EXAMPLE
    ./script.ps1 -o "publish/nupkg" -g "name" -i "proj1/proj1.csproj","proj2/proj2.csproj"

.LINK
    https://github.com/senketsu03/dotnet_gh_deploy
#>

[CmdletBinding(PositionalBinding = $false)]
param (
    # Nuget API token
    [Parameter (Mandatory = $true)]
    [SecureString] $nugetToken = (Read-Host "Enter your nuget token" -AsSecureString),

    # Github token
    [Parameter (Mandatory = $true)]
    [SecureString] $githubToken = (Read-Host "Enter your github token" -AsSecureString),

    # Paths to projects to publish
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [string[]] $inputs = ("Deploy.Core/Deploy.Core.csproj"),  

    # Output (dotnet pack) path
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [string] $output = "publish/nupkg",

    # Github package feed name
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("g", "github-feed-name")]
    [string] $githubFeedName = "senketsu03"
)

#region Constants

Set-Variable NugetFeed -Option ReadOnly -Value "https://api.nuget.org/v3/index.json"
Set-Variable GithubFeed -Option ReadOnly -Value "https://nuget.pkg.github.com/$githubFeedName/index.json"

#endregion

#region Functions declartions

function DotnetPack([string] $versionSuffix, [string] $buildVersion) {
    Write-Output "dotnet build/dotnet pack started"

    foreach ($project in $inputs) {
        Write-Output "Building: $project"

        dotnet build $project /tl -c Release --verbosity quiet

        if ($versionSuffix) {
            Write-Output "Pack prerelease: $buildVersion"
            dotnet pack $project /tl -c Release -o $output --no-build --verbosity quiet --version-suffix ci-$buildVersion
        }
        else {
            Write-Output "Pack release: $versionPrefix"
            dotnet pack $project /tl -c Release -o $output --no-build --verbosity quiet
        }
    }

    Write-Output "dotnet build/dotnet pack finished"
    Write-Output "packages are ready at: $output"
}

function DotnetNugetPush([string] $file, [SecureString] $token, [string] $feed) {
    $decryptedToken = ConvertFrom-SecureString $token -AsPlainText

    # see: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-nuget-push
    dotnet nuget push $file -k $decryptedToken -s $feed --skip-duplicate
}

function PushPackages() {
    Write-Output "dotnet nuget push started"

    foreach ($file in (Get-ChildItem $output -Recurse -Include *.nupkg)) {
        DotnetNugetPush $file $nugetToken $NugetFeed
        DotnetNugetPush $file $githubToken $GithubFeed
    }

    Write-Output "dotnet nuget push finished"
}

#endregion

$versionPrefix, $versionSuffix, $buildVersion, $dockerTag = ./read_version.ps1

DotnetPack $versionSuffix $buildVersion
PushPackages
