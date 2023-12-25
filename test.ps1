<#
.SYNOPSIS
    Script for running test and recieving coverage reports

.DESCRIPTION
    Change runsettings.xml for configuration

.EXAMPLE
    ./script.ps1 -i "proj1/proj1.csproj","proj2/proj2.csproj" -r "runsettings.xml"

.LINK
    https://github.com/Gigas002/dotnet_gh_deploy
    https://github.com/coverlet-coverage/coverlet/blob/master/Documentation/VSTestIntegration.md
#>

[CmdletBinding(PositionalBinding = $false)]
param (
    # Paths to projects to test
    [Parameter ()]
    [ValidateNotNullOrEmpty ()]
    [string[]] $inputs = ("Deploy.Tests/Deploy.Tests.csproj",
                          "Deploy.Server.Tests/Deploy.Server.Tests.csproj"
    ),    
    
    # Codecov token
    [Parameter ()]
    [Alias("codecov-token")]
    [SecureString] $codecovToken = (Read-Host "Enter your codecov token" -AsSecureString),

    # runsettings.xml path
    [Parameter ()]
    [Alias("r", "runsettings-xml")]
    [string] $runsettingsXml = ""
)

#region Functions

function Test() {
    Write-Host "dotnet test started..." -ForegroundColor Yellow

    foreach ($project in $inputs) {
        Write-Host "Testing: $project" -ForegroundColor Yellow

        if ($runsettingsXml) {
            dotnet test $project -c Release --settings $runsettingsXml
        }
        else {
            dotnet test $project -c Release --collect "XPlat Code Coverage;SkipAutoProps=true"
        }
    }

    Write-Host "dotnet test finished" -ForegroundColor Green
}

function Coverage() {
    # upload test results
    if ($codecovToken -and $codecovToken.Length -gt 0) {
        Write-Host "upload coverage report started..." -ForegroundColor Yellow

        $token = ConvertFrom-SecureString $codecovToken -AsPlainText
        codecov -t $token

        Write-Host "upload coverage report finished" -ForegroundColor Green
    }
}

#endregion

Write-Host "Started test/codecov process..." -ForegroundColor Yellow

Test
Coverage

Write-Host "Finished test/codecov process" -ForegroundColor Green
