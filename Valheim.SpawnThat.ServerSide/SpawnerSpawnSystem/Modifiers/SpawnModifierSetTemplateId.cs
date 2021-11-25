using UnityEngine;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Modifiers;

public class SpawnModifierSetTemplateId : ISpawnModifier
{
    public const string ZdoFeature = "spawn_template_id";
    public static int ZdoFeatureHash { get; } = ZdoFeature.GetStableHashCode();

    public string Id { get; }

    public SpawnModifierSetTemplateId(string id)
    {
        Id = id;
    }

    public void Modify(Models.SpawnContext context, GameObject entity, ZDO entityZdo)
    {
        Log.LogTrace($"Setting template id {Id}");

        // TODO: For some reason it refuses to recognize Set(int, string)... Maybe a reference is wrong somewhere or something... I only get 16 overloads instead of the expected 17.
        entityZdo?.Set(ZdoFeature, Id);
    }
}
