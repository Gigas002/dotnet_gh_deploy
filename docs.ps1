<#
.SYNOPSIS
    Script for building docs with docfx

.DESCRIPTION
    Change docfx.json for configuration

.EXAMPLE
    ./script.ps1 -d "docfx.json"

.LINK
    https://github.com/senketsu03/dotnet_gh_deploy
    https://dotnet.github.io/docfx/docs/basic-concepts.html
#>

param (
    # docfx.json path
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [string] $docfxJson = "docfx.json"
)

Write-Host "Started generating docs with docfx ($docfxJson)..." -ForegroundColor Yellow

Copy-Item README.md _docs/index.md
docfx metadata $docfxJson
docfx build $docfxJson
# docfx serve

Write-Host "Finished generating docs with docfx" -ForegroundColor Green
