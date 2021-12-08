using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Valheim.SpawnThat.Debugging.Gizmos;

public class LineGizmo : Gizmo
{
    public LineRenderer Renderer { get; private set; }

    public static void Create(Vector3 position, TimeSpan? lifeTime = null)
    {
#if DEBUG
        var gameObject = new GameObject();
        gameObject.transform.position = position;

        var gizmo = gameObject.AddComponent<LineGizmo>();
        gizmo.LifeTime = lifeTime;

        gameObject.SetActive(true);
#endif
    }

    protected override void OnAwake()
    {
        Renderer = CreateRenderer();

        var segments = new Vector3[]
        {
            Vector3.up * -10000,
            Vector3.up * 10000,
        };

        Renderer.positionCount = segments.Length;
        Renderer.SetPositions(segments);
    }
}
