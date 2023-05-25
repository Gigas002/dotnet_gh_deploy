<#
.DESCRIPTION
    Script for deploying nuget packages

.EXAMPLE
    ./script.ps1 -p "publish" -github-feed-name "name" -i "proj1/proj1.csproj","proj2/proj2.csproj" -b "Directory.Build.props"

.LINK
    https://github.com/senketsu03/dotnet_gh_deploy
#>

[CmdletBinding(PositionalBinding = $false)]
param (
    # Nuget API token
    [Parameter ()]
    [Alias("n", "nuget-token")]
    [SecureString] $nugetToken = (Read-Host "Enter your nuget token" -AsSecureString),

    # Github token
    [Parameter ()]
    [Alias("github-token")]
    [SecureString] $githubToken = (Read-Host "Enter your github token" -AsSecureString),

    # Paths to packages to publish
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [string[]] $inputs = (
        "Deploy.Core/Deploy.Core.csproj",
        "Deploy.Core.Dummy/Deploy.Core.Dummy.csproj"
    ),  

    # Output (dotnet pack) path
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("p", "publish-path")]
    [string] $publishPath = "publish",

    # Github package feed name
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("github-feed-name")]
    [string] $githubFeedName = "senketsu03",

    # Directory.Build.props path
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("b", "build-props-path")]
    [string] $buildPropsPath = "Directory.Build.props"
)

$publishPath = "$publishPath/nupkg"

#region Constants

Set-Variable NugetFeed -Option ReadOnly -Value "https://api.nuget.org/v3/index.json"
Set-Variable GithubFeed -Option ReadOnly -Value "https://nuget.pkg.github.com/$githubFeedName/index.json"

#endregion

#region Functions declartions

function DotnetPack([string] $versionSuffix, [string] $buildVersion) {
    Write-Host "dotnet build/dotnet pack started..." -ForegroundColor Yellow

    foreach ($project in $inputs) {
        Write-Host "Building: $project" -ForegroundColor Yellow

        dotnet build $project /tl -c Release --verbosity quiet

        if ("$versionSuffix") {
            Write-Host "Pack prerelease (build): $buildVersion" -ForegroundColor Yellow
            dotnet pack $project /tl -c Release -o $publishPath --no-build --verbosity quiet --version-suffix ci-$buildVersion
        }
        else {
            Write-Host "Pack release: $versionPrefix" -ForegroundColor Yellow
            dotnet pack $project /tl -c Release -o $publishPath --no-build --verbosity quiet
        }
    }

    Write-Host "dotnet build/dotnet pack finished" -ForegroundColor Green
    Write-Host "packages are ready at: $publishPath" -ForegroundColor Green
}

function DotnetNugetPush([string] $file, [SecureString] $token, [string] $feed) {
    $decryptedToken = ConvertFrom-SecureString $token -AsPlainText

    # see: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-nuget-push
    dotnet nuget push $file -k $decryptedToken -s $feed --skip-duplicate
}

function PushPackages() {
    Write-Host "dotnet nuget push started..." -ForegroundColor Yellow

    foreach ($file in (Get-ChildItem $publishPath -Recurse -Include *.nupkg)) {
        if ($nugetToken -and $nugetToken.Length -gt 0) {
            DotnetNugetPush $file $nugetToken $NugetFeed
        }
        if ($githubToken -and $githubToken.Length -gt 0) {
            DotnetNugetPush $file $githubToken $GithubFeed
        }
    }

    Write-Host "dotnet nuget push finished" -ForegroundColor Green
}

#endregion

$versionPrefix, $versionSuffix, $buildVersion, $_ = ./read_version.ps1 -buildPropsPath $buildPropsPath

DotnetPack $versionSuffix $buildVersion
PushPackages
