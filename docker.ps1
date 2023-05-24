<#
.DESCRIPTION
    Script for deploying docker images

.EXAMPLE
    ./script.ps1 -d trolltrollski -g senketsu03 -inputs @{"dotnet.cli"="Dockerfile"} -repo dotnet_gh_deploy

.LINK
    https://github.com/senketsu03/dotnet_gh_deploy
#>

[CmdletBinding(PositionalBinding = $false)]
param (
    # Docker hub token
    [Parameter (Mandatory = $true)]
    [SecureString] $dockerHubToken = (Read-Host "Enter your docker hub token token" -AsSecureString),

    # Github token
    [Parameter (Mandatory = $true)]
    [SecureString] $githubToken = (Read-Host "Enter your github token" -AsSecureString),

    # Docker hub username
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("d", "docker-hub-username")]
    [string] $dockerHubUsername = "trolltrollski",

    # Github username
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("g", "github-username")]
    [string] $githubUsername = "senketsu03",

    # Dictionary<ProjectName,DockerfilePath>
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [hashtable] $inputs = [ordered]@{
        "dotnet.cli" = "Dockerfile"
    },

    # Github repo name
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("repo", "github-repo-name")]
    [string] $githubRepoName = "dotnet_gh_deploy",    

    # Directory.Build.props path
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("p")]
    [string] $buildPropsPath = "Directory.Build.props",

    # docker continious tag
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("c")]
    [string] $continiousTag = "latest"
)

$versionPrefix, $versionSuffix, $buildVersion, $dockerTag = ./read_version.ps1 -p $buildPropsPath -c $continiousTag

#region Constants

Set-Variable DockerHubRegistry -Option ReadOnly -Value "index.docker.io"
Set-Variable GithubRegistry -Option ReadOnly -Value "ghcr.io"

#endregion

#region Functions

function DeployDocker([string]$registry, [string] $username, [SecureString] $token,
    [string] $imageKey, [string] $tag) {
    $dockerfile = $inputs[$imageKey]
    $decryptedToken = ConvertFrom-SecureString $token -AsPlainText

    Write-Host "docker login to registry: $registry" -ForegroundColor Yellow
    docker login $registry -u $username -p $decryptedToken

    Write-Host "docker build image: $imageKey with $dockerfile" -ForegroundColor Yellow
    docker build -t $tag -f $dockerfile .

    Write-Host "docker push image: $tag" -ForegroundColor Yellow
    docker push $tag

    Write-Host "docker logout: $registry" -ForegroundColor Yellow
    docker logout $registry
}

#endregion

Write-Host "Deploy of docker images started..." -ForegroundColor Yellow

foreach ($imageKey in $inputs.Keys) {
    $dockerHubTag = "$dockerHubUsername/${imageKey}:$dockerTag"
    $githubTag = "$GithubRegistry/$githubUserName/$githubRepoName/${imageKey}:$dockerTag"

    DeployDocker $DockerHubRegistry $dockerHubUsername $dockerHubToken $imageKey $dockerHubTag
    DeployDocker $GithubRegistry $githubUsername $githubToken $imageKey $githubTag
}

Write-Host "Finished deploy of docker images" -ForegroundColor Green
