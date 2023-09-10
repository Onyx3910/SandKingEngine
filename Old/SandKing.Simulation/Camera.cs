using Microsoft.Graphics.Canvas;
using SandKing.Simulation.Materials.Base;
using SandKing.Simulation.Materials.Interfaces;
using System;
using System.Collections.Generic;
using VectorInt;

namespace SandKing.Simulation
{
    public class Camera
    {
        public Camera(VectorInt2 position, float zoom)
        {
            Position = position;
            Zoom = zoom;
        }

        public Camera(int width, int height, VectorInt2 position, float zoom) : this(position, zoom) 
        { 
            Width = width;
            Height = height;
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public VectorInt2 Position { get; set; }
        public float Zoom { get; set; }

        public void Render(CanvasDrawingSession session)
        {
            foreach (var chunk in FallingSand.Instance.Chunks)
            {
                if (IsInView(chunk)) chunk.Draw(session);
            }
        }

        public void Move(VectorInt2 direction)
        {
            Position += direction;
        }

        public bool IsInView(IMaterial material)
        {
            var materialPosition = material.Position;
            var materialSize = Material.Size;
            var materialLeft = materialPosition.X - materialSize / 2;
            var materialTop = materialPosition.Y - materialSize / 2;
            var materialRight = materialLeft + materialSize;
            var materialBottom = materialTop + materialSize;

            var cameraLeft = Position.X;
            var cameraTop = Position.Y;
            var cameraRight = cameraLeft + Width;
            var cameraBottom = cameraTop + Height;

            return materialRight >= cameraLeft && materialLeft <= cameraRight && materialBottom >= cameraTop && materialTop <= cameraBottom;
        }

        // overload IsInView for chunk
        public bool IsInView(Chunk chunk)
        {
            var chunkPosition = chunk.Position;
            var chunkSizeWidth = Material.Size * Chunk.Width;
            var chunkSizeHeight = Material.Size * Chunk.Height;
            var chunkLeft = chunkPosition.X;
            var chunkTop = chunkPosition.Y;
            var chunkRight = chunkLeft + chunkSizeWidth;
            var chunkBottom = chunkTop + chunkSizeHeight;

            var cameraLeft = Position.X;
            var cameraTop = Position.Y;
            var cameraRight = cameraLeft + Width;
            var cameraBottom = cameraTop + Height;

            return chunkRight >= cameraLeft || chunkLeft <= cameraRight || chunkBottom >= cameraTop || chunkTop <= cameraBottom;
        }
    }
}
