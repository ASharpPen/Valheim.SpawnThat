﻿using System;

namespace Valheim.SpawnThat.Core.Configuration
{
    [Serializable]
    public abstract class Config
    {
        public string SectionName;

        public string SectionKey;
    }
}
