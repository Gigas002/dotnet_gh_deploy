# change docfx.json for configuration
# see docfx docs: https://dotnet.github.io/docfx/docs/basic-concepts.html

Write-Host "Started generating docs with docfx..." -ForegroundColor Yellow

Copy-Item README.md _docs/index.md
docfx metadata
docfx build
# docfx serve

Write-Host "Finished generating docs with docfx" -ForegroundColor Yellow
