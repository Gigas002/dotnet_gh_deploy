# for configuration see:
# https://github.com/coverlet-coverage/coverlet/blob/master/Documentation/VSTestIntegration.md

$format = 'opencover'

dotnet test --collect:"XPlat Code Coverage;Format=$format" # --results-directory .
