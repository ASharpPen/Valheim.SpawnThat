Additional console commands are added for debugging purposes.

- `spawnthat room` prints if in a dungeon room and which one
- `spawnthat area` prints the area id of the players current location
- `spawnthat arearoll <index>` prints the rolled chance for a template, in area player is currently in
- `spawnthat arearoll <index> <x> <y>` prints the rolled chance for a template, in the area with indicated coordinates
- `spawnthat arearollheatmap <index>` prints a png map of area rolls for a template to disk.
- `spawnthat wheredoesitspawn <index>` prints a png map of areas in which the world spawner template with <index> spawns to disk.

All the indexes mentioned refer to the number used in your WorldSpawn template.

Eg. 
To print a png of where `[WorldSpawner.321]` is allowed to spawn, write the command `spawnthat wheredoesitspawn 321`
