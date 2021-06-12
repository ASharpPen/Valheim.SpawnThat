namespace Valheim.SpawnThat.WorldMap
{
    public interface IImagePrinter
    {
        void PrintSquareMap(int[][] map, string imageFileName, bool rainbowOn, int valueRangeMin = 0, int valueRangeMax = 1);

        void PrintBiomes(int[][] map, string imageFileName);
    }
}
