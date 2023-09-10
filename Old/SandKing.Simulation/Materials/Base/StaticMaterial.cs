using Microsoft.Graphics.Canvas;
using SandKing.Simulation.Materials.Interfaces;
using System.Diagnostics;
using VectorInt;

namespace SandKing.Simulation.Materials.Base
{
    public abstract class StaticMaterial : Material, ISolid
    {
        public StaticMaterial(ISimulation simulation, VectorInt2 position, float density, bool debug = false) : base(simulation, position, density, debug) 
        {
            IsParticle = false;
        }

        protected override void SimulateFallingSand() { }

        public override void Update()
        {
            return;
        }
    }
}
