using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.SpawnThat.Maps
{

    public class WorldGeneratorAreaProvider : IAreaProvider
    {
        private WorldGenerator _instance = null;

        private WorldGenerator World => _instance ??= WorldGenerator.instance;

        public int GetArea(int x, int y)
        {
            return (int)World.GetBiome(x, y);
        }
    }
}
