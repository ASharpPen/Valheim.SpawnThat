----

⚠️ This is an archived version of the documentation. Find the latest version [here](/configs/general/intro.html) ⚠️

----

# Creature Level and Loot Control Integration

Additional options for [Creature Level and Loot Control](https://valheim.thunderstore.io/package/Smoothbrain/CreatureLevelAndLootControl/).
See the mod page for more in-depth documentation for the options.

Note, when CLLC is installed, it will generally take over management of difficulty. This means all levels settings will be controlled by CLLC configurations, unless specifically disabled with one of the below Spawn That settings.

# World Spawner Options

Mod-specific configs can be added to each world spawner as `[WorldSpawner.ID.CreatureLevelAndLootControl]`

Example of a world spawner template, for a boar that will spawn with a fire infusion, when the CLLC world level is high enough.

``` INI
[WorldSpawner.1]
Name = FireBoar
PrefabName = Boar

[WorldSpawner.1.CreatureLevelAndLootControl]
ConditionWorldLevelMin = 3
SetInfusion = Fire
```

| Setting | Type | Default | Example | Description |
| --- | --- | --- | --- | --- |
| ConditionWorldLevelMin | int | -1 | 3 | Minimum CLLC world level for spawn to activate. Negative value disables this condition |
| ConditionWorldLevelMax | int | -1 | 4 | Maximum CLLC world level for spawn to active. Negative value disables this condition | 
| SetInfusion | string | | Fire | Assigns the specified infusion to creature spawned. Ignored if empty |
| SetExtraEffect | string | | Quick | Assigns the specified effect to creature spawned. Ignored if empty. |
| SetBossAffix | string | | Shielded | Assigns the specified boss affix to creature spawned. May not work for anything but the default 5 bosses. Ignored if empty |
| UseDefaultLevels | bool | false | true | Use the default LevelMin and LevelMax for level assignment, ignoring the usual CLLC level control |

# Local Spawner Options

Mod-specific configs can be added to each local spawner as `[Location.PrefabName.CreatureLevelAndLootControl]`

Example of local spawners around the boar runestone, which will always spawn with a fire infusion.

``` INI
[Runestone_Boars.Boar]
PrefabName = Boar
Enabled = true

[Runestone_Boars.Boar.CreatureLevelAndLootControl]
SetInfusion = Fire
```

| Setting | Type | Default | Example | Description |
| --- | --- | --- | --- | --- |
| ConditionWorldLevelMin | int | -1 | 3 | Minimum CLLC world level for spawn to activate. Negative value disables this condition |
| ConditionWorldLevelMax | int | -1 | 4 | Maximum CLLC world level for spawn to active. Negative value disables this condition | 
| SetInfusion | string | | Fire | Assigns the specified infusion to creature spawned. Ignored if empty |
| SetExtraEffect | string | | Quick | Assigns the specified effect to creature spawned. Ignored if empty. |
| SetBossAffix | string | | Shielded | Assigns the specified boss affix to creature spawned. May not work for anything but the default 5 bosses. Ignored if empty |
| UseDefaultLevels | bool | false | true | Use the default LevelMin and LevelMax for level assignment, ignoring the usual CLLC level control |

# SpawnArea Options

Mod-specific configs can be added to each spawnarea spawner spawn as `[Name.ID.CreatureLevelAndLootControl]`.

Example of a greydwarf spawner which will be changed to spawn with a fire infusion.

``` INI
[GreydwarvesOnFire]
IdentifyByName = Greydwarf_Nest

[GreydwarvesOnFire.0]
PrefabName = Greydwarf

[GreydwarvesOnFire.0.CreatureLevelAndLootControl]
SetInfusion = Fire
```

| Setting | Type | Default | Example | Description |
| --- | --- | --- | --- | --- |
| ConditionWorldLevelMin | int | -1 | 3 | Minimum CLLC world level for spawn to activate. Negative value disables this condition |
| ConditionWorldLevelMax | int | -1 | 4 | Maximum CLLC world level for spawn to active. Negative value disables this condition | 
| SetInfusion | string | | Fire | Assigns the specified infusion to creature spawned. Ignored if empty |
| SetExtraEffect | string | | Quick | Assigns the specified effect to creature spawned. Ignored if empty. |
| SetBossAffix | string | | Shielded | Assigns the specified boss affix to creature spawned. May not work for anything but the default 5 bosses. Ignored if empty |
| UseDefaultLevels | bool | false | true | Use the default LevelMin and LevelMax for level assignment, ignoring the usual CLLC level control |

# Field Options

## Boss Affixes
- None
- Reflective
- Shielded
- Mending
- Summoner
- Elementalist
- Enraged
- Twin

## Extra Effects
- None
- Aggressive
- Quick
- Regenerating
- Curious
- Splitting
- Armored

## Infusions
- None
- Lightning
- Fire
- Frost
- Poison
- Chaos
- Spirit