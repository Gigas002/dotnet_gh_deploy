<#
.DESCRIPTION
    Script for publishing applications. Requires .sln and Directory.Build.props

.EXAMPLE
    ./script.ps1 -p "publish" -i "proj1/proj1.csproj","proj2/proj2.csproj"

.LINK
    https://github.com/senketsu03/dotnet_gh_deploy
#>

[CmdletBinding(PositionalBinding = $false)]
param (
    # Output (publish) path
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("p", "publish-path")]
    [string] $publishPath = "publish",

    # Paths to projects to publish
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [string[]] $inputs = ("Deploy.Cli/Deploy.Cli.csproj",
                          "Deploy.Benchmarks/Deploy.Benchmarks.csproj",
                          "Deploy.Gui/Deploy.Gui.csproj",
                          "Deploy.Server/Deploy.Server.csproj",
                          "Deploy.Client/Deploy.Client.csproj")
)

#region Functions

function DotnetPublish([string] $project, [string] $pub, [string] $rid) {
    Write-Host "Publishing $project into $pub..." -ForegroundColor Yellow

    dotnet publish /tl "$project" -c Release -r $rid -o "$pub" --sc false --verbosity quiet

    Write-Host "Publishing $project finished" -ForegroundColor Green
}

function CopyDocs([string] $pub) {
    Write-Host "Copying docs into $pub..." -ForegroundColor Yellow

    Copy-Item "*.md" "$pub/"
    Remove-Item "$pub/*.pdb"

    Write-Host "Finished copying docs" -ForegroundColor Green
}

function ZipArtifacts([string] $project, [string] $pub, [string] $rid) {
    Write-Host "Zip artifacts for $pub..." -ForegroundColor Yellow

    $artifactPath = "$publishPath/${project}_${rid}.zip"
    Remove-Item -Path $artifactPath -Force -ErrorAction SilentlyContinue
    Compress-Archive -Path "$pub/*" -Destination "$artifactPath"
    
    Write-Host "Finished zip artifacts" -ForegroundColor Green
}

#endregion

switch ($true) {
    $IsWindows { $rid = "win-x64"; break }
    $IsLinux { $rid = "linux-x64"; break }
    $IsMacOS { $rid = "osx-x64"; break }
}  

Write-Host "Publishing native binaries for: $rid" -ForegroundColor Yellow

foreach ($project in $inputs) {
    $projectName = Split-Path -Path "$project" -Leaf -Resolve | Split-Path -LeafBase
    $pub = "$publishPath/$projectName/$rid"
    DotnetPublish $project $pub $rid
    CopyDocs $pub
    ZipArtifacts $projectName $pub $rid
}

Write-Host "Publish binaries finished" -ForegroundColor Green
