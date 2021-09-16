
namespace Valheim.SpawnThat.Spawners
{
    internal static class Constants
    {
        /// <summary>
        /// Workaround for manually getting the level up chance, since it changed to a const in valheim now. 
        /// Became a magic number in v0.202.14, probably optimized away.
        /// </summary>
        public const float CreatureSpawner_LevelUpChance = 10f;
    }
}
