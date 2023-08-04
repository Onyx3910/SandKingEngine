using SandKing.Simulation.Materials.Base;
using VectorInt;
using Windows.UI;

namespace SandKing.Simulation.Materials
{
    public class Stone : StaticMaterial
    {
        public Stone(ISimulation simulation, VectorInt2 position = default) : base(simulation, position, 3000f) { }

        public override Color Color => Colors.Gray;
    }
}
