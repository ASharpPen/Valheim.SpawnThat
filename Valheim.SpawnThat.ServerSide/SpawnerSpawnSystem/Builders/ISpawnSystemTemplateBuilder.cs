using UnityEngine;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Modifiers;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Builders
{
    public interface ISpawnSystemTemplateBuilder
    {
        ISpawnSystemTemplateBuilder Initialize(GameObject prefab);

        ISpawnSystemTemplateBuilder SetIndex(uint index);

        ISpawnSystemTemplateBuilder AddSpawnCondition(ISpawnCondition condition);

        ISpawnSystemTemplateBuilder AddSpawnPositionCondition(ISpawnPositionCondition condition);

        ISpawnSystemTemplateBuilder AddSpawnModifier(ISpawnModifier modifier);

        SpawnTemplate Build();
    }
}
