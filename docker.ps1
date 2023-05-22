$dockerHubUserName = "trolltrollski"
$githubUserName = "senketsu03"
$dockerHubToken = ""
$githubToken = ""
$application = "deploy.cli"
$version = "latest"
$dockerHubTag = "$dockerHubUserName/${application}:$version"
$githubTag = "ghcr.io/$githubUserName/dotnet_gh_deploy/${application}:$version"
$dockerFile = "Dockerfile"

#region publish to Docker hub

# log in
docker login -u $dockerHubUserName -p $dockerHubToken

# build image
docker build -t $dockerHubTag -f $dockerFile .

# push image
docker push $dockerHubTag

#endregion

#region publish to Github

# log in
docker login -u $githubUserName -p $githubToken

# build image
docker build -t $githubTag -f $dockerFile .

# push image
docker push $githubTag

#endregion
