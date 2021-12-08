using System;
using UnityEngine;
using Valheim.SpawnThat.Utilities.Extensions;

namespace Valheim.SpawnThat.Debugging.Gizmos;

public abstract class Gizmo : MonoBehaviour
{
    public double? Timestamp { get; private set; }

    public TimeSpan? LifeTime { get; set; } = null;

    public float MaxDistanceToPlayer { get; set; } = 500;

    protected static Texture2D RendererTexture { get; private set; }

    protected static Shader RendererShader { get; private set; }

    static Gizmo()
    {
        RendererTexture = new Texture2D(1, 1);
        RendererTexture.SetPixel(0, 0, Color.gray);

        RendererShader = Shader.Find("Particles/Standard Unlit");
    }

    void Awake()
    {
        if (ZNet.instance != null)
        {
            Timestamp = ZNet.instance.m_netTime;
        }

        OnAwake();
    }

    protected virtual void OnAwake()
    { 
    }

    void Update()
    {
        try
        {
            if (!ZNet.instance && this.gameObject)
            {
                GameObject.Destroy(this.gameObject);
                return;
            }

            if (!Player.m_localPlayer && ZNet.instance.IsServer() && this.gameObject)
            {
                GameObject.Destroy(this.gameObject);
                return;
            }

            if (Timestamp is null)
            {
                Timestamp = ZNet.instance.m_netTime;

                if (ZNet.instance is null)
                {
                    GameObject.Destroy(this.gameObject);
                    return;
                }
            }

            if (LifeTime != null && ZNet.instance.m_netTime - Timestamp > LifeTime.Value.TotalSeconds && this.gameObject)
            {
                GameObject.Destroy(this.gameObject);
                return;
            }

            if (!Player.m_localPlayer && this.gameObject)
            {
                GameObject.Destroy(this.gameObject);
                return;
            }

            if (Player.m_localPlayer.m_nview.GetZDO().GetPosition().DistanceHorizontal(transform.position) > MaxDistanceToPlayer && this.gameObject)
            {
                GameObject.Destroy(this.gameObject);
                return;
            }

            OnUpdate();
        }
        catch { }
    }

    protected virtual void OnUpdate()
    {
    }

    protected LineRenderer CreateRenderer(Color? color = null)
    {
        var goZ = new GameObject();
        goZ.transform.parent = this.transform;
        goZ.transform.localPosition = Vector3.zero;

        var renderer = goZ.AddComponent<LineRenderer>();

        var material = new Material(RendererShader);
        material.SetColor("_Color", color ?? Color.blue);
        material.SetFloat("_BlendOp", (float)UnityEngine.Rendering.BlendOp.Subtract);
        material.SetTexture("_MainTex", RendererTexture);


        renderer.useWorldSpace = false;
        renderer.material = material;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.widthMultiplier = 0.05f;

        return renderer;
    }
}
