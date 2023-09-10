using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UltimateQuadTree;

namespace SandKing.Physics
{
    public class World
    {
        public World(float width, float height, Vector2 gravity) : this(width, height, gravity, 10, 5) { }

        public World(float width, float height, Vector2 gravity, int particlesPerQuad, int quadTreeMaxDepth)
        {
            Width = width;
            Height = height;
            Gravity = gravity;
            Particles = new List<Particle>();
            QuadTree = new QuadTree<Particle>(width, height, new ParticleObjectBounds(), particlesPerQuad, quadTreeMaxDepth);
        }

        public float Width { get; }
        public float Height { get; }
        public Vector2 Gravity { get; set; }
        public List<Particle> Particles { get; }
        protected QuadTree<Particle> QuadTree { get; set; }

        public void Step(float deltaTime)
        {
            MoveParticles(deltaTime);
            PreHandleCollisions();
            HandleCollisions();
        }

        public void Insert(params Particle[] p)
        {
            Particles.AddRange(p.ToList());
        }

        protected void MoveParticles(float deltaTime)
        {
            foreach(var p in Particles.Where(p => p.IsParticle))
            {
                p.Move(deltaTime, Gravity);
            }
        }

        protected void PreHandleCollisions()
        {
            QuadTree.Clear();
            QuadTree.InsertRange(Particles.Where(p => p.IsParticle));
        }

        protected void HandleCollisions()
        {
            var quadTreeParticles = QuadTree.GetObjects().ToList();
            for(var index = 0; index < quadTreeParticles.Count; index++) 
            { 
                var particle = quadTreeParticles[index];
                var otherParticles = QuadTree.GetNearestObjects(particle).ToList();
                foreach(var otherParticle in otherParticles)
                {
                    if (particle.Position == otherParticle.Position) continue;
                    if (particle.IsColliding(otherParticle))
                    {
                        ResolveCollision(particle, otherParticle);
                    }
                }
            }
        }

        private static void ResolveCollision(Particle p1, Particle p2)
        {
            // Calculate the direction vector from p2 to p1
            var normal = Vector2.Normalize(p1.Position - p2.Position);

            // Calculate the relative velocity of p1 to p2
            Vector2 relativeVelocity = p1.Velocity - p2.Velocity;

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
            j /= (1 / p1.Radius) + (1 / p2.Radius);

            // Apply the impulse to adjust the velocities
            Vector2 impulse = j * normal;
            p1.Velocity += (1 / p1.Radius) * impulse;
            p2.Velocity -= (1 / p2.Radius) * impulse;

            // Move particles apart to avoid sticking together
            float overlap = (p1.Radius + p2.Radius) - (p1.Position - p2.Position).Length();
            Vector2 correction = overlap * 0.5f * normal;
            p1.Position += correction;
            p2.Position -= correction;
        }
    }
}