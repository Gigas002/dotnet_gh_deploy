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

[CmdletBinding(PositionalBinding = $false)]
param (
    # Codecov token
    [Parameter ()]
    [Alias("codecov-token")]
    [SecureString] $codecovToken = (Read-Host "Enter your codecov token" -AsSecureString),

    # runsettings.xml path
    [Parameter ()]
    [Alias("r", "runsettings-xml")]
    [string] $runsettingsXml = ""
)

Write-Host "Started test/codecov process..." -ForegroundColor Yellow

if ($runsettingsXml) {
    dotnet test /tl --collect:"XPlat Code Coverage" --settings $runsettingsXml
}
else {
    dotnet test /tl --collect:"XPlat Code Coverage"
}

if ($codecovToken -and $codecovToken.Length -gt 0) {
    # upload test results
    $token = ConvertFrom-SecureString $codecovToken -AsPlainText
    codecov -t $token
}

Write-Host "Finished test/codecov process" -ForegroundColor Green
