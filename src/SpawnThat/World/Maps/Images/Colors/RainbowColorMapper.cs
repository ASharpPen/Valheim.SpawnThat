using UnityEngine;

namespace SpawnThat.World.Maps.Images.Colors;

internal readonly struct RainbowColorMapper : IColorMapper<int>
{
    public static RainbowColorMapper FromIntMap() => new RainbowColorMapper();

    public Color Map(int cell)
    {
        const double goldenRatioConjugate = 0.618033;
        const float saturationFactor = 0.25f;
        const float brightnessFactor = 0.3f;

        float hue = (float)((cell * goldenRatioConjugate) % 1.0);

        float saturation = 1f - (cell % 3) * saturationFactor;
        float brightness = 1f - ((cell / 3) % 2) * brightnessFactor;

        return Color.HSVToRGB(hue, saturation, brightness);
    }
}
