using Microsoft.Graphics.Canvas;
using SandKing.Simulation.Materials.Interfaces;
using VectorInt;

namespace SandKing.Simulation.Materials.Base
{
    public abstract class Solid : FallingMaterial, ISolid
    {
        public Solid(ISimulation simulation, VectorInt2 position, float density, bool debug = false) : base(simulation, position, density, debug) { }

        public override void TrySetActive()
        {
            base.TrySetActive();
            if (IsEmpty(GridPosition + Down)) IsActive = true;
        }

        protected virtual void TestAdjacentSolids()
        {
            var left = new VectorInt2(GridPosition.X - 1, GridPosition.Y);
            if (InBounds(left) && Simulation.Grid[left.X, left.Y] is Solid leftFallingMaterial)
            {
                leftFallingMaterial.TryDislodge();
            }

            var right = new VectorInt2(GridPosition.X + 1, GridPosition.Y);
            if (InBounds(right) && Simulation.Grid[right.X, right.Y] is Solid rightFallingMaterial)
            {
                rightFallingMaterial.TryDislodge();
            }
        }

        protected override void EarlyUpdate()
        {
            base.EarlyUpdate();
            if (IsParticle) TestAdjacentSolids();
        }

        public override void Update()
        {
            base.Update();
        }

        protected override void SimulateFallingSand()
        {
            if (TryMoveDown(out var location)) 
            {
                TestAdjacentSolids();
                IsParticle = true; 
            }
            else if (TryMoveDownSide(GridPosition, out location)) 
            {
                TestAdjacentSolids();
            }
            else 
            { 
                location = GridPosition; 
            }

            Position = location;
        }
    }
}
