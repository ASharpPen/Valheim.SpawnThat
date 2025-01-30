using UnityEngine;

namespace SpawnThat.Utilities.Spatial;

internal interface IBox
{
    Vector3 Pos { get; }

    Quaternion Rotation { get; }

    Vector3Int Size { get; }
}
