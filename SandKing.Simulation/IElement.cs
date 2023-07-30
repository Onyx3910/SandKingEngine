using Microsoft.Graphics.Canvas;

namespace SandKing.Simulation
{
    public interface IElement
    {
        void Update(int x, int y);
        void Draw(CanvasDrawingSession session);
    }
}