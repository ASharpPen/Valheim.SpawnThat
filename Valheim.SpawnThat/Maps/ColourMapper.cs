using System;

namespace Valheim.SpawnThat.Maps
{
    public static class ColourMapper
    {
        public static (int r, int g, int b) IntegerToColor255(int value)
        {
            int r;
            int g;
            int b;

            if (value > 255)
            {
                b = 255;
                value -= 255;
            }
            else
            {
                b = value;
                return (0, 0, b);
            }

            if (value > 255)
            {
                g = 255;
                value -= 255;
            }
            else
            {
                g = value;
                return (0, g, b);
            }

            if (value > 255)
            {
                r = 255;
            }
            else
            {
                r = value;
            }
            return (r, g, b);
        }

        public static (int r, int g, int b) IntegerToColour3Byte(int value)
        {
            var bytes = BitConverter.GetBytes(value);
            return (bytes[2], bytes[1], bytes[0]);
        }

        public static (float r, float g, float b) IntegerToColor(int value)
        {
            var (r, g, b) = IntegerToColour3Byte(value);
            return (r / 255, g / 255, b / 255);
        }

        public static (float r, float g, float b) Rainbow255(int id)
        {
            // Feels inefficient. Gotta look into a fix.
            Random rnd = new Random(id);

            var scale = (float)rnd.NextDouble();
            var type = (int)(scale * 6);

            int ascending = (int)(scale * 255);
            int descending = 1 - ascending;

            switch (type)
            {
                case 0:
                    return (255, ascending, 0);
                case 1:
                    return (descending, 255, 0);
                case 2:
                    return (0, 255, ascending);
                case 3:
                    return (0, descending, 255);
                case 4:
                    return (ascending, 0, 255);
                default: // case 5:
                    return (255, 0, descending);
            }
        }

        public static (float r, float g, float b) Rainbow(int id)
        {
            // Feels inefficient. Gotta look into a fix.
            Random rnd = new Random(id);

            var scale = (float)rnd.NextDouble();

            var type = (int)(scale * 6);

            float ascending = scale;
            float descending = 1 - ascending;

            switch (type)
            {
                case 0:
                    return (1, ascending, 0);
                case 1:
                    return (descending, 1, 0);
                case 2:
                    return (0, 1, ascending);
                case 3:
                    return (0, descending, 1);
                case 4:
                    return (ascending, 0, 1);
                default: // case 5:
                    return (1, 0, descending);
            }
        }

        public static (int r, int g, int b) ToBiomeColour255(int biome)
        {
            return (Heightmap.Biome)biome switch
            {
                Heightmap.Biome.None => (0, 0, 0),
                Heightmap.Biome.Meadows => (0, 255, 0),
                Heightmap.Biome.Swamp => (128, 0, 0),
                Heightmap.Biome.Mountain => (255, 255, 255),
                Heightmap.Biome.BlackForest => (0, 128, 0),
                Heightmap.Biome.Plains => (255, 255, 0),
                Heightmap.Biome.AshLands => (255, 0, 0),
                Heightmap.Biome.DeepNorth => (0, 255, 255),
                Heightmap.Biome.Ocean => (0, 0, 255),
                Heightmap.Biome.Mistlands => (128, 128, 128),
                Heightmap.Biome.BiomesMax => (0, 0, 0),
                _ => (0, 0, 0)
            };
        }

        public static (float r, float g, float b) ToBiomeColour(Heightmap.Biome biome)
        {
            return biome switch
            {
                Heightmap.Biome.None => (0, 0, 0),
                Heightmap.Biome.Meadows => (0, 1, 0),
                Heightmap.Biome.Swamp => (0.5f, 0, 0),
                Heightmap.Biome.Mountain => (1, 1, 1),
                Heightmap.Biome.BlackForest => (0, 0.5f, 0),
                Heightmap.Biome.Plains => (1, 1, 0),
                Heightmap.Biome.AshLands => (1, 0, 0),
                Heightmap.Biome.DeepNorth => (0, 1, 1),
                Heightmap.Biome.Ocean => (0, 0, 1),
                Heightmap.Biome.Mistlands => (0.5f, 0.5f, 0.5f),
                Heightmap.Biome.BiomesMax => (0, 0, 0),
                _ => (0, 0, 0)
            };
        }

        public static (float r, float g, float b) ToBiomeColourGrayscale(int biome)
        {
            return (Heightmap.Biome)biome switch
            {
                Heightmap.Biome.None => (255, 255, 255),
                Heightmap.Biome.Meadows => (120, 120, 120),
                Heightmap.Biome.Swamp => (140, 140, 140),
                Heightmap.Biome.Mountain => (160, 160, 160),
                Heightmap.Biome.BlackForest => (180, 180, 180),
                Heightmap.Biome.Plains => (200, 200, 200),
                Heightmap.Biome.AshLands => (220, 220, 220),
                Heightmap.Biome.DeepNorth => (240, 240, 240),
                Heightmap.Biome.Mistlands => (130, 130, 130),
                Heightmap.Biome.Ocean => (255, 255, 255),
                Heightmap.Biome.BiomesMax => (255, 255, 255),
                _ => (255, 255, 255)
            };
        }
    }
}
