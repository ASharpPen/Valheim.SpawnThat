using SpawnThat.Spawners.Contexts;

namespace SpawnThat.Options.Identifiers;

public interface ISpawnerIdentifier
{
    /// <summary>
    /// When identifier is evaluated as a match, it will add this value to the match score.
    /// 
    /// Intended to select the more specific match, when multiple potential templates are
    /// identified for a spawner.
    /// </summary>
    int GetMatchWeight();

    bool IsValid(IdentificationContext context);
}
