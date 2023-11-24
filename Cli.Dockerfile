# https://hub.docker.com/_/microsoft-dotnet-sdk
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /Deploy

# copy csproj and restore as distinct layers
COPY . ./
RUN dotnet restore Deploy.Cli/Deploy.Cli.csproj -r linux-x64

# copy and publish app and libraries
RUN dotnet publish Deploy.Cli/Deploy.Cli.csproj -c Release -o /app -r linux-x64 --sc false --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:8.0-jammy-chiseled
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["./Deploy.Cli"]
