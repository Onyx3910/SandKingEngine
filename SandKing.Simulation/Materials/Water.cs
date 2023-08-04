using Microsoft.Graphics.Canvas;
using SandKing.Simulation.Materials.Base;
using SandKing.Simulation.Materials.Interfaces;
using VectorInt;
using Windows.UI;

namespace SandKing.Simulation.Materials
{
    public class Water : Liquid
    {
        public Water(ISimulation simulation, VectorInt2 position = default) : base(simulation, position, 1000f, dispersionRate: 5) { }

        public override Color Color => Colors.Blue;
    }
}
