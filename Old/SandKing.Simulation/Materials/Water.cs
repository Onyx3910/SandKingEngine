using Microsoft.Graphics.Canvas;
using SandKing.Simulation.Materials.Base;
using SandKing.Simulation.Materials.Interfaces;
using VectorInt;
using Windows.UI;

namespace SandKing.Simulation.Materials
{
    public class Water : Liquid
    {
        public Water(ISimulation simulation, VectorInt2 position = default, bool debug = false) : base(simulation, position, 1000f, dispersionRate: 5, debug) { }

        public override double InertialResistance => 0.0D;

        //public override Color Color => Debug ? base.Color : Colors.Blue;
    }
}
