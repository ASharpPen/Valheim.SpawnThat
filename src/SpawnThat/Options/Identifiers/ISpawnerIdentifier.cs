using System;
using SpawnThat.Spawners.Contexts;

namespace SpawnThat.Options.Identifiers;

public interface ISpawnerIdentifier : IEquatable<ISpawnerIdentifier>
{
    /// <summary>
    /// When identifier is evaluated as a match, it will add this value to the match score.
    /// 
    /// Intended to select the more specific match, when multiple potential templates are
    /// identified for a spawner.
    /// </summary>
    int GetMatchWeight();

    bool IsValid(IdentificationContext context);

    /// <summary>
    /// Code for caching or quickly comparing the parameters of identifiers with same type.
    /// 
    /// Is not expected to be unique across identifiers with different concrete types.
    /// </summary>
    long GetParameterHash();
}
