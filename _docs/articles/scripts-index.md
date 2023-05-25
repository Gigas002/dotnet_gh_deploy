# Scripts usage/args

## Script/tag

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

nuget.ps1 -> read_version.ps1:

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

## All tags listed

-b/-buildPropsPath/-build-props-path

-codecovToken/-codecov-token
-continiousTag/-continious-tag

-dockerContiniousTag/-docker-continious-tag
-docfxJson/-docfx-json
-dockerHubToken/-docker-hub-token
-dockerHubUsername/-docker-hub-username

-githubToken/-github-token
-githubFeedName/-github-feed-name
-githubUsername/-github-username
-githubRepoName/-github-repo-name

-i/-inputs

-n/-nugetToken/-nuget-token

-p/-publishPath/-publish-path

-r/-runsettingsXml/-runsettings-xml
