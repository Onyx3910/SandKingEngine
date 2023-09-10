using System;
using System.Numerics;

namespace SandKing.Physics
{
    public class CircleCollider : Collider
    {
        public CircleCollider(float radius) : base()
        {
            Radius = radius;
        }

        public float Radius { get; set; }

        public override bool IsColliding(Collider other)
        {
            if (other is null || other is not CircleCollider circleCollider) return false;
            var squaredDistance = (Body.Position - circleCollider.Body.Position).LengthSquared();
            var squaredRadiusSum = Math.Pow(Radius + circleCollider.Radius, 2);
            return squaredDistance < squaredRadiusSum;
        }

        public override void ResolveCollision(Collider other)
        {
            if (other is null || other is not CircleCollider circleCollider) return;

            // Calculate the direction vector from p2 to p1
            var normal = Vector2.Normalize(Body.Position - circleCollider.Body.Position);

            // Calculate the relative velocity of p1 to p2
            Vector2 relativeVelocity = Body.Velocity - circleCollider.Body.Velocity;

            // Calculate the relative velocity along the normal vector
            float velAlongNormal = Vector2.Dot(relativeVelocity, normal);

            // If the particles are moving away from each other, do not resolve collision
            if (velAlongNormal > 0)
            {
                return;
            }

            // Calculate the impulse scalar for the collision resolution
            float e = 1.0f; // Coefficient of restitution, set to 1 for elastic collision
            float j = -(1 + e) * velAlongNormal;
            j /= (1 / Radius) + (1 / circleCollider.Radius);

            // Apply the impulse to adjust the velocities
            Vector2 impulse = j * normal;
            Body.Velocity += (1 / Radius) * impulse;
            circleCollider.Body.Velocity -= (1 / circleCollider.Radius) * impulse;

            // Move particles apart to avoid sticking together
            float overlap = (Radius + circleCollider.Radius) - (Body.Position - circleCollider.Body.Position).Length();
            Vector2 correction = overlap * 0.5f * normal;
            Body.Position += correction;
            circleCollider.Body.Position -= correction;
        }
    }
}
