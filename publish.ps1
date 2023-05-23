<#
.DESCRIPTION
    Script for publishing applications. Requires .sln and Directory.Build.props

.EXAMPLE
    ./script.ps1 -o "publish" -i "proj1/proj1.csproj","proj2/proj2.csproj"

.LINK
    https://github.com/senketsu03/dotnet_gh_deploy
#>

[CmdletBinding(PositionalBinding = $false)]
param (
    # Output (publish) path
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [string] $output = "publish",

    # Paths to projects to publish
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [string[]] $inputs = ("Deploy.Cli/Deploy.Cli.csproj",
                          "Deploy.Benchmarks/Deploy.Benchmarks.csproj",
                          "Deploy.Gui/Deploy.Gui.csproj")
)

#region Functions

function DotnetPublish([string] $project, [string] $publishPath, [string] $rid) {
    Write-Host "Publishing $project into $publishPath..." -ForegroundColor Yellow

    dotnet publish /tl "$project" -c Release -r $rid -o "$publishPath" --sc false --verbosity quiet

    Write-Host "Publishing $project finished" -ForegroundColor Green
}

function CopyDocs([string] $publishPath) {
    Write-Host "Copying docs into $publishPath..." -ForegroundColor Yellow

    Copy-Item "*.md" "$publishPath/"
    Remove-Item "$publishPath/*.pdb"

    Write-Host "Finished copying docs" -ForegroundColor Green
}

function ZipArtifacts([string] $project, [string] $publishPath, [string] $rid) {
    Write-Host "Zip artifacts for $publishPath..." -ForegroundColor Yellow

    Compress-Archive -Path "$publishPath/*" -Destination "$output/$project/$project_$rid.zip"
    
    Write-Host "Finished zip artifacts" -ForegroundColor Green
}

#endregion

Write-Host "Removing previous publish directory: $output" -ForegroundColor Yellow
Remove-Item -Path $output -Recurse -Force -ErrorAction SilentlyContinue

switch ($true) {
    $IsWindows { $rid = "win-x64"; break }
    $IsLinux { $rid = "linux-x64"; break }
    $IsMacOS { $rid = "osx-x64"; break }
}  

Write-Host "Publishing native binaries for: $rid" -ForegroundColor Yellow

foreach ($project in $inputs) {
    $projectName = Split-Path -Path $project -Leaf -Resolve | Split-Path -LeafBase
    $publishPath = "$output/$projectName/$rid"
    DotnetPublish $project $publishPath $rid
    CopyDocs $publishPath
    ZipArtifacts $projectName $publishPath $rid
}

Write-Host "Publish finished" -ForegroundColor Green
