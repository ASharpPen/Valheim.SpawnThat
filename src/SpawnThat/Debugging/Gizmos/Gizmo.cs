using System;
using SpawnThat.Core;
using SpawnThat.Utilities.Extensions;
using UnityEngine;

namespace SpawnThat.Debugging.Gizmos;

internal abstract class Gizmo : MonoBehaviour
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

    void Update()
    {
        try
        {
            if (!gameObject)
            {
                return;
            }

            // Ignore while znet and player is not loaded.
            if (ZNet.instance != null && ZNet.instance && Player.m_localPlayer)
            {
                if (Timestamp == null && ZNet.instance != null && ZNet.instance)
                {
                    Timestamp = ZNet.instance.GetTimeSeconds();
                };

                if (LifeTime != null && ZNet.instance.m_netTime - Timestamp > LifeTime.Value.TotalSeconds && this.gameObject)
                {
                    GameObject.Destroy(this.gameObject);
                    Log.LogTrace($"{GetType().Name} too old. Removing object.");
                    return;
                }

                if (!Player.m_localPlayer)
                {
                    GameObject.Destroy(this.gameObject);
                    Log.LogTrace($"No local player for {GetType().Name}. Removing object.");
                    return;
                }

                var dist = Player.m_localPlayer.m_nview.GetZDO().GetPosition().DistanceHorizontal(transform.position);
                if (dist > MaxDistanceToPlayer && this.gameObject)
                {
                    GameObject.Destroy(this.gameObject);
                    Log.LogTrace($"{GetType().Name} is {dist} await from player, at max {MaxDistanceToPlayer}. Removing object.");
                    return;
                }

                OnUpdate();
            }
        }
        catch (Exception e)
        {
#if DEBUG
            Log.LogError($"Error during {this.GetType().Name}.Update", e);
#endif
        }
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

        goZ.SetActive(true);

        return renderer;
    }
}
