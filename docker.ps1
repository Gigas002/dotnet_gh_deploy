<#
.DESCRIPTION
    Script for deploying docker images

.EXAMPLE
    ./script.ps1 -docker-hub-username trolltrollski -github-username Gigas002 -i @{"dotnet.cli"="Cli.Dockerfile";"dotnet.benchmarks"="Benchmarks.Dockerfile"} -github-repo-name dotnet_gh_deploy -b "Directory.Build.props" -docker-continious-tag "latest"

.LINK
    https://github.com/Gigas002/dotnet_gh_deploy
#>

[CmdletBinding(PositionalBinding = $false)]
param (
    # Docker hub token
    [Parameter ()]
    [Alias("docker-hub-token")]
    [SecureString] $dockerHubToken = (Read-Host "Enter your docker hub token token" -AsSecureString),

    # Github token
    [Parameter ()]
    [Alias("github-token")]
    [SecureString] $githubToken = (Read-Host "Enter your github token" -AsSecureString),

    # Docker hub username
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("docker-hub-username")]
    [string] $dockerHubUsername = "trolltrollski",

    # Github username
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("github-username")]
    [string] $githubUsername = "Gigas002",

    # Dictionary<ProjectName,DockerfilePath>
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [hashtable] $inputs = [ordered]@{
        "dotnet.cli"        = "Cli.Dockerfile";
        "dotnet.benchmarks" = "Benchmarks.Dockerfile"
    },

    # Github repo name
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("github-repo-name")]
    [string] $githubRepoName = "dotnet_gh_deploy",    

    # Directory.Build.props path
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("b", "build-props-path")]
    [string] $buildPropsPath = "Directory.Build.props",

    # docker continious tag
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("docker-continious-tag")]
    [string] $dockerContiniousTag = "latest"
)

#region Constants

Set-Variable DockerHubRegistry -Option ReadOnly -Value "index.docker.io"
Set-Variable GithubRegistry -Option ReadOnly -Value "ghcr.io"

#endregion

#region Functions

function DeployDocker([string]$registry, [string] $username, [SecureString] $token,
    [string] $imageKey, [string] $tag) {
    $dockerfile = $inputs[$imageKey]
        
    Write-Host "docker build image: $imageKey with $dockerfile" -ForegroundColor Yellow
    docker build -t $tag -f $dockerfile .

    if ($token -and $token.Length -gt 0) {
        $decryptedToken = ConvertFrom-SecureString $token -AsPlainText
        Write-Host "docker login to registry: $registry" -ForegroundColor Yellow
        docker login $registry -u $username -p $decryptedToken
        
        Write-Host "docker push image: $tag" -ForegroundColor Yellow
        docker push $tag
        
        Write-Host "docker logout: $registry" -ForegroundColor Yellow
        docker logout $registry
    }
}

#endregion

Write-Host "Deploy of docker images started..." -ForegroundColor Yellow

$_, $_, $_, $dockerTag = ./read_version.ps1 -b $buildPropsPath -dockerContiniousTag $dockerContiniousTag

foreach ($imageKey in $inputs.Keys) {
    $dockerHubTag = "$dockerHubUsername/${imageKey}:$dockerTag"
    $githubTag = "$GithubRegistry/$githubUserName/$githubRepoName/${imageKey}:$dockerTag"

    DeployDocker $DockerHubRegistry $dockerHubUsername $dockerHubToken $imageKey $dockerHubTag
    DeployDocker $GithubRegistry $githubUsername $githubToken $imageKey $githubTag
}

Write-Host "Finished deploy of docker images" -ForegroundColor Green
