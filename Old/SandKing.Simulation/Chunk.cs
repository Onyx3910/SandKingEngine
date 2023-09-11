using Microsoft.Graphics.Canvas;
using SandKing.Simulation.Materials.Base;
using SandKing.Simulation.Materials.Interfaces;
using System.Collections.Generic;
using VectorInt;

namespace SandKing.Simulation
{
    public class Chunk
    {
        public static int Width;
        public static int Height;

        public Chunk(FallingSand simulation, VectorInt2 position, bool debug = false)
        {
            Simulation = simulation;
            Position = position;
            Materials = new List<IMaterial>(Width * Height);
        }

        public bool Debug { get; set; }
        public FallingSand Simulation { get; set; }
        public VectorInt2 Position { get; set; }
        public bool IsDirty { get; set; }
        public List<IMaterial> Materials { get; set; }

        public void Draw(CanvasDrawingSession session)
        {
            //if(Debug)
            //{
            //    var borderColor = IsDirty ? Windows.UI.Colors.Purple : Windows.UI.Colors.White;
            //    session.DrawRectangle(Position.X * Material.Size, Position.Y * Material.Size, Width * Material.Size, Height * Material.Size, borderColor);
            //}

            for(var index = 0; index < Materials.Count; index++)
            {
                var material = Materials[index];
                material.Draw(session);
            }

            Materials.Clear();
        }
    }
}
