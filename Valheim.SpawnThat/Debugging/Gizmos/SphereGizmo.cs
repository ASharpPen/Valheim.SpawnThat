using System;
using UnityEngine;

namespace Valheim.SpawnThat.Debugging.Gizmos;

/// <summary>
/// Sphere drawing inspired by https://github.com/JereKuusela/valheim-esp
/// </summary>
public class SphereGizmo : Gizmo
{
    public LineRenderer RenderCircleY { get; set; }
    public LineRenderer RenderCircleX { get; set; }
    public LineRenderer RenderCircleZ { get; set; }

    public float Radius { get; set; } = 3;

    public Color Color { get; private set; }

    public static void Create(Vector3 position, float radius, Color? color = null, TimeSpan? lifeTime = null)
    {
#if !DEBUG
        return;
#endif
        if (ZNet.instance.IsDedicated())
        { 
            // Don't do anything while on a server.
            return;
        }

        var gameObject = new GameObject();
        gameObject.transform.position = position;

        var gizmo = gameObject.AddComponent<SphereGizmo>();
        gizmo.Radius = radius;
        gizmo.LifeTime = lifeTime;
        gizmo.Color = color ?? Color.blue;

        gizmo.Init();

        gameObject.SetActive(true);
    }

    public static void Create(GameObject parent, float radius, Color? color = null, TimeSpan? lifeTime = null)
    {
#if !DEBUG
        return;
#endif
        if (ZNet.instance.IsDedicated())
        {
            // Don't do anything while on a server.
            return;
        }

        var gameObject = new GameObject();
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.localScale = Vector3.zero;

        var gizmo = gameObject.AddComponent<SphereGizmo>();
        gizmo.Radius = radius;
        gizmo.LifeTime = lifeTime;
        gizmo.Color = color ?? Color.blue;

        gizmo.Init();

        gameObject.SetActive(true);
    }

    public void Init()
    {
        RenderCircleY = CreateRenderer(Color);
        RenderCircleX = CreateRenderer(Color);
        RenderCircleZ = CreateRenderer(Color);

        var positionsY = GetCircleSegments1(Radius);
        RenderCircleY.positionCount = positionsY.Length;
        RenderCircleY.SetPositions(positionsY);

        var positionsX = GetCircleSegments2(Radius);
        RenderCircleX.positionCount = positionsX.Length;
        RenderCircleX.SetPositions(positionsX);

        var positionsZ = GetCircleSegments3(Radius);
        RenderCircleZ.positionCount = positionsZ.Length;
        RenderCircleZ.SetPositions(positionsZ);
    }

    protected override void OnUpdate()
    {
        var positionsY = GetCircleSegments1(Radius);
        RenderCircleY.positionCount = positionsY.Length;
        RenderCircleY.SetPositions(positionsY);

        var positionsX = GetCircleSegments2(Radius);
        RenderCircleX.positionCount = positionsX.Length;
        RenderCircleX.SetPositions(positionsX);

        var positionsZ = GetCircleSegments3(Radius);
        RenderCircleZ.positionCount = positionsZ.Length;
        RenderCircleZ.SetPositions(positionsZ);
    }

    private static Vector3[] GetCircleSegments1(float radius, int segments = 32)
    {
        Vector3[] circleSegments = new Vector3[segments + 1];

        var angleIncrement = 360f / segments;

        for (int i = 0; i <= segments; i++)
        {
            var rad = Mathf.Deg2Rad * angleIncrement * i;
            circleSegments[i] = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad)) * radius;
        }

        return circleSegments;
    }

    private static Vector3[] GetCircleSegments2(float radius, int segments = 32)
    {
        Vector3[] circleSegments = new Vector3[segments + 1];

        var angleIncrement = 360f / segments;

        for (int i = 0; i <= segments; i++)
        {
            var rad = Mathf.Deg2Rad * angleIncrement * i;
            circleSegments[i] = new Vector3(0, Mathf.Sin(rad), Mathf.Cos(rad)) * radius;
        }

        return circleSegments;
    }

    private static Vector3[] GetCircleSegments3(float radius, int segments = 32)
    {
        Vector3[] circleSegments = new Vector3[segments + 1];

        var angleIncrement = 360f / segments;

        for (int i = 0; i <= segments; i++)
        {
            var rad = Mathf.Deg2Rad * angleIncrement * i;
            circleSegments[i] = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0) * radius;
        }

        return circleSegments;
    }
}