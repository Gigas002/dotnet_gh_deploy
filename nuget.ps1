$versionSuffix = "2"
$apiKey = ""
$src = "Deploy.Core/Deploy.Core.csproj"
$packDir = "publish/Deploy.Core"

dotnet build $src -c Release
dotnet pack $src -c Release -o $packDir --no-build --version-suffix ci-$versionSuffix

foreach($file in (Get-ChildItem $packDir -Recurse -Include *.nupkg))
{
    # see: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-nuget-push
    dotnet nuget push $file -k $apiKey -s https://api.nuget.org/v3/index.json --skip-duplicate

    # for github package feed
    # dotnet nuget push $file -k $apiKey -s https://nuget.pkg.github.com/senketsu03/index.json -ss https://nuget.pkg.github.com/senketsu03/index.json --skip-duplicate
}
