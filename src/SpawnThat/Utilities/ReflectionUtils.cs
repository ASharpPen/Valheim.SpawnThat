using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SpawnThat.Utilities;

internal static class ReflectionUtils
{
    private static MethodInfo InstantiateGameObject = null;

    public static MethodInfo InstantiateGameObjectMethod
    {
        get
        {
            return InstantiateGameObject ??= typeof(UnityEngine.Object)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(x => x.Name.Equals(nameof(UnityEngine.Object.Instantiate)))
                .Where(m => m.IsGenericMethod)
                .First(m =>
                    m.ContainsGenericParameters &&
                    m.GetParameters().Length == 3 &&
                    m.GetParameters()[2].ParameterType == typeof(Quaternion))
                .GetGenericMethodDefinition()
                .MakeGenericMethod(typeof(GameObject));
        }
    }
}
