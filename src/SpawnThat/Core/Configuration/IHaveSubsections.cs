
namespace SpawnThat.Core.Configuration;

internal interface IHaveSubsections
{
    Config GetSubsection(string subsectionName);
}
