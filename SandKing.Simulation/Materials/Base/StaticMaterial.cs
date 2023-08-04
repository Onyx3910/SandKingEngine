using Microsoft.Graphics.Canvas;
using SandKing.Simulation.Materials.Interfaces;
using VectorInt;

namespace SandKing.Simulation.Materials.Base
{
    public abstract class StaticMaterial : Material, ISolid
    {
        public StaticMaterial(ISimulation simulation, VectorInt2 position, float density) : base(simulation, position, density) 
        {
            IsParticle = false;
        }

        protected override void SimulateFallingSand() { }

        public override void Update(CanvasDrawingSession session)
        {
            session.FillRectangle(Position.X * Size, Position.Y * Size, Size, Size, Color);
        }
    }
}
