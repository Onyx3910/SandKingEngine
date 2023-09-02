using Microsoft.Graphics.Canvas;
using VectorInt;
using Windows.UI;
using System.Numerics;

namespace SandKing.Simulation.Materials.Interfaces
{
    public interface IMaterial
    {
        bool Debug { get; set; }
        Chunk Chunk { get; }
        Vector2 Position { get; set; }
        VectorInt2 LastGridPosition { get; }
        bool IsParticle { get; set; }
        bool IsActive { get; set; }
        float Density { get; }
        Color Color { get; }
        ISimulation Simulation { get; }
        void Update();
        void Draw(CanvasDrawingSession session);
        void AssignToChunk();
    }
}
