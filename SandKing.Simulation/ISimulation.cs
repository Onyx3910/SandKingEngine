using SandKing.Physics;
using SandKing.Simulation.Materials.Interfaces;

namespace SandKing.Simulation
{
    public interface ISimulation
    {
        bool Debug { get; set; }
        Camera Camera { get; }
        World World { get; }
        Chunk[,] Chunks { get; }
        IMaterial[,] Grid { get; }
        void Simulate();
        void ToggleDebug();
    }
}