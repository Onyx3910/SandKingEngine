using SandKing.Simulation.Materials.Interfaces;
using System;
using VectorInt;

namespace SandKing.Simulation.Materials.Base
{
    public abstract class FallingMaterial : DynamicMaterial
    {
        public static readonly VectorInt2 Down = new VectorInt2(0, 1);

        protected FallingMaterial(ISimulation simulation, VectorInt2 position, float density) : base(simulation, position, density) { }

        protected virtual bool TryMoveDown(out VectorInt2 outLocation)
        {
            var down = IsEmpty(GridPosition + Down);
            if (down) outLocation = MoveTo(GridPosition + Down);
            else outLocation = GridPosition;
            return down;
        }

        protected virtual bool TryMoveDownSide(VectorInt2 position, out VectorInt2 outLocation)
        {
            var downLeftPos = position + Down + VectorInt2.Left;
            var downRightPos = position + Down + VectorInt2.Right;

            var downLeft = IsEmpty(downLeftPos);
            var downRight = IsEmpty(downRightPos);

            if (downLeft && downRight)
            {
                downLeft = new Random().Next() >= int.MaxValue / 2;
                downRight = !downLeft;
            }

            if (downLeft) outLocation = MoveTo(downLeftPos);
            else if (downRight) outLocation = MoveTo(downRightPos);
            else outLocation = position;

            return downLeft || downRight;
        }
    }
}
