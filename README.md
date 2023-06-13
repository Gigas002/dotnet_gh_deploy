# dotnet_gh_deploy

## Status

[![build-test-deploy](https://github.com/senketsu03/dotnet_gh_deploy/actions/workflows/build-test-deploy.yml/badge.svg)](https://github.com/senketsu03/dotnet_gh_deploy/actions/workflows/build-test-deploy.yml)

![Dependabot Status](https://flat.badgen.net/github/dependabot/senketsu03/dotnet_gh_deploy)

[![codecov](https://codecov.io/github/senketsu03/dotnet_gh_deploy/branch/master/graph/badge.svg)](https://codecov.io/github/senketsu03/dotnet_gh_deploy)

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/0830b8500252474481805631e4984392)](https://app.codacy.com/gh/senketsu03/dotnet_gh_deploy/dashboard)

Training to use GitHub CI only to deploy dotnet stuff

## Configuration

Grant actions more permissions. See Repo settings->Actions->General page

You'll need to select a branch `gh-pages` for the first deployment, see: https://github.com/marketplace/actions/github-pages-action#%EF%B8%8F-first-deployment-with-github_token

Manual setting of these secrets required: `CODECOV_TOKEN`, `DOCKER_HUB_TOKEN`, `NUGET_API_KEY`, `SNYK_TOKEN`

GitHub apps integrations:

- [codacy](https://github.com/codacy)
- [codecov](https://github.com/codecov)
- [Dotnet Policy Service](https://github.com/microsoft1estools)

## Releases

**Downloaded**

On Github: [![GitHub all releases](https://img.shields.io/github/downloads/senketsu03/dotnet_gh_deploy/total)](https://github.com/senketsu03/dotnet_gh_deploy/releases)

Deploy.Core: [![Nuget](https://img.shields.io/nuget/dt/Deploy.Core)](https://www.nuget.org/packages/Deploy.Core/)

Deploy.Core.Dummy: [![Nuget](https://img.shields.io/nuget/dt/Deploy.Core.Dummy)](https://www.nuget.org/packages/Deploy.Core.Dummy/)

**Grab binaries**

GitHub Releases: [![Release](https://img.shields.io/github/release/senketsu03/dotnet_gh_deploy.svg)](https://github.com/senketsu03/dotnet_gh_deploy/releases/latest)

NuGet: [![NuGet](https://img.shields.io/nuget/v/Deploy.Core.svg)](https://www.nuget.org/packages/Deploy.Core/)

Deploy.Cli on Docker Hub: [![Docker Image (latest/tagged)](https://img.shields.io/docker/v/trolltrollski/deploy.cli)](https://hub.docker.com/repository/docker/trolltrollski/deploy.cli)

Deploy.Benchmarks on Docker Hub: [![Docker Image (latest/tagged)](https://img.shields.io/docker/v/trolltrollski/deploy.benchmarks)](https://hub.docker.com/repository/docker/trolltrollski/deploy.benchmarks)

## Vulnerabilities

Build.props are [not yet supported](https://docs.snyk.io/guides/snyk-for-.net-developers#not-supported-in-.net) by snyk

## Resources

Icons: https://www.flaticon.com/packs/far-west

## Sponsors

![GitHub Sponsors](https://img.shields.io/github/sponsors/senketsu03)

## License

[![GitHub](https://img.shields.io/github/license/senketsu03/dotnet_gh_deploy)](https://github.com/senketsu03/dotnet_gh_deploy/blob/master/LICENSE.md)
