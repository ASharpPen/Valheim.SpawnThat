using UnityEngine;
using Valheim.SpawnThat.Utilities.World;

namespace Valheim.SpawnThat.ServerSide.Utilities.World;

internal static class Environment
{
    public static EnvSetup GetCurrent(Vector3 position)
    {
        var biome = WorldData.GetZone(position).Biome;

        var potentialEnvs = EnvMan.instance.GetAvailableEnvironments(biome);

        // Pick env by seeded random. Kinda odd, but whatever, it's how Valheim does it.

        // TODO: Consider shifting time by -transition period, to fake the delayed change?
        var randomSeed = ((long)ZNet.instance.GetTimeSeconds()) / EnvMan.instance.m_environmentDuration;
        var existingRandomSeed = UnityEngine.Random.state;
        UnityEngine.Random.InitState((int)randomSeed);

        var currentEnv = EnvMan.instance.SelectWeightedEnvironment(potentialEnvs);

        // Reset random to before our little random hacking.
        UnityEngine.Random.state = existingRandomSeed;

        return currentEnv;
    }
}
