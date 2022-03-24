using System.Linq;
using UnityEditor;

namespace FrostweepGames.Plugins
{
    [InitializeOnLoad]
    internal class DefineProcessing : Editor
    {
        private static readonly string[] _Defines = new string[] 
        {
            "FG_GCTTS"
        };

        static DefineProcessing()
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var defines = definesString.Split(';').ToList();
            defines.AddRange(_Defines.Except(defines));
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", defines.ToArray()));
        }
    }
}