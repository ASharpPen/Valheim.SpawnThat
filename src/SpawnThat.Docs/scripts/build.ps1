docfx metadata ../docfx.json

.\nest-namespaces.ps1 ../obj/api/reference/toc.yml ../obj/api/reference/toc.yml

docfx build ../docfx.json

