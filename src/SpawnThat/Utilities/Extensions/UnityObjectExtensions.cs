namespace SpawnThat.Utilities.Extensions;

public static class UnityObjectExtensions
{
    public static bool IsNull(this UnityEngine.Object obj)
    {
        if (obj == null || !obj)
        {
            return true;
        }

        return false;
    }

    public static bool IsNotNull(this UnityEngine.Object obj)
    {
        if (obj != null && obj)
        {
            return true;
        }

        return false;
    }
}
