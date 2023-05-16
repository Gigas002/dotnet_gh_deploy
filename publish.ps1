echo "Publishing native binaries for:"

if ($IsWindows)
{
    $r = "win-x64"
}
elseif ($IsLinux)
{
    $r = "linux-x64"
}
elseif ($IsMacOS)
{
    $r = "osx-x64"
}

echo $r
dotnet publish "Deploy.Cli/Deploy.Cli.csproj" -c Release -r $r -o "publish/$r" --sc false

echo "Publish finished"
