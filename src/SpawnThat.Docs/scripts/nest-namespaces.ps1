[CmdletBinding()]
param (
    [Parameter()]
    [string]
    $FileIn,

    [Parameter()]
    [string]
    $FileOut
)

Import-Module powershell-yaml

Write-Host "Reading $FileIn"

$content = Get-Content -Path $FileIn -Raw

$yaml = ConvertFrom-Yaml $content

function Flatten($yml)
{
  $flattened = @()

  foreach ($dic in $yml) {

    if ($dic.Contains('items'))
    {
      foreach ($nestedDic in $dic['items']) {
        $paths = $nestedDic['uid'].split('.');
        $flattened += 
          [ordered]@{
            uid = $nestedDic['uid']
            name = $paths[-1]
            path = $paths[0..($paths.Length-2)]
          }
      }
    }
  }

  return $flattened
}

$flat = Flatten($yaml)

# Strip SpawnThat from names and namespaces
foreach ($item in $flat) {
  $item.name = $item.name.Replace("SpawnThat", "")
  if ($item.path.Length -gt 0)
  {
    $item.path = $item.path[1..($item.path.Length-1)]
  }
}

$nested = [ordered]@{
  path = [ordered]@{}
}

function AddItemNested($entry)
{
  $layer = $nested

  $depth = 0;

  $combinedPath = ""

  foreach ($layerKey in $entry.path) {

    if ($combinedPath.Length -eq 0){
      $combinedPath += $layerKey
    }
    else {
      $combinedPath += "." + $layerKey
    }

    if ($null -eq $layer.path)
    {
      Write-Host $entry.path
      Write-Host "Path is null for $combinedPath"
      return
    }

    if ($layer.path.Contains($layerKey))
    {
      #Write-Host "Existing: $combinedPath"
      $layer = $layer.path[$layerKey]
    }
    else {
      #Write-Host ("New path $combinedPath")

      $newLayer = [ordered]@{
        uid = ' '*$depth + '- uid: ' + $combinedPath
        name = ' '*$depth + '  name: ' + $layerKey        
        path = [ordered]@{}
      }
      $layer.path[$layerKey] = $newLayer
      $layer = $newLayer
    }
    $depth += 2;
  }

  if (-not $layer.Contains('items'))
  {
    $layer['items'] = @()
  }

  $layer.items += [ordered]@{
    uid = ' '*$depth + '- uid: ' + $entry.uid
    name = ' '*$depth + '  name: ' + $entry.name
  }
}

foreach ($entry in $flat) {
  AddItemNested($entry)
}

function RecursiveWrite($dic)
{
  foreach ($layerKey in $dic.Keys) {

    $layer = $dic[$layerKey]

    Add-Content -Path $FileOut -Value $layer.uid
    Add-Content -Path $FileOut -Value $layer.name

    $itemsAdded = $false

    if ($layer.Contains('items') -and
        $layer.items.Count -gt 0)
    {
      #TODO: Need to gather the indent automatically.
      $indent = ' '*2*($layer.uid.split('.').Length)
      Add-Content -Path $FileOut -Value ($indent + "items: ")

      $itemsAdded = $true

      foreach ($item in $layer.items) {
        Add-Content -Path $FileOut -Value $item.uid
        Add-Content -Path $FileOut -Value $item.name
      }
    }

    if ($layer.path.Count -gt 0)
    {
      if (-not $itemsAdded)
      {
        $indent = ' '*2*($layer.uid.split('.').Length)
        Add-Content -Path $FileOut -Value ($indent + "items: ")
      }

      RecursiveWrite($layer.path)
    }
  }
}

# Replace existing file
Clear-Content -Path $FileOut

# Write new
RecursiveWrite($nested.path)

#return $nested['path']['SpawnThat']['path']['Integrations']['path']['CLLC']['path']['Modifiers']['items']
