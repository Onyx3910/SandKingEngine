using Microsoft.Graphics.Canvas;
using SandKing.Simulation.Materials.Interfaces;
using VectorInt;

namespace SandKing.Simulation.Materials.Base
{
    public abstract class Solid : FallingMaterial, ISolid
    {
        public Solid(ISimulation simulation, VectorInt2 position, float density) : base(simulation, position, density) { }

        protected virtual void TestAdjacentSolids()
        {
            var left = new VectorInt2(GridPosition.X - 1, GridPosition.Y);
            if (!IsEmpty(left) && Simulation.Grid[left.X, left.Y] is Solid leftFallingMaterial)
            {
                //leftFallingMaterial.IsActive = true;
            }

            var right = new VectorInt2(GridPosition.X + 1, GridPosition.Y);
            if (!IsEmpty(right) && Simulation.Grid[right.X, right.Y] is Solid rightFallingMaterial)
            {
                //rightFallingMaterial.IsActive = true;
            }
        }

        public override void Update(CanvasDrawingSession session)
        {
            base.Update(session);
        }

        protected override void SimulateFallingSand()
        {
            if (TryMoveDown(out var location)) { IsParticle = true; }
            else if (TryMoveDownSide(GridPosition, out location)) { }
            else { location = GridPosition; }
            Position = location;
        }
    }
}
