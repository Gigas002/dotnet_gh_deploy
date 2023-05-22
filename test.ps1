# for configuration see:
# https://github.com/coverlet-coverage/coverlet/blob/master/Documentation/VSTestIntegration.md

# $format = 'opencover'

# dotnet test --collect:"XPlat Code Coverage;Format=$format" # --results-directory .
dotnet test --collect:"XPlat Code Coverage"

# upload test results
# codecov -t $token
