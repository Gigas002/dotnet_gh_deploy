<#
.SYNOPSIS
    Script for bundling current repo into .zip file, excluding .gitignore files

.EXAMPLE
    ./script.ps1 -o "repo.zip"

.LINK
    https://github.com/senketsu03/dotnet_gh_deploy
    https://git-scm.com/docs/git-archive
#>

param (
    # archive name
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [string] $output = ""
)

Write-Host "Started compressing the repo into .zip..." -ForegroundColor Yellow

if (-not $output)
{
    $dir = Get-Location
    $output = Split-Path -Path $dir.Path -Leaf
}

git archive HEAD -o "$output.zip" --worktree-attributes -v

Write-Host "Finished compressing the repo into $output.zip" -ForegroundColor Green
