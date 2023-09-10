using Microsoft.Graphics.Canvas;
using SandKing.Physics;
using SandKing.Simulation.Materials.Interfaces;
using System;
using System.Numerics;
using VectorInt;
using Windows.UI;

namespace SandKing.Simulation.Materials.Base
{
    public abstract class Material : Particle, IMaterial
    {
        public const int Size = 5;

        public Material(ISimulation simulation, Vector2 position, float density, bool debug = false) : base(position, 0.5f)
        {
            LastGridPosition = position;
            Density = density;
            Debug = debug;

            simulation.World.Insert(this);
            Simulation = simulation;
            Simulation.Grid[(int)position.X, (int)position.Y] = this;

            AssignToChunk();
        }

        public bool Debug { get; set; }
        public Chunk Chunk { get; private set; }
        public bool IsActive { get; set; } = true;
        public VectorInt2 GridPosition => new VectorInt2(Position);
        public VectorInt2 LastGridPosition { get; private set; }
        public float Density { get; }
        public virtual Color Color { get; protected set; }
        public ISimulation Simulation { get; }

        public virtual void Update()
        {
            EarlyUpdate();
            if (Debug) Color = Colors.Red;
            if (!IsParticle)
            {
                TrySetActive();
                if (IsActive) SimulateFallingSand();
                if (Debug) Color = Colors.Green;
                if (LastGridPosition == GridPosition)
                {
                    IsActive = false;
                    if (Debug) Color = Colors.DarkGray;
                }
            }

            LateUpdate();
        }

        public abstract void TrySetActive();

        public abstract void TryDislodge();

        public virtual void AssignToChunk()
        {
            var x = GridPosition.X / Chunk.Width;
            var y = GridPosition.Y / Chunk.Height;
            Chunk = Simulation.Chunks[x, y];
            Chunk.Materials.Add(this);
            Chunk.IsDirty = IsParticle || IsActive;
        }

        protected virtual void EarlyUpdate()
        {
            Simulation.Grid[LastGridPosition.X, LastGridPosition.Y] = null;

            if (IsParticle)
            {
                var locationBeforeCollision = BresenhamTraversal(LastGridPosition, GridPosition, out var collision);
                if (collision != new VectorInt2(-1, -1) && (!InBounds(collision) || !Simulation.Grid[collision.X, collision.Y].IsParticle))
                {
                    IsParticle = false;
                    Velocity = Vector2.Zero;
                }

                Position = new Vector2(locationBeforeCollision.X, locationBeforeCollision.Y);
            }
        }

        public virtual void Draw(CanvasDrawingSession session)
        {
            if (Simulation.Camera.IsInView(this)) session.FillRectangle(Position.X * Size, Position.Y * Size, Size, Size, Color);
        }

        protected virtual void LateUpdate()
        {
            Simulation.Grid[GridPosition.X, GridPosition.Y] = this;
            LastGridPosition = GridPosition;
            AssignToChunk();
        }

        protected abstract void SimulateFallingSand();

        protected VectorInt2 MoveTo(VectorInt2 to)
        {
            Simulation.Grid[GridPosition.X, GridPosition.Y] = null;
            Simulation.Grid[to.X, to.Y] = this;
            return (to.X, to.Y);
        }

        protected virtual void SwapWith(int fromX, int fromY, int toX, int toY)
        {
            var material = Simulation.Grid[toX, toY];
            Simulation.Grid[toX, toY] = this;
            Simulation.Grid[fromX, fromY] = material;
        }

        protected bool InBounds(VectorInt2 pos) => pos.X >= 0 && pos.Y >= 0 && pos.X < Simulation.Grid.GetLength(0) && pos.Y < Simulation.Grid.GetLength(1);
        protected bool IsEmpty(VectorInt2 pos) => InBounds(pos) && (Simulation.Grid[pos.X, pos.Y] == null);

        protected VectorInt2 BresenhamTraversal(VectorInt2 start, VectorInt2 end, out VectorInt2 pointOfCollision, Func<VectorInt2, bool> breakEarlyFunc = null)
        {
            // See: https://stackoverflow.com/questions/11678693/all-cases-covered-bresenhams-line-algorithm
            pointOfCollision = Vector2.One * -1;

            var width = end.X - start.X;
            var height = end.Y - start.Y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (width < 0) dx1 = -1; else if (width > 0) dx1 = 1;
            if (height < 0) dy1 = -1; else if (height > 0) dy1 = 1;
            if (width < 0) dx2 = -1; else if (width > 0) dx2 = 1;
            int longest = Math.Abs(width);
            int shortest = Math.Abs(height);
            if (longest <= shortest)
            {
                longest = Math.Abs(height);
                shortest = Math.Abs(width);
                if (height < 0) dy2 = -1; else if (height > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                if (breakEarlyFunc != null && breakEarlyFunc.Invoke(start)) break;
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    start.X += dx1;
                    start.Y += dy1;

                    if (!IsEmpty(start))
                    {
                        pointOfCollision = start;
                        start.X -= dx1;
                        start.Y -= dy1;
                        break;
                    }
                }
                else
                {
                    start.X += dx2;
                    start.Y += dy2;

                    if (!IsEmpty(start))
                    {
                        pointOfCollision = start;
                        start.X -= dx2;
                        start.Y -= dy2;
                        break;
                    }
                }
            }

            return start;
        }
    }
}
