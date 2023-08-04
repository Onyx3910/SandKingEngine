using SandKing.Simulation.Materials.Base;
using VectorInt;
using Windows.UI;

namespace SandKing.Simulation.Materials
{
    public class Sand : Solid
    {
        public Sand(ISimulation simulation, VectorInt2 position = default) : base(simulation, position, 1600f) { }

        //public override Color Color => Color.FromArgb(255, 235, 200, 175);
    }
}
