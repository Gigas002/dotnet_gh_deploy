$versionSuffix = "0"
$nugetKey = ""
$githubKey = ""
$src = "Deploy.Core/Deploy.Core.csproj"
$packDir = "publish/Nupkg"
$githubFeedName = "senketsu03"

# Build packages
dotnet build $src -c Release --verbosity quiet

# Pack with needed version suffix
if ($versionSuffix)
{
    dotnet pack $src -c Release -o $packDir --no-build --verbosity quiet
}
else
{
    dotnet pack $src -c Release -o $packDir --no-build --verbosity quiet --version-suffix ci-$versionSuffix
}

# Push to nuget package feed
foreach($file in (Get-ChildItem $packDir -Recurse -Include *.nupkg))
{
    # see: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-nuget-push
    dotnet nuget push $file -k $nugetKey -s https://api.nuget.org/v3/index.json --skip-duplicate

    # for github package feed; no symbols package it seems
    dotnet nuget push $file -k $githubKey -s https://nuget.pkg.github.com/$githubFeedName/index.json --skip-duplicate
}
