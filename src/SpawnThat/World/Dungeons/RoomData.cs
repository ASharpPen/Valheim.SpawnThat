using UnityEngine;
using SpawnThat.Utilities.Spatial;
using SpawnThat.Core.Cache;

namespace SpawnThat.World.Dungeons;

public sealed class RoomData : IPoint, IHaveVector3, IBox, IManagedValue
{
    private Vector3 _pos;
    private Vector2[] RotatedRectangle { get; set; }

    public string Name;

    public Vector3Int Size { get; set; }

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

    public Quaternion Rotation { get; set; }

    public void OnCreate() { }

    public void OnDestroy()
    {
        RoomManager.RemoveRoom(this);
    }

    public bool Contains(Vector3 pos)
    {
        RotatedRectangle ??= SearchSpatial.CreateRectangle(this);

        return SearchSpatial.Contains(this, pos);
    }
}
