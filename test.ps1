# for configuration see:
# https://github.com/coverlet-coverage/coverlet/blob/master/Documentation/VSTestIntegration.md

Write-Host "Started test/codecov process..." -ForegroundColor Yellow

# $format = 'opencover'
# dotnet test --collect:"XPlat Code Coverage;Format=$format" # --results-directory .
dotnet test /tl --collect:"XPlat Code Coverage"

# upload test results
# codecov -t $token

Write-Host "Finished test/codecov process" -ForegroundColor Green
