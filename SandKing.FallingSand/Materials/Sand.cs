using SandKing.Physics;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SandKing.FallingSand.Materials
{
    public class Sand : Material
    {
        public Sand(Vector2 position) : base(Color.Yellow, position, 1631f, new CircleCollider(Size))
        {
            Properties = Property.MoveDown |
                         Property.MoveDownLaterally;
        }
    }
}
