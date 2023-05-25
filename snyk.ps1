<#
.SYNOPSIS
    Script for running security analyzis with snyk

.DESCRIPTION
    Requires snyk tool

.EXAMPLE
    ./script.ps1 -r "runsettings.xml"

.LINK
    https://github.com/senketsu03/dotnet_gh_deploy
    https://github.com/snyk/cli/releases
    https://docs.snyk.io/snyk-cli
#>

[CmdletBinding(PositionalBinding = $false)]
param (
    # snyk token
    [Parameter (Mandatory=$true)]
    [Alias("snyk-token")]
    [SecureString] $snykToken = (Read-Host "Enter your snyk token" -AsSecureString),

    # sarif path
    [Parameter ()]
    [Alias("sarif-path")]
    [string] $sarifPath = "snyk-code.sarif"
)

Write-Host "Started snyk analyzis in process..." -ForegroundColor Yellow

snyk auth $snykToken

# || true means dont throw if error
snyk code test --sarif > "$sarifPath" || true

# fails due to https://docs.snyk.io/guides/snyk-for-.net-developers#not-supported-in-.net
snyk monitor --all-projects || true

Write-Host "Snyk analyzis finished" -ForegroundColor Green
