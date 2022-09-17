# MobAILib Integration

Options for setting [MobAILib](https://www.nexusmods.com/valheim/mods/1188) ai's and configuration. See the mod page for more in-depth documentation of the options. By default, only the built-in AI's will be available, but should support any customly registered by other mods.

Note, MobAI will most likely completely take over any AI related features, so don't expect things like SetTryDespawnOnAlert to work when assigning a custom ai.

# World Spawner Options

Mod-specific configs can be added to each world spawner as `[WorldSpawner.Index.MobAI]`

Example of a repair boar:

```INI 
[WorldSpawner.321]
Name = My fixing boar
PrefabName = Boar

[WorldSpawner.321.MobAI]
SetAI = Fixer
AIConfigFile=MyFixerConfig.json
```

| Setting | Type | Default | Example | Description |
| --- | --- | --- | --- | --- |
| SetAI | string | | Fixer | Name of MobAI to register for spawn. Eg. the defaults 'Fixer' and 'Worker' |
| AIConfigFile | string | | MyFixerConfig.json | Configuration file to use for the SetAI. Eg. 'MyFixerConfig.json', can include path, but will always start searching from config folder. See MobAI documentation for file setup

# Local Spawner Options

Mod-specific configs can be added to each local spawner as `[Location.PrefabName.MobAI]`

Example of a boar repairman spawning at boar runestones.

```INI
[Runestone_Boars.Boar]
Name = Repair Boar
PrefabName = Boar

[Runestone_Boars.Boar.MobAI]
SetAI = Fixer
AIConfigFile=MyFixerConfig.json
```

| Setting | Type | Default | Example | Description |
| --- | --- | --- | --- | --- |
| SetAI | string | | Fixer | Name of MobAI to register for spawn. Eg. the defaults 'Fixer' and 'Worker' |
| AIConfigFile | string | | MyFixerConfig.json | Configuration file to use for the SetAI. Eg. 'MyFixerConfig.json', can include path, but will always start searching from config folder. See MobAI documentation for file setup
