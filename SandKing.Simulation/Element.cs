using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandKing.Simulation
{
    public abstract class Element : IElement
    {
        protected Element(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public virtual void Draw(CanvasDrawingSession session)
        {
            //var regionColor = Windows.UI.Colors.LightYellow;
            //session.FillRectangle(X * FallingSand.Instance.CellWidth, Y * FallingSand.Instance.CellHeight, FallingSand.Instance.CellWidth, FallingSand.Instance.CellHeight, regionColor);
        }

        public abstract void Update(int x, int y);
    }
}
