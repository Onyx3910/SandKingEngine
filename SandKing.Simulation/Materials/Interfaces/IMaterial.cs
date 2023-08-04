using Microsoft.Graphics.Canvas;
using VectorInt;
using Windows.UI;
using System.Numerics;

namespace SandKing.Simulation.Materials.Interfaces
{
    public interface IMaterial
    {
        Vector2 Position { get; set; }
        VectorInt2 LastGridPosition { get; }
        //bool IsActive { get; set; }
        bool IsParticle { get; set; }
        float Density { get; }
        Color Color { get; }
        ISimulation Simulation { get; }
        void Update(CanvasDrawingSession session);
    }
}
