<#
.SYNOPSIS
    Script for publishing applications. Requires .sln and Directory.Build.props

.DESCRIPTION
    For additional usage info, please use Get-Help -Full and see related scripts source code

.LINK
    https://github.com/Gigas002/dotnet_gh_deploy
#>

[CmdletBinding(PositionalBinding = $false)]
param (
    # Directory.Build.props path
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("b", "build-props-path")]
    [string] $buildPropsPath = "Directory.Build.props",

    # Codecov token
    [Parameter ()]
    [Alias("codecov-token")]
    [SecureString] $codecovToken = (Read-Host "Enter your codecov token" -AsSecureString),

    # continious tag
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("continious-tag")]
    [string] $continiousTag = "continious",

    # docker continious tag
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("docker-continious-tag")]
    [string] $dockerContiniousTag = "latest",

    # docfx.json path
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("docfx-json")]
    [string] $docfxJson = "docfx.json",

    # Docker hub token
    [Parameter ()]
    [Alias("docker-hub-token")]
    [SecureString] $dockerHubToken = (Read-Host "Enter your docker hub token token" -AsSecureString),

    # Docker hub username
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("docker-hub-username")]
    [string] $dockerHubUsername = "gigas002",

    # Github token
    [Parameter ()]
    [Alias("github-token")]
    [SecureString] $githubToken = (Read-Host "Enter your github token" -AsSecureString),

    # Github username
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("github-username")]
    [string] $githubUsername = "Gigas002",
 
    # Github package feed name
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("github-feed-name")]
    [string] $githubFeedName = "Gigas002",

    # Github repo name
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("github-repo-name")]
    [string] $githubRepoName = "dotnet_gh_deploy",    

    # Nuget API token
    [Parameter ()]
    [Alias("n", "nuget-token")]
    [SecureString] $nugetToken = (Read-Host "Enter your nuget token" -AsSecureString),

    # Publish path
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("p", "publish-path")]
    [string] $publishPath = "publish",

    # runsettings.xml path
    [Parameter ()]
    [Alias("r", "runsettings-xml")]
    [string] $runsettingsXml = "",

    # Paths to projects to publish
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("inputs-publish")]
    [string[]] $inputsPublish = ("Deploy.Cli/Deploy.Cli.csproj",
                                 "Deploy.Benchmarks/Deploy.Benchmarks.csproj",
                                 "Deploy.Gui/Deploy.Gui.csproj",
                                 "Deploy.Server/Deploy.Server.csproj",
                                 "Deploy.Client/Deploy.Client.csproj"),

    # Paths to packages to publish
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("inputs-nupkg")]
    [string[]] $inputsNupkg = (
        "Deploy.Core/Deploy.Core.csproj",
        "Deploy.Core.Dummy/Deploy.Core.Dummy.csproj"
    ),

    # Dictionary<ProjectName,DockerfilePath>
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("inputs-docker")]
    [hashtable] $inputsDocker = [ordered]@{
        "dotnet.cli"        = "Cli.Dockerfile";
        "dotnet.benchmarks" = "Benchmarks.Dockerfile"
    },

    # snyk token
    [Parameter ()]
    [Alias("snyk-token")]
    [SecureString] $snykToken = (Read-Host "Enter your snyk token" -AsSecureString),
    
    # sarif path
    [Parameter ()]
    [Alias("sarif-path")]
    [string] $sarifPath = "snyk-code.sarif"
)

Write-Host "Removing previous publish directory: $publishPath" -ForegroundColor Yellow
Remove-Item -Path $publishPath -Recurse -Force -ErrorAction SilentlyContinue

# pack all source code into zip, excluding .gitignore files
./src.ps1 -p $publishPath -continious-tag $continiousTag -b $buildPropsPath

# build and test + upload report if needed
./test.ps1 -codecov-token $codecovToken -r $runsettingsXml

# build docs
./docs.ps1 -docfx-json $docfxJson

# publish binaries
./publish.ps1 -p $publishPath -i $inputsPublish

# publish nupkgs
./nupkg.ps1 -n $nugetToken -github-token $githubToken -i $inputsNupkg -p $publishPath -github-feed-name $githubFeedName -b $buildPropsPath

# publish docker
./docker.ps1 -docker-hub-token $dockerHubToken -github-token $githubToken -docker-hub-username $dockerHubUsername -github-username $githubUsername -i $inputsDocker -github-repo-name $githubRepoName -b $buildPropsPath -docker-continious-tag $dockerContiniousTag

# run secirity analyzis if needed
if ($snykToken -and $snykToken.Length -gt 0) {
    ./snyk.ps1 -snyk-token $snykToken -docker-hub-token $dockerHubToken -github-token $githubToken -docker-hub-username $dockerHubUsername -github-username $githubUsername -inputs-docker $inputsDocker -github-repo-name $githubRepoName -b $buildPropsPath -docker-continious-tag $dockerContiniousTag
}

Write-Host "Deploy complete" -ForegroundColor Green
