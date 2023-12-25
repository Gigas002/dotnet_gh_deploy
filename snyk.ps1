<#
.SYNOPSIS
    Script for running security analyzis with snyk

.DESCRIPTION
    Requires snyk tool

.EXAMPLE
    ./script.ps1 -docker-hub-username gigas002 -github-username Gigas002 -inputs-docker @{"dotnet.cli"="Cli.Dockerfile";"dotnet.benchmarks"="Benchmarks.Dockerfile"} -github-repo-name dotnet_gh_deploy -b "Directory.Build.props" -docker-continious-tag "latest"

.LINK
    https://github.com/Gigas002/dotnet_gh_deploy
    https://github.com/snyk/cli/releases
    https://docs.snyk.io/snyk-cli/commands
    https://docs.snyk.io/snyk-cli/test-for-vulnerabilities/set-severity-thresholds-for-cli-tests
#>

[CmdletBinding(PositionalBinding = $false)]
param (
    # snyk token
    [Parameter ()]
    [Alias("snyk-token")]
    [SecureString] $snykToken = (Read-Host "Enter your snyk token" -AsSecureString),

    # Dictionary<ProjectName,DockerfilePath>
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("inputs-docker")]
    [hashtable] $inputsDocker = [ordered]@{
        "dotnet.cli"        = "Cli.Dockerfile";
        "dotnet.benchmarks" = "Benchmarks.Dockerfile"
    },

    # Directory.Build.props path
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("b", "build-props-path")]
    [string] $buildPropsPath = "Directory.Build.props",
    
    # docker continious tag
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("docker-continious-tag")]
    [string] $dockerContiniousTag = "latest",

    # Github repo name
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("github-repo-name")]
    [string] $githubRepoName = "dotnet_gh_deploy", 
    # Docker hub username
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("docker-hub-username")]
    [string] $dockerHubUsername = "gigas002",

    # Github username
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [Alias("github-username")]
    [string] $githubUsername = "Gigas002"
)

#region Constants

Set-Variable ReportsBase -Option ReadOnly -Value "sarifs"
Set-Variable CodeSarif -Option ReadOnly -Value "code.sarif"
Set-Variable DockerHubRegistry -Option ReadOnly -Value "index.docker.io"
Set-Variable GithubRegistry -Option ReadOnly -Value "ghcr.io"

#endregion

if ($snykToken -and $snykToken.Length -gt 0) {
    Write-Host "Started snyk analyzis in process..." -ForegroundColor Yellow

    snyk auth $snykToken

    # || true means dont throw if error
    snyk code test --sarif > "$ReportsBase/$CodeSarif" || true

    # fails due to https://docs.snyk.io/guides/snyk-for-.net-developers#not-supported-in-.net
    snyk monitor --all-projects || true

    # check docker images
    $_, $_, $_, $dockerTag = ./read_version.ps1 -b $buildPropsPath -dockerContiniousTag $dockerContiniousTag

    foreach ($imageKey in $inputsDocker.Keys) {
        $dockerHubTag = "$dockerHubUsername/${imageKey}:$dockerTag"
        $githubTag = "$GithubRegistry/$githubUserName/$githubRepoName/${imageKey}:$dockerTag"

        $dockerHubSarif = "$ReportsBase/{$imageKey}_dockerhub.sarif"
        $githubSarif = "$ReportsBase/{$imageKey}_github.sarif"
        snyk container test $dockerHubTag --file $inputsDocker[$imageKey] --sarif-file-output=$dockerHubSarif --exclude-base-image-vulns=true
        snyk container test $githubTag --file $inputsDocker[$imageKey] --sarif-file-output=$githubSarif --exclude-base-image-vulns=true
    }

    Write-Host "Snyk analyzis finished" -ForegroundColor Green
}
else {
    Write-Host "No snyk-token specified, skipping..." -ForegroundColor Yellow
}
