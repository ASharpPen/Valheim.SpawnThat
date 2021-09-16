
using UnityEngine;

namespace Valheim.SpawnThat.Utilities.Extensions
{
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
            return ZoneUtils.GetZone((int)position.x, (int)position.y);
        }
    }
}
