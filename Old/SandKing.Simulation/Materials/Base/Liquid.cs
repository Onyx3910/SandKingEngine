using Microsoft.Graphics.Canvas;
using SandKing.Simulation.Materials.Interfaces;
using System;
using System.Diagnostics;
using System.Numerics;
using VectorInt;

namespace SandKing.Simulation.Materials.Base
{
    public abstract class Liquid : FallingMaterial, ILiquid
    {
        public Liquid(ISimulation simulation, VectorInt2 position, float density, int dispersionRate, bool debug = false) : base(simulation, position, density, debug) 
        {
            DispersionRate = dispersionRate;
        }

        public int DispersionRate { get; }

        public override void TrySetActive()
        {
            base.TrySetActive();
            if (IsEmpty(GridPosition + Down) || 
                IsEmpty(GridPosition + VectorInt2.Left) || 
                IsEmpty(GridPosition + VectorInt2.Right) || 
                IsEmpty(GridPosition + Down + VectorInt2.Left) || 
                IsEmpty(GridPosition + Down + VectorInt2.Right)) IsActive = true;
        }

        public override void Update()
        {
            base.Update();
        }

        protected override void SimulateFallingSand()
        {
            if (TryMoveDown(out var location)) { IsParticle = true; }
            else if (TryMoveDownSide(GridPosition, out location)) { }
            else if (TryMoveSide(GridPosition, out location)) { }
            else { location = GridPosition; }
            Position = location;
        }

        protected override bool TryMoveDownSide(VectorInt2 position, out VectorInt2 outLocation)
        {
            var nextDownLeft = position + Down + VectorInt2.Left;
            var targetDownLeft = position + new VectorInt2(-DispersionRate, DispersionRate);
            var nextDownRight = position + Down + VectorInt2.Right;
            var targetDownRight = position + new VectorInt2(DispersionRate, DispersionRate);

            var downLeft = IsEmpty(nextDownLeft);
            var downRight = IsEmpty(nextDownRight);

            if (downLeft && downRight)
            {
                downLeft = new Random().Next() >= int.MaxValue / 2;
                downRight = !downLeft;
            }

            var actualDownLeft = BresenhamTraversal(nextDownLeft, targetDownLeft, out var _);
            var actualDownRight = BresenhamTraversal(nextDownRight, targetDownRight, out var _);

            if (downLeft)
            {
                outLocation = MoveTo(actualDownLeft);
                var vel = actualDownLeft - position;
                Velocity = new Vector2(vel.X, vel.Y);
            }
            else if (downRight)
            {
                outLocation = MoveTo(actualDownRight);
                var vel = actualDownRight - position;
                Velocity = new Vector2(vel.X, vel.Y);
            }
            else outLocation = position;

            return downLeft || downRight;
        }

        protected virtual bool TryMoveSide(VectorInt2 position, out VectorInt2 outLocation)
        {
            var nextLeft = position + VectorInt2.Left;
            var targetLeft = position + new VectorInt2(-DispersionRate, 0);
            var nextRight = position + VectorInt2.Right;
            var targetRight = position + new VectorInt2(DispersionRate, 0);

            var left = IsEmpty(nextLeft);
            var right = IsEmpty(nextRight);

            if (left && right)
            {
                left = new Random().Next() >= int.MaxValue / 2;
                right = !left;
            }

            var actualLeft = BresenhamTraversal(nextLeft, targetLeft, out var _);
            var actualRight = BresenhamTraversal(nextRight, targetRight, out var _);

            if (left)
            {
                outLocation = MoveTo(actualLeft);
                var vel = actualLeft - position;
                Velocity = new Vector2(vel.X, vel.Y);
            }
            else if (right)
            {
                outLocation = MoveTo(actualRight);
                var vel = actualRight - position;
                Velocity = new Vector2(vel.X, vel.Y);
            }
            else outLocation = position;

            return left || right;
        }
    }
}
