using System.Collections.Generic;

namespace SandKing.Engine.Core
{
    public static class ScriptManager
    {
        static ScriptManager()
        {
            Scripts = new List<Script>();
        }

        private static List<Script> Scripts { get; }

        public static void Add(Script script)
        {
            Scripts.Add(script);
        }

        public static void Remove(Script script)
        {
            Scripts.Remove(script);
        }

        public static void RunAll()
        {
            foreach (var script in Scripts)
            {
                if (script.Enabled) script.Run();
            }
        }
    }
}
