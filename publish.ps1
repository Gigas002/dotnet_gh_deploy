if ($IsWindows)
{
    $r = "win-x64"
    dotnet publish "Deploy.Cli/Deploy.Cli.csproj" -c Release -r $r -o "publish/$r" --sc false
}

if ($IsLinux)
{

}

if ($IsMacOS)
{

}