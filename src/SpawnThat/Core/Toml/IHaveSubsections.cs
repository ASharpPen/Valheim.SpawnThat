namespace SpawnThat.Core.Toml;

internal interface IHaveSubsections
{
    TomlConfig GetSubsection(string subsectionName);
}
