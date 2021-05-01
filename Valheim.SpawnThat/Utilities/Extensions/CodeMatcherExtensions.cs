using HarmonyLib;
using System.Reflection.Emit;

namespace Valheim.SpawnThat.Utilities.Extensions
{
    public static class CodeMatcherExtensions
    {
        public static CodeMatcher GetPosition(this CodeMatcher codeMatcher, out int position)
        {
            position = codeMatcher.Pos;
            return codeMatcher;
        }

        public static CodeMatcher AddLabel(this CodeMatcher codeMatcher, out Label label)
        {
            label = new Label();
            codeMatcher.AddLabels(new[] { label });
            return codeMatcher;
        }
    }
}
