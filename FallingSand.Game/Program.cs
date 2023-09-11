using SandKing.FallingSand.Materials;
using System.Numerics;

namespace SandKing.Engine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Engine.Initialize();
            Engine.Run();
            Engine.ShutDown();
        }
    }
}