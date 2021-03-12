# Valheim Spawning

## SpawnSystem

- Defined as part of the _ZoneCtrl prefab. Instantiated when locations are loaded in.
- Each _ZoneCtrl will have its own SpawnSysten, with its own SpawnData associated. Changing one, will only affect that one.
- Each _ZoneCtrl will be placed at some point in the location, and will be despawned as players get out of range.
- The _ZoneCtrl is initialized with an IsOwner check, meaning only one client will be in control of all spawning.
- The _ZoneCtrl prefab is defined in only one place, the ZoneZystem. All instantiations of it will have the full list of all possible spawners.
- SpawnSystem checks each spawner it has associated for each update, for whether or not it should start spawning mobs.
	- There is a good 43 spawners by default

### Instantiated at:
ZoneSystem -> Update -> CreateLocalZones -> PokeLocalZone -> SpawnZone
or
ZoneSystem -> Update -> CreateGhostZones -> SpawnZone

In the ghost spawn mode, the zoneCtrl instantiated will be destroyed almost immediately after initialization.

### Update loop: SpawnSystem -> UpdateSpawning
Invoked every 4 seconds.

Checks for ownership as well as nearby players.

Calls UpdateSpawningList with this.m_spawners, which are the spawners designated at instantiation of prefab _ZoneCtrl.

If an event is active, it will do an additional call to UpdateSpawningList, with the spawners of the RandEventSystem.

### UpdateSpawning -> UpdateSpawningList
Checks all input spawners for valid spawn conditions.

Conditions:
1. Spawn enabled
2. In the right biome
3. Compare spawn frequency with last spawn of spawner. (I think this one actually carries across different SpawnSystems, due to the way they calculate the hashcode. Meaning index becomes very important)
-> Starts looping
4. Chance to spawn
5. A required global key is is set.
6. The environment is right.
7. Can spawn at day, and time is day.
8. Can spawn at night, and time is night.
9. There are less entities of the prefab of the spawner spawned at location zero?? than there are m_maxSpawned. This one might actually just be a waste of cpu.
10. Checks FindBaseSpawnPoint
11. SpawnDistance <= 0 or if any SpawnData.m_prefab is not within SpawnData.m_distance of the point found by FindBaseSpawnPoint.
-> Loops through random group size * 2, stopping if enough have spawned.
12. Generates a random point in a circle, then checks IsSpawnPointGood again
-> IsSpawnPointGood
-> Spawn

-> FindBaseSpawnPoint
Loops through an additional spawn condition check, up to 20 times, using a randomized position based off a random Player. 
Returns whether or not a position was found, and outs the Vector3 it was found at.

-> IsSpawnPointGood
1. Is in right biome
2. Is in right biome area
3. Position is not blocked
4. Inside altitude requirements, based on water level.
5. Position tilt acceptable
6. Spawn radius not too close to player.
7. Is position checked inside EffectArea.Type.PlayerBase
8. Is the spawn allowed in forest / outside forest, and is the location inside a forest.
9. Is ocean depth min and max equal, or is position within ocean depth range.


## CreatureSpawner

- Loaded in as part of ZoneSystem, are part of the Location objects being loaded.
- Each location is defined as part of a biome "_Zone", which has a set of potentiel locations inside it.
	- Blackforest is a "_Zone", inside blackforest could be locations:
		- Trollcave
		- Greydwarf_camp_1
- Each location will most often have a set of CreateSpawner's defined, placed relative to the center of the location transform.
- These spawners are simplified versions of the SpawnSystem, and will have their own update loop, for checking if a single enemy type should be spawned in, according to their own limits of respawn time and max.
- They each only have a specific single enemy defined.