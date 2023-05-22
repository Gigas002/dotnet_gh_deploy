#region variables

$publishPath = "publish"

# TODO: array
$projectToPublish = "Deploy.Cli/Deploy.Cli.csproj"

#endregion

#region remove publish directory if it exists

Write-Output "Removing previous publish directory: $publish"
Remove-Item -Path $publishPath -Recurse -Force -ErrorAction SilentlyContinue

#endregion

#region decide runtime

Write-Output "Publishing native binaries for:"

if ($IsWindows)
{
    $rid = "win-x64"
}
elseif ($IsLinux)
{
    $rid = "linux-x64"
}
elseif ($IsMacOS)
{
    $rid = "osx-x64"
}

Write-Output $rid

#endregion

#region run tests

# TODO: args
./test.ps1

#endregion

#region build docs

./docs.ps1

#endregion

#region publish apps

# TODO: add `/tl` after net8 release
dotnet publish "$projectToPublish" -c Release -r $rid -o "$publishPath/$rid" --sc false --verbosity quiet

#endregion

#region publish nupkgs

Write-Output "Publishing nupkgs"

# TODO: args
./nuget.ps1

#endregion

#region cp docs

Write-Output "Copy documents/Remove pdbs"

Copy-Item "*.md" "$publishPath/$rid/"
Remove-Item "$publishPath/$rid/*.pdb"

#endregion

#region zip artifacts

Write-Output "Zip artifacts"

Compress-Archive -Path "$publishPath/$rid/*" -Destination "$publishPath/$rid.zip"

#endregion

#region publish docker

# TODO: args
./docker.ps1

#endregion

Write-Output "Publish finished"
