using UnityEngine;

namespace SpawnThat.World.Maps.Images.Colors;

internal interface IColorMapper<T>
{
    public Color Map(T cell);
}
