using UnityEngine;
using SpawnThat.Core;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Options.Modifiers;

public class ModifierSetTemplateId : ISpawnModifier
{
    public const string ZdoFeature = "spawn_template_id";
    public static int ZdoFeatureHash { get; } = ZdoFeature.HashInteger();

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
