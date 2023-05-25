<#
.DESCRIPTION
    Script for publishing applications. Requires .sln and Directory.Build.props

.EXAMPLE
    ./script.ps1 -o "publish" -i "proj1/proj1.csproj","proj2/proj2.csproj"

.LINK
    https://github.com/senketsu03/dotnet_gh_deploy
#>

[CmdletBinding(PositionalBinding = $false)]
param (

)

#region Constants



#endregion

#region Functions


#endregion

# TODO: args

# TODO: pack src before building anything
./src.ps1

# build and test + upload report if needed
./test.ps1 $codecovToken

# build docs
./docs.ps1 $docfxJson

# publish binaries
./publish.ps1

# publish nupkgs
./nuget.ps1

# publish docker
./docker.ps1
