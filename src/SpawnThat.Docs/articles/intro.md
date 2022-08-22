Welcome to the Spawn That documentation!

Spawn That! is an advanced tool for configuring all world spawners.

With this, it is possible to change almost all of the default settings for the way spawners work in Valheim.
Want to have a world without trolls? Possible! (probably)
Want to have a world with ONLY trolls? Possible! (almost)
Want to have a world where greydwarves only spawn at night? Possible!
Just want to have more/less of a mob type? Simple modifiers exist!

## Features

- Change spawning rates of specific mobs
- Replace existing spawn configurations throughout the world
- Set almost any of the default parameters the game uses
- Add your own spawn configuration to the world
- Modify the localized spawners by mob type and location
- Dump existing game templates as files using the same format as the mod configs. 
	- Easy to copy-paste and change the parts you want.
	- Investigate what the world throws at you.
- Server-side configs
- Modify the spawners in camps, villages and dungeons
- Conditions and settings specific to integrated mods:
	- [Creature Level and Loot Control](https://valheim.thunderstore.io/package/Smoothbrain/CreatureLevelAndLootControl/)
	- [MobAILib](https://www.nexusmods.com/valheim/mods/1188)
	- [Epic Loot](https://valheim.thunderstore.io/package/RandyKnapp/EpicLoot/)

## How does it all work?

Valheim's main way of managing spawns work by having multiple types of spawners spread throughout the world.

The world spawners, for which the world has them spread out in a grid fashion, each using the same list of templates to check if something should spawn. These are the general spawners and all currently use the same 44 templates.

The local spawners, which are intended for fine-tuned spawning. Local spawners only spawn one specific mob type, and only has one alive at a time. These are bound to specific world locations, such as the surtling firehole.

SpawnArea spawners, like greydwarf nests and draugr piles. These are generated based on locations, like local spawners, but do not keep track of their spawns, just a general idea of what is in the area. And they can be destroyed.

For world spawners, you can either replace existing templates based on their index, or add to the list of possible templates to spawn from.

For local spawners, since these are more custom, you describe a location and the prefab name of the mob you what you want to override.

For SpawnArea spawners, the configuration settings are still in development.

The mod modification happens at run-time, once for each spawner. Reloading the world resets all changes.
As the player moves through the world, the game loads in the various spawners, and the mod applies its own settings.

## Client / Server

Spawn That needs to be installed on all clients (and server) to work.

From v0.3.0 clients will request the configurations currently loaded by the server, and use those without affecting the clients config files.
This means you should be able to have server-specific configurations, and the client can have its own setup for singleplayer.
For this to work, the mod needs to be installed on the server, and have configs set up properly there. When players join with Spawn That v0.3.0, their mod will use the servers configs.

## FAQ
- I keep getting the warning "Unable to find find prefab"
	- This happens when Spawn That is trying to retrieve a prefab that it has been told to find, but the prefab has either not been properly loaded by whatever mod was responsible for it, or it is mistyped.
	- Generally this warning seem to occur with relation to the RRR-series of mods, but there is nothing Spawn That can do about it.

- Where do I find the prefab names?
	- Multiple pages have long lists, you can check out this one [here](https://gist.github.com/Sonata26/e2b85d53e125fb40081b18e2aee6d584), or the [valheim wiki](https://valheim.fandom.com/wiki/Creatures)

- The config files are empty?
	- All configs but `spawn_that.cfg` and `spawn_that.simple.cfg` are intended empty initially. You need to fill in the sections yourself.
	- Enable any of the "WriteX" in spawn_that.cfg, and files containing the default values will be created in the plugins folder.
	- If it just won't work for you for some reason, the files can be found on this site under `Data`.

## Support

If you feel like it

<a href="https://www.buymeacoffee.com/asharppen"><img src="https://img.buymeacoffee.com/button-api/?text=Buy me a coffee&emoji=&slug=asharppen&button_colour=FFDD00&font_colour=000000&font_family=Cookie&outline_colour=000000&coffee_colour=ffffff" /></a>