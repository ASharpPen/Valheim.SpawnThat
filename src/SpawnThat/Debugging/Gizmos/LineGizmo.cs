using System;
using UnityEngine;

namespace SpawnThat.Debugging.Gizmos;

internal class LineGizmo : Gizmo
{
    public LineRenderer Renderer { get; private set; }

    public Color? Color { get; private set; }

    public bool UpdateRenderer { get; private set; }

    public static void Create(Vector3 position, Color? color = null, TimeSpan? lifeTime = null)
    {
#if !DEBUG
        return;
#endif
        var gameObject = new GameObject();
        gameObject.transform.position = position;

        var gizmo = gameObject.AddComponent<LineGizmo>();
        gizmo.LifeTime = lifeTime;
        gizmo.Color = color;

        gizmo.Init();

        gameObject.SetActive(true);
    }

    public static void Create(GameObject parent, Color? color = null, TimeSpan? lifeTime = null)
    {
#if !DEBUG
        return;
#endif
        var gameObject = new GameObject();
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.localPosition = Vector3.zero;

        var gizmo = gameObject.AddComponent<LineGizmo>();
        gizmo.LifeTime = lifeTime;
        gizmo.Color = color;
        gizmo.UpdateRenderer = true;

        gizmo.Init();

        gameObject.SetActive(true);
    }

    protected void Init()
    {
        Renderer = CreateRenderer(Color);

        var segments = new Vector3[]
        {
            Vector3.up * -1000,
            Vector3.up * 1000,
        };

        Renderer.positionCount = segments.Length;
        Renderer.SetPositions(segments);
    }

    protected override void OnUpdate()
    {
        if (!UpdateRenderer)
        {
            return;
        }

        var segments = new Vector3[]
        {
            Vector3.up * -1000,
            Vector3.up * 1000,
        };

        //segments[0] += transform.position;

        Renderer.positionCount = segments.Length;
        Renderer.SetPositions(segments);
    }
}
