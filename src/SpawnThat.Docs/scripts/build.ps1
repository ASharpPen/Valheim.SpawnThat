# Get old code references
if (-Not (Test-Path -Path ..\obj\oldcode\1.0.0\))
{
  New-Item -ItemType Directory -Force -Path ..\obj\oldcode\1.0.0\
  git clone -b 1.0.5 --depth 1 https://github.com/ASharpPen/Valheim.SpawnThat.git ..\obj\oldcode\1.0.0\
}

# Build initial
docfx metadata ../docfx.json

# Fix namespace nesting for code docs
.\nest-namespaces.ps1 ../obj/api/1.0.0/reference/toc.yml ../obj/api/1.0.0/reference/toc.yml
.\nest-namespaces.ps1 ../obj/api/1.1.0/reference/toc.yml ../obj/api/1.1.0/reference/toc.yml

# Build final
docfx build ../docfx.json
