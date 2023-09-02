using SandKing.Simulation.Materials.Base;
using VectorInt;
using Windows.UI;

namespace SandKing.Simulation.Materials
{
    public class Sand : Solid
    {
        public Sand(ISimulation simulation, VectorInt2 position = default, bool debug = false) : base(simulation, position, 1600f, debug) { }

        public override double InertialResistance => 0.1D;

        public override Color Color => Debug ? base.Color : Color.FromArgb(255, 235, 200, 175);
    }
}
