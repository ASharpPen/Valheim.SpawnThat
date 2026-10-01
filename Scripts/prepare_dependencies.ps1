# Path to mod-profile dir (Eg., AppData\Roaming\r2modmanPlus-local\Valheim\profiles\my-profile)
$profilePath = ""

# Path to Valheim\valheim_Data\Managed dir.
$valheimPath = ""

$output = "..\src\Libs\";
$nstripDir = ".\NStrip"

function Strip
{
  [CmdletBinding()]
  param (
    [Parameter(Mandatory=$true, Position=0)]
    [string]
    $Source,

    [Parameter(Mandatory=$true, Position=1)]
    [string]
    $TargetDir,

    [Parameter(Mandatory=$false, Position=2)]
    [switch]
    $Publicize
  )

  $out = "$output\$TargetDir"

  if (-not(Test-Path $out))
  {
    New-Item -ItemType Directory -Path $out
  }

  if (Test-Path -Path $Source -PathType Leaf)
  {
    $file = Split-Path $Source -Leaf
    $out = Join-Path $out $file
  }

  if($Publicize) { 
    & $nstripDir\Nstrip.exe -p -cg -d $valheimPath $Source $out
  }
  else {
    & $nstripDir\Nstrip.exe -cg -d $valheimPath $Source $out
  }
  
  Write-Host "-Source $Source -TargetDir $out"
}

function CopyFile
{
  [Cmdletbinding()]
  param(
    [Parameter(Mandatory=$true, Position=0)]
    [string]
    $Source,

    [Parameter(Mandatory=$true, Position=1)]
    [string]
    $TargetDir
  )

  $out = "$output\$TargetDir"

  if (-not(Test-Path $out))
  {
    New-Item -ItemType Directory -Path $out
  }

  if (Test-Path -Path $Source -PathType Leaf)
  {
    Copy-Item $Source -Destination $out
  }

  Write-Host "-Source $Source -TargetDir $out"
}

# NStrip
if (-not(Test-Path($nstripDir)))
{
  New-Item -ItemType Directory -Path $nstripDir
}

if (-not(Test-Path "$nstripDir\NStrip.exe"))
{
  Invoke-WebRequest "https://github.com/bbepis/NStrip/releases/download/v1.4.1/NStrip.exe" -OutFile "$nstripDir\NStrip.exe"
}

# BepInEx 
CopyFile "$profilePath\BepInEx\core\BepInEx.dll" "BepInEx"
CopyFile "$profilePath\BepInEx\core\0Harmony.dll" "BepInEx"

# Valheim
Strip "$valheimPath\assembly_valheim.dll" "Valheim" -Publicize
Strip "$valheimPath\assembly_utils.dll" "Valheim" -Publicize
Strip "$valheimPath\SoftReferenceableAssets.dll" "Valheim" -Publicize

# Unity
CopyFile "$valheimPath\UnityEngine.dll" "Unity"
CopyFile "$valheimPath\UnityEngine.CoreModule.dll" "Unity"
CopyFile "$valheimPath\UnityEngine.PhysicsModule.dll" "Unity"
CopyFile "$valheimPath\UnityEngine.ImageConversionModule.dll" "Unity"
CopyFile "$valheimPath\UnityEngine.UI.dll" "Unity"

# Integrated mods
Strip "$profilePath\BepInEx\plugins\RandyKnapp-EpicLoot" "RandyKnapp-EpicLoot" -Publicize
Strip "$profilePath\BepInEx\plugins\Smoothbrain-CreatureLevelAndLootControl" "Smoothbrain-CreatureLevelAndLootControl"
Strip "$profilePath\BepInEx\plugins\Oran1-MobAILib" "RagnarsRokare-MobAILib" -Publicize

Write-Host "Done"