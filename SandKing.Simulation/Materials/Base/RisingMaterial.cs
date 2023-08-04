using SandKing.Simulation.Materials.Interfaces;
using VectorInt;

namespace SandKing.Simulation.Materials.Base
{
    public abstract class RisingMaterial : DynamicMaterial
    {
        protected RisingMaterial(ISimulation grid, VectorInt2 position, float density) : base(grid, position, density) { }
    }
}
