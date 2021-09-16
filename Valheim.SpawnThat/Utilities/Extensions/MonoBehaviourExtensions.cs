using System;
using System.Collections;
using UnityEngine;

namespace Valheim.SpawnThat.Utilities
{
    public static class MonoBehaviourExtensions
    {
        public static void StartCoroutine(this MonoBehaviour obj, Action task, TimeSpan? delay = null)
        {
            obj.StartCoroutine(DelayedCoroutine(task, delay));
        }

        private static IEnumerator DelayedCoroutine(Action task, TimeSpan? delay = null)
        {
            if (delay is not null)
            {
                yield return new WaitForSeconds((float)delay.Value.TotalSeconds);
            }

            task();
        }
    }
}
