using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UltimateQuadTree;

namespace SandKing.Physics
{
    public abstract class Body
    {
        public Body(Vector2 position, Vector2 velocity, float mass, Collider collider)
        {
            IsActive = true;
            Position = position;
            Velocity = velocity;
            Mass = mass;
            Collider = collider;
            Collider.Body = this;
            Forces = new List<Vector2>();
        }

        public bool IsActive { get; set; }
        public Collider Collider { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public float Mass { get; set; }
        public static IQuadTreeObjectBounds<Body> Bounds { get; set; }
        private List<Vector2> Forces { get; }

        public void ApplyForce(Vector2 force, bool impulse = false)
        {
            if(impulse) force /= Mass;
            Forces.Add(force);
        }

        public void Verlet(float deltaTime)
        {
            var newPosition = Position + Velocity * deltaTime + Acceleration * deltaTime * deltaTime * 0.5f;
            var newAcceleration = Forces.Aggregate(Vector2.Zero, (current, force) => current + force);
            var newVelocity = Velocity + (Acceleration + newAcceleration) * deltaTime * 0.5f;
            Position = newPosition;
            Velocity = newVelocity;
            Acceleration = newAcceleration;
            Forces.Clear();
        }

        public bool IsColliding(Body other)
        {
            return Collider.IsColliding(other.Collider);
        }
    }
}
