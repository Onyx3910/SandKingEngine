﻿using SandKing.Simulation.Materials.Base;
using VectorInt;
using Windows.UI;

namespace SandKing.Simulation.Materials
{
    public class Stone : StaticMaterial
    {
        public Stone(ISimulation simulation, VectorInt2 position = default, bool debug = false) : base(simulation, position, 3000f, debug) { }

        //public override Color Color => Colors.Gray;

        public override void TrySetActive()
        {
            return;
        }

        public override void TryDislodge()
        {
            return;
        }
    }
}
