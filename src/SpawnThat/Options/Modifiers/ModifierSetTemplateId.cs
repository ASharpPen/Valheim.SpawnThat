using UnityEngine;
using SpawnThat.Core;

namespace SpawnThat.Options.Modifiers;

public class ModifierSetTemplateId : ISpawnModifier
{
    public const string ZdoFeature = "spawn_template_id";
    public static int ZdoFeatureHash { get; } = ZdoFeature.GetStableHashCode();

    public string TemplateId { get; set; }

    public ModifierSetTemplateId()
    { }

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
