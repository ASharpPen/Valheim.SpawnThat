# World Spawner - Simple Config

All of this might be more complicated than what you need or want. Therefore, `spawn_that.simple.cfg` exists to provide simpler modifiers to world spawner mobs.
These will simply scale the number of mobs up or down.

Be aware, these will be applied after any other configurations to world spawners have been set. 
Meaning if you have 10 times spawning in your `spawn_that.world_spawners_advanced.cfg`, and the same in the simple config, you are going to end up with 100x spawning.

## Example

```INI
[Boar_A_Lot]
PrefabName = Boar
Enable = true
SpawnMaxMultiplier = 10
GroupSizeMinMultiplier = 1.5
GroupSizeMaxMultiplier = 2
SpawnFrequencyMultiplier = 10

[Less_Greydwarf]
PrefabName = Greydwarf
SpawnMaxMultiplier = 0.5
SpawnFrequencyMultiplier = 0.1
```

## Config Options 

| Setting | Type | Default | Example | Description |
| --- | --- | --- | --- | --- |
| PrefabName | string | Greydwarf | Deer | Prefab name of entity to modify |
| Enable | bool | true | false | Toggle this set of modifiers |
| SpawnMaxMultiplier | float | 1 | 2.5 | Change maximum of total spawned entities. 2 means twice as many. Multiplies MaxSpawned. |
| GroupSizeMinMultiplier | float | 1 | 1.5 | Change min number of entities that will spawn at once. 2 means twice as many. |
| GroupSizeMaxMultiplier | float | 1 | 1.8 | Change max number of entities that will spawn at once. 2 means twice as many. |
| SpawnFrequencyMultiplier | float | 1 | 0.5 | Change how often the game will try to spawn in new creatures. Higher means more often. 2 is twice as often, 0.5 is double the time between spawn checks |