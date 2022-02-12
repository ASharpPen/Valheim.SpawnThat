using UnityEngine;
using SpawnThat.Utilities.Spatial;

namespace SpawnThat.World.Dungeons;

public class RoomData : IPoint, IHaveVector3
{
    public string Name;

    public Vector3Int Size;

    private Vector3 _pos;

    public Vector3 Pos
    {
        get
        {
            return _pos;
        }

        set
        {
            _pos = value;
            X = value.x;
            Y = value.z;
        }
    }

    public float X { get; set; }
    public float Y { get; set; }
}
