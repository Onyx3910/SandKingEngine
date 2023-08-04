using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using UltimateQuadTree;

namespace SandKing.Physics
{
    public class Particle
    {
        public Particle(Vector2 position, float radius)
        {
            IsParticle = true;
            Position = position;
            Radius = radius;
        }

        public bool IsParticle { get; set; }
        public float Radius { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public void Move(float deltaTime, Vector2 gravity = default)
        {
            var velocityLengthSquare = Velocity.LengthSquared();
            var oneOverDt = 1f / deltaTime;
            if (velocityLengthSquare > 0 && velocityLengthSquare < oneOverDt * oneOverDt) Velocity = Vector2.Normalize(Velocity) * oneOverDt;
            Position += Velocity * deltaTime;
            Velocity += gravity;
        }

        public bool IsColliding(Particle other)
        {
            if (other == null) return false;
            var squaredDistance = (Position - other.Position).LengthSquared();
            var squaredRadiusSum = Math.Pow(Radius + other.Radius, 2);
            return squaredDistance < squaredRadiusSum;
        }
    }

    public class ParticleObjectBounds : IQuadTreeObjectBounds<Particle>
    {
        public double GetBottom(Particle obj)
        {
            return obj.Position.Y + obj.Radius;
        }

        public double GetLeft(Particle obj)
        {
            return obj.Position.X - obj.Radius;
        }

        public double GetRight(Particle obj)
        {
            return obj.Position.X + obj.Radius;
        }

        public double GetTop(Particle obj)
        {
            return obj.Position.Y - obj.Radius;
        }
    }
}
