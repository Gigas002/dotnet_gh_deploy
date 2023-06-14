# Deploy.Server

Uses `HTTP/1.1`, `HTTP/2` and `HTTP/3`. See the API on `https://localhost:5230/swagger` or `https://localhost:5230/api-docs`

## Requirements

If you're running on `win11` -- you're good to go, just build the solution and run the apps. For `linux` you'll need some stuff been done, otherwise server/client will fail on runtime:

1. Install dependencies: `dotnet`, `nss`, `openssl`
2. Install `msquic` with your package manager (e.g. from [AUR](https://aur.archlinux.org/packages/msquic); there's no [native nuget package for linux](https://github.com/microsoft/msquic/discussions/3700))
3. Install `devcerts`. See [official docs](https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-8.0&tabs=visual-studio%2Clinux-ubuntu#trust-https-certificate-on-linux), [related issue](https://github.com/dotnet/aspnetcore/issues/32842#issuecomment-1107951047) and [this repo](https://github.com/BorisWilhelms/create-dotnet-devcert). Note, that these solutions doesn't provide ipv6 support yet
