#region variables

$publishPath = "publish"

# TODO: array
$projectToPublish = "Deploy.Cli/Deploy.Cli.csproj"

#endregion

#region remove publish directory if it exists

echo "Removing previous publish directory: $publish"
Remove-Item -Path $publishPath -Recurse -Force -ErrorAction SilentlyContinue

#endregion

#region decide runtime

echo "Publishing native binaries for:"

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

echo $rid

#endregion

#region publish apps

# TODO: add /tl after net8 release
dotnet publish "$projectToPublish" -c Release -r $rid -o "$publishPath/$rid" --sc false --verbosity quiet

#endregion

#region publish nupkgs

echo "Publishing nupkgs"

# TODO: args
./nuget.ps1

#endregion

#region cp docs

echo "Copy documents/Remove pdbs"

cp "*.md" "$publishPath/$rid/"
rm "$publishPath/$rid/*.pdb"

#endregion

#region zip artifacts

echo "Zip artifacts"

Compress-Archive -Path "$publishPath/$rid/*" -Destination "$publishPath/$rid.zip"

#endregion

echo "Publish finished"
