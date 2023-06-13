# Scripts usage

## Requirements

Depends on what you want to publish

- [mandatory] [dotnet sdk](https://dotnet.microsoft.com/en-us/download/dotnet)
- [optional] [docfx](https://github.com/dotnet/docfx) or run `dotnet tool update -g docfx`
- [optional] [codecov](https://github.com/codecov/uploader/releases)
- [optional] [docker](https://www.docker.com/products/docker-desktop/)
- [optional] [snyk](https://github.com/snyk/cli/releases)

And accounts on following platforms:

- [codecov](https://app.codecov.io/gh)
- [nuget](https://www.nuget.org/)
- [github](https://github.com/)
- [docker hub](https://hub.docker.com/)
- [snyk](https://app.snyk.io/)

## Script/args

test.ps1:

- [secure,optional] -codecovToken/-codecov-token
- [optional] -r/-runsettingsXml/-runsettings-xml

src.ps1 -> ./read_version.ps1:

- [optional] -p/-publishPath/-publish-path
- [optional] -continiousTag/-continious-tag
- [optional] -b/-buildPropsPath/-build-props-path

read_version.ps1:

- [optional] -b/-buildPropsPath/-build-props-path
- [optional] -dockerContiniousTag/-docker-continious-tag

publish.ps1:

- [optional] -p/-publishPath/-publish-path
- [optional] -i/-inputs

nupkg.ps1 -> read_version.ps1:

- [secure,optional] -n/-nugetToken/-nuget-token
- [secure,optional] -githubToken/-github-token
- [optional] -i/-inputs
- [optional] -p/-publishPath/-publish-path
- [optional] -githubFeedName/-github-feed-name
- [optional] -b/-buildPropsPath/-build-props-path

docs.ps1:

- [optional] -docfxJson/-docfx-json

docker.ps1 -> read_version.ps1:

- [secure,optional] -dockerHubToken/-docker-hub-token
- [secure,optional] -githubToken/-github-token
- [optional] -dockerHubUsername/-docker-hub-username
- [optional] -githubUsername/-github-username
- [optional] -i/-inputs
- [optional] -githubRepoName/-github-repo-name
- [optional] -b/-buildPropsPath/-build-props-path
- [optional] -dockerContiniousTag/-docker-continious-tag

snyk.ps1 -> read_version.ps1:

- [secury,mandatory] -snykToken/-snyk-token
- [optional] -inputsDocker/-inputs-docker
- [optional] -b/-buildPropsPath/-build-props-path
- [optional] -dockerContiniousTag/-docker-continious-tag
- [optional] -githubRepoName/-github-repo-name
- [optional] -dockerHubUsername/-docker-hub-username
- [optional] -githubUsername/-github-username

## build_test_deploy.ps1

Includes all the above args (excluding `inputs`), additionaly listed below:

- -b/-buildPropsPath/-build-props-path
- -codecovToken/-codecov-token
- -continiousTag/-continious-tag
- -dockerContiniousTag/-docker-continious-tag
- -docfxJson/-docfx-json
- -dockerHubToken/-docker-hub-token
- -dockerHubUsername/-docker-hub-username
- -githubToken/-github-token
- -githubUsername/-github-username
- -githubFeedName/-github-feed-name
- -githubRepoName/-github-repo-name
- ~~-i/-inputs~~
- -n/-nugetToken/-nuget-token
- -p/-publishPath/-publish-path
- -r/-runsettingsXml/-runsettings-xml
- -snykToken/-snyk-token

build_test_deploy.ps1 exclusive args:

- -inputsPublish/-inputs-publish
- -inputsNupkg/-inputs-nupgkg
- -inputsDocker/-inputs-docker
