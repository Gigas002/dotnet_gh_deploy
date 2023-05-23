<#
.DESCRIPTION
    Script for deploying nuget packages

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
    [string] $githubFeedName = "senketsu03",

    # Directory.Build.props path
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("p")]
    [string] $buildPropsPath = "Directory.Build.props"
)

#region Constants

Set-Variable NugetFeed -Option ReadOnly -Value "https://api.nuget.org/v3/index.json"
Set-Variable GithubFeed -Option ReadOnly -Value "https://nuget.pkg.github.com/$githubFeedName/index.json"

#endregion

#region Functions declartions

function DotnetPack([string] $versionSuffix, [string] $buildVersion) {
    Write-Host "dotnet build/dotnet pack started" -ForegroundColor Yellow

    foreach ($project in $inputs) {
        Write-Host "Building: $project" -ForegroundColor Yellow

        dotnet build $project /tl -c Release --verbosity quiet

        if ("$versionSuffix") {
            Write-Host "Pack prerelease (build): $buildVersion" -ForegroundColor Yellow
            dotnet pack $project /tl -c Release -o $output --no-build --verbosity quiet --version-suffix ci-$buildVersion
        }
        else {
            Write-Host "Pack release: $versionPrefix" -ForegroundColor Yellow
            dotnet pack $project /tl -c Release -o $output --no-build --verbosity quiet
        }
    }

    Write-Host "dotnet build/dotnet pack finished" -ForegroundColor Yellow
    Write-Host "packages are ready at: $output" -ForegroundColor Yellow
}

function DotnetNugetPush([string] $file, [SecureString] $token, [string] $feed) {
    $decryptedToken = ConvertFrom-SecureString $token -AsPlainText

    # see: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-nuget-push
    dotnet nuget push $file -k $decryptedToken -s $feed --skip-duplicate
}

function PushPackages() {
    Write-Host "dotnet nuget push started" -ForegroundColor Yellow

    foreach ($file in (Get-ChildItem $output -Recurse -Include *.nupkg)) {
        DotnetNugetPush $file $nugetToken $NugetFeed
        DotnetNugetPush $file $githubToken $GithubFeed
    }

    Write-Host "dotnet nuget push finished" -ForegroundColor Yellow
}

#endregion

$versionPrefix, $versionSuffix, $buildVersion, $dockerTag = ./read_version.ps1 -p $buildPropsPath

DotnetPack $versionSuffix $buildVersion
PushPackages
