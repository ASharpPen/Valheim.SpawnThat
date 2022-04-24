using UnityEngine;
using SpawnThat.World.Zone;

namespace SpawnThat.Utilities.Extensions;

public static class Vector3Extensions
{
    public static bool WithinSquare(this Vector3 position, int centerX, int centerZ, int size = 10)
    {
        float posX = position.x;
        float posZ = position.z;

        if (posX < (centerX - size) || posX > (centerX + size))
        {
            return false;
        }

        if (posZ < (centerZ - size) || posZ > (centerZ + size))
        {
            return false;
        }

        return true;
    }

    public static Vector2i GetZoneId(this Vector3 position)
    {
        return ZoneUtils.GetZone((int)position.x, (int)position.z);
    }

    public static float DistanceHorizontal(this Vector3 source, Vector3 destination)
    {
        float dx = source.x - destination.x;
        float dz = source.z - destination.z;
        return Mathf.Sqrt(dx * dx + dz * dz);
    }

    public static bool WithinHorizontalDistance(this Vector3 pos1, Vector3 pos2, float distance)
    {
        float x = pos1.x - pos2.x;
        float z = pos1.z - pos2.z;
        return x * x + z * z < distance * distance;
    }
}
