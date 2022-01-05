using UnityEngine;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawn.Modifiers;

public class ModifierSetTemplateId : ISpawnModifier
{
    public const string ZdoFeature = "spawn_template_id";
    public static int ZdoFeatureHash { get; } = ZdoFeature.GetStableHashCode();

    private string TemplateId { get; set; }

    public ModifierSetTemplateId(string templateId)
    {
        TemplateId = templateId;
    }

    public void Modify(GameObject entity, ZDO entityZdo)
    {
        if (string.IsNullOrWhiteSpace(TemplateId))
        {
            return;
        }

        Log.LogTrace($"Setting template id {TemplateId}");
        entityZdo?.Set(ZdoFeatureHash, TemplateId);
    }
}
