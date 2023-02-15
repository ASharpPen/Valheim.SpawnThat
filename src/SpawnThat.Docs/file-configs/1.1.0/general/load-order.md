----

⚠️ This is an archived version of the documentation. Find the latest version [here](/configs/general/intro.html) ⚠️

----

# Config Load Order

Configurations for Spawn That is loaded every time a game is started, in the order

1. API (using Spawn That through code)
    - API itself is loaded in order of when the mod registers its configuration action.
2. Custom configs 
    - The customly named configurations like `spawn_that.world_spawners.some_random_name.cfg`.
    - Loaded in the order they are found in folder structure.
3. Default config
    - The auto-generated configs like `spawn_that.world_spawners_advanced.cfg`.

Configurations that overlap get overridden by the last loaded config. For instance, if you have two configurations for the same world spawner, the later can override the settings of the first config.

The default configs are intended for letting users having the final say.