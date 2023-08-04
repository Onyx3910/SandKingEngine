using VectorInt;

namespace SandKing.Simulation.Materials.Base
{
    public abstract class DynamicMaterial : Material
    {
        protected DynamicMaterial(ISimulation simulation, VectorInt2 position, float density) : base(simulation, position, density) 
        {
            IsParticle = true;
        }
    }
}
