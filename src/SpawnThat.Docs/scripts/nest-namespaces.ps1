[CmdletBinding()]
[Parameter(Mandatory=$true, Position=0)]
param (
  $File
)

if (-not(Test-Path $File))
{
  return Write-Host "Missing path to file."
}

Import-Module powershell-yaml

Write-Host "Reading $File"

$content = Get-Content -Path $File -Raw

$yaml = ConvertFrom-Yaml $content

$result = New-Object ([System.Collections.specialized.OrderedDictionary])

#$paths = [ordered]@{}
#$items = [ordered]@{}

$nested = [ordered]@{}

function NestItems2($Entry)
{
  $currentPath = $nested

  $depth = 0

  $namespaces = $Entry['uid'].split('.');
  for ($i = 0; $i -lt $namespaces.Length; $i++)
  {
    $currentKey = $namespaces[$i];

    # Ensure last is not same as name
    if ($i -ne ($namespaces.Length))
    {
      if ($Entry['name'] -ne $currentKey)
      {
        # We are neither the last key, nor same as name.
        # Ergo, this must be a namespace part.

        # Ensure nested path table exist
        if (-not $currentPath.Contains('pathId'))
        {
          $currentPath['pathId'] = [ordered]@{}
        }

        $pathTable = $currentPath['pathId'];

        if ($pathTable.Contains($currentKey))
        {
          # Move down to existing nested path.
          $currentPath = $pathTable[$currentKey]
        }
        else 
        {
          # Add new nested path.
          $nestedPath = [ordered]@{}
          $nestedPath['uid'] = ' '*2*$depth + '- uid: '
          $nestedPath['name'] = ' '*2*$depth + '  name: ' + $currentKey
          $pathTable[$currentKey] = $nestedPath
          $currentPath = $nestedPath
        }

        $depth++
      }
    }
  }

  if ($currentKey -eq $Entry['name'])
  {
    # This is a file.
    if (-not $currentPath.Contains('items'))
    {
      $currentPath['items'] = {}
    }

    $indent = ' '*2*$Depth
    $uid = $indent + "- uid: " + $Entry['uid']
    $name = $indent + "  name: " + $Entry['name']

    $currentPath['items'].Add($uid, $name);
  }
}

function NestItems($Entry)
{
  $currentPath = $paths
  
  foreach ($currentKey in $Entry['uid'].split('.'))
  {
    if ($currentKey -eq $Entry['name'])
    {
      #This is a file.
    }

    if ($currentPath.Contains($currentKey))
    {
      $currentPath = $currentPath[$currentKey]
    }
    else
    {
      $path = [ordered]@{}
      $currentPath[$currentKey] = $path
      $currentPath = $path
    }
  }

  # Check if this is a file. Files do not contain full namespace in name.

  if ($Entry['uid'] -ne $Entry['name'])
  {
    Write-Host ($currentKey + ":" + $Entry['name'])
    $currentPath['name'] = $Entry['name']
  }

  foreach ($item in $Entry['items'])
  {
    NestItems($item);
  }
}

function CreatePaths($Entry)
{
  $namespace = $Entry['uid'];
  $namespaces = $namespace.split(".");

  $currentPath = $paths

  foreach ($currentKey in $namespaces)
  {
    #Write-Host $currentKey
    if ($currentPath.Contains($currentKey))
    {
      $currentPath = $currentPath[$currentKey]
    }
    else 
    {
      $path = [ordered]@{}
      $currentPath[$currentKey] = $path
      $currentPath = $path
    }
  }
}

foreach ($dic in $yaml)
{
  NestItems2($dic)
}

$out = "$File.2"

function WriteRecursive($Entry, $CurrentPath, $Indent)
{
  # Check if we hit a file.
  if ($Entry -is [System.String])
  {

  }

  foreach ($key in $Entry.Keys) {
    
  }

  $uid = ' '*$Indent +  '- uid: ' + $Entry['uid']
  $name = ' '*$Indent + '  name: ' + $entry['name']
  
  Add-Content -Path $out -Value $uid
  Add-Content -Path $out -Value $name

  if ($Entry['items'])
  {
    $items = ' '*$Indent + ' ' + 'items: '
    Add-Content -Path $out -Value $items

    foreach ($item in $Entry['items']) {
      WriteRecursive($item, $Indent + 2)
    }
  }
}

foreach ($key in $paths)
{
  #WriteRecursive($key)
}

function WriteNested($Entry)
{
  foreach ($key in $Entry.Keys) {
    switch ($key)
    {
      'uid'{ 
        Write-Host $Entry[$key] 
        Add-Content -Path $out -Value $Entry[$key] 
      }
      'name'{ 
        Write-Host $Entry[$key] 
        Add-Content -Path $out -Value $Entry[$key] 
      }
      'items'{
        foreach ($item in $items) {
          Write-Host $Entry['items'][$item]          
          Add-Content -Path $out -Value $Entry['items'][$item]
        }
      }
      'pathId'{
        foreach ($path in $Entry[$key].Keys) {
          Write-Host ($key + ":" + $path)
          WriteNested($Entry[$key][$path])
        }
      }
    }
  }
}

#$paths["SpawnThat"]["Integrations"]["CLLC"]
#$nested["pathId"]['SpawnThat']
#WriteNested($nested)

$nested['pathId']['SpawnThat']['pathId']['Lifecycle']