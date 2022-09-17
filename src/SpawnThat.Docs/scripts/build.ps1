# Build initial
docfx metadata ../docfx.json

# Fix namespace nesting for code docs
.\nest-namespaces.ps1 ../obj/api/reference/toc.yml ../obj/api/reference/toc.yml

# Build final
docfx build ../docfx.json
