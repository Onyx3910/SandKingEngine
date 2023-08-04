using Microsoft.Graphics.Canvas;
using SandKing.Physics;
using SandKing.Simulation.Materials.Interfaces;
//using tainicom.Aether.Physics2D.Dynamics;

namespace SandKing.Simulation
{
    public interface ISimulation
    {
        World World { get; }
        IMaterial[,] Grid { get; }
        void Simulate(CanvasDrawingSession session);
    }
}