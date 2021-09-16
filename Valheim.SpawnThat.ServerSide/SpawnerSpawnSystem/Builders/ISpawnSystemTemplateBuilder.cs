using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;

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
