using SandKing.Physics;
using SFML.Graphics;
using SFML.System;
using System;
using System.Numerics;
using VectorInt;

namespace SandKing.FallingSand
{
    public abstract class Material : Body, Drawable
    {
        public const uint Size = 5;

        public Material(Color color, Vector2 position, float mass, Collider collider) : base(position, Vector2.Zero, mass, collider)
        {
            Color = color;
        }

        public Property Properties { get; protected set; }
        public Color Color { get; set; }
        public VectorInt2 MaterialPosition => new((int)Position.X, (int)Position.Y);
        public Vector2f DisplayPosition => new(Position.X * Size, Position.Y * Size);

        public bool HasProperty(Property property)
        {
            return (Properties & property) == property;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(GetVertices(), PrimitiveType.Quads, states);
        }

        protected Vertex[] GetVertices()
        {
            var vertices = new Vertex[4];
            vertices[0] = new Vertex(new Vector2f(DisplayPosition.X,        DisplayPosition.Y),        Color);
            vertices[1] = new Vertex(new Vector2f(DisplayPosition.X + Size, DisplayPosition.Y),        Color);
            vertices[2] = new Vertex(new Vector2f(DisplayPosition.X + Size, DisplayPosition.Y + Size), Color);
            vertices[3] = new Vertex(new Vector2f(DisplayPosition.X,        DisplayPosition.Y + Size), Color);
            return vertices;
        }

        protected static Color RandomColorBetween(Color color1, Color color2)
        {
            var random = new Random();
            return new Color(
                (byte)random.Next(Math.Min(color1.R, color2.R), Math.Max(color1.R, color2.R)),
                (byte)random.Next(Math.Min(color1.G, color2.G), Math.Max(color1.G, color2.G)),
                (byte)random.Next(Math.Min(color1.B, color2.B), Math.Max(color1.B, color2.B)),
                (byte)random.Next(Math.Min(color1.A, color2.A), Math.Max(color1.A, color2.A))
            );
        }
    }
}
