using SandKing.Engine.Core;
using SandKing.FallingSand;
using SandKing.FallingSand.Materials;
using SandKing.Physics;
using System;
using System.Numerics;
using System.Threading;

namespace SandKing.Engine
{
    public static class Engine
    {
        public static bool Debug { get; set; }
        public static Input Input { get; set; }
        public static GameTimer GameTimer { get; set; }
        public static Display Display { get; set; }
        public static World World { get; set; }
        public static Simulation Simulation { get; set; }

        public static void Initialize()
        {
            GameTimer = new GameTimer();
            Display = new Display(800, 600, "SandKing");
            Input = new Input(Display);
            World = new World(800 / Material.Size, 600 / Material.Size, new Vector2(0, 9.81f));
            Simulation = new Simulation(800 / Material.Size, 600 / Material.Size, Display);
            _ = new DebugModeScript();
        }

        public static void Run()
        {
            while (Display.IsOpen)
            {
                //World.Step((float)GameTimer.DeltaTime);
                ScriptManager.RunAll();
                Simulation.Simulate();
                Display.Update();
                GameTimer.Update();
                Thread.Sleep(16);
            }
        }

        public static void ShutDown()
        {
            GameTimer = null;
            World = null;
            Simulation = null;
            Display.Dispose();
        }
    }
}
