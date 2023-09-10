using SandKing.Physics;
using SFML.Graphics;
using System.Numerics;

namespace SandKing.FallingSand.Materials
{
    public class OutOfBounds : Material
    {
        public static readonly OutOfBounds Instance = new();

        private OutOfBounds() : base(Color.Transparent, new Vector2(-1), float.PositiveInfinity, new CircleCollider(1f))
        {
        }
    }
}
