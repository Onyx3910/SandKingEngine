using System;
using VectorInt;

namespace SandKing.Simulation.Materials.Base
{
    public abstract class DynamicMaterial : Material
    {
        protected DynamicMaterial(ISimulation simulation, VectorInt2 position, float density, bool debug = false) : base(simulation, position, density, debug) 
        {
            IsParticle = true;
        }

        public abstract double InertialResistance { get; }

        public override void TryDislodge()
        {
            if (new Random().NextDouble() >= InertialResistance) IsActive = true;
        }
    }
}
