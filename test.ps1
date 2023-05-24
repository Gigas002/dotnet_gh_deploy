<#
.SYNOPSIS
    Script for running test and recieving coverage reports

.DESCRIPTION
    Change runsettings.xml for configuration

.EXAMPLE
    ./script.ps1 -r "runsettings.xml"

.LINK
    https://github.com/senketsu03/dotnet_gh_deploy
    https://github.com/coverlet-coverage/coverlet/blob/master/Documentation/VSTestIntegration.md
#>

param (
     # Codecov token
    [Parameter (Mandatory = $true)]
    [SecureString] $codecovToken = (Read-Host "Enter your codecov token" -AsSecureString),

    # runsettings.xml path
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [string] $runsettingsXml = ""
)

Write-Host "Started test/codecov process..." -ForegroundColor Yellow

# $format = 'opencover'
# dotnet test --collect:"XPlat Code Coverage;Format=$format" # --results-directory .
dotnet test /tl --collect:"XPlat Code Coverage" --settings $runsettingsXml

# TODO: secure string is null
# $token = ConvertFrom-SecureString $codecovToken -AsPlainText
if ($token)
{
    # upload test results
    codecov -t $token
}

Write-Host "Finished test/codecov process" -ForegroundColor Green
