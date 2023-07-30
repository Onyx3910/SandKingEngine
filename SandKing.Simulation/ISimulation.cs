using Microsoft.Graphics.Canvas;

namespace SandKing.Simulation
{
    public interface ISimulation
    {
        void Simulate(CanvasDrawingSession session);
        int CellWidth { get; }
        int CellHeight { get; }
    }
}