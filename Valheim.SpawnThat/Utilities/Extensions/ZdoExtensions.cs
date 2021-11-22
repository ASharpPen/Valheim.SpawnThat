using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.SpawnThat.Utilities.Extensions;

public static class ZdoExtensions
{
    private static int NoiseHash = "noise".GetStableHashCode();
    private static int TamedHash = "tamed".GetStableHashCode();

    public static float GetNoise(this ZDO zdo)
    {
        return zdo.GetFloat(NoiseHash);
    }

    public static bool GetTamed(this ZDO zdo)
    {
        return zdo.GetBool(TamedHash);
    }
}
