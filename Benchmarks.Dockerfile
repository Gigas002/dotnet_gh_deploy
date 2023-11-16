# https://hub.docker.com/_/microsoft-dotnet-sdk
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /Deploy

# copy csproj and restore as distinct layers
COPY . ./
RUN dotnet restore Deploy.Benchmarks/Deploy.Benchmarks.csproj -r linux-x64

# copy and publish app and libraries
RUN dotnet publish Deploy.Benchmarks/Deploy.Benchmarks.csproj -c Release -o /app -r linux-x64 --sc false --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["./Deploy.Benchmarks"]
