using SandKing.Physics;
using SFML.Graphics;
using System.Numerics;

namespace SandKing.FallingSand.Materials
{
    public class Sand : Material
    {
        public Sand(Vector2 position) : base(RandomColorBetween(new Color(235, 200, 175), Color.Yellow), position, 1631f, new CircleCollider(Size))
        {
            Properties = Property.MoveDown |
                         Property.MoveDownLaterally;
        }
    }
}
