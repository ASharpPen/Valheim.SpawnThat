﻿
namespace Valheim.SpawnThat.Core.Configuration
{
    public interface IHaveSubsections
    {
        Config GetSubsection(string subsectionName);
    }
}
