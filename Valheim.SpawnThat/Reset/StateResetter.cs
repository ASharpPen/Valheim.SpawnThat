using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Valheim.SpawnThat.ConfigurationCore;

namespace Valheim.SpawnThat.Reset
{
    public static class StateResetter
    {
        private static HashSet<IReset> Resetables = new HashSet<IReset>();
        private static HashSet<Action> OnResetActions = new HashSet<Action>();

        public static void Subscribe(IReset reset)
        {
            Resetables.Add(reset);
        }

        public static void Subscribe(Action onReset)
        {
            OnResetActions.Add(onReset);
        }

        public static void Unsubscribe(IReset reset)
        {
            Resetables.Remove(reset);
        }

        public static void Unsubscribe(Action onReset)
        {
            OnResetActions.Remove(onReset);
        }

        public static void Reset()
        {
            Log.LogDebug("Resetting mod state.");

            foreach(var resetable in Resetables)
            {
                resetable.Reset();
            }

            foreach(var onReset in OnResetActions)
            {
                onReset.Invoke();
            }
        }
    }
}
