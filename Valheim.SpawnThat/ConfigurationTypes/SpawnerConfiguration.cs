using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.SpawnThat.ConfigurationCore;

namespace Valheim.SpawnThat.ConfigurationTypes
{
    public class SpawnerConfiguration : ConfigurationGroup<SpawnConfiguration>
    {

    }

    public class SpawnConfiguration : ConfigurationSection
    {

        public ConfigurationEntry<string> PrefabName = new ConfigurationEntry<string>("Greydwarf", "");

        public ConfigurationEntry<float> SpawnMaxMultiplier = new ConfigurationEntry<float>(1, "");

        public ConfigurationEntry<float> GroupSizeMinMultiplier = new ConfigurationEntry<float>(1, "");

        public ConfigurationEntry<float> GroupSizeMaxMultiplier = new ConfigurationEntry<float>(1, "");

        public ConfigurationEntry<float> SpawnFrequencyMultiplier = new ConfigurationEntry<float>(1, "");
    }
}
