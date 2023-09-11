using SandKing.FallingSand.Materials;
using System.Numerics;

namespace SandKing.Engine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Restructure projects be centered around an EngineCore project and a Game project

            Engine.Initialize();
            Engine.Simulation.Add(new Sand(new Vector2(0, 0)));
            Engine.Simulation.Add(new Sand(new Vector2(0, 1)));
            Engine.Run();
            Engine.ShutDown();
        }
    }
}