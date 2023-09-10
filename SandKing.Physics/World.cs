using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UltimateQuadTree;

namespace SandKing.Physics
{
    public class World
    {
        public World(uint width, uint height, Vector2 gravity)
        {
            Width = width;
            Height = height;
            Gravity = gravity;
            Bodies = new List<Body>();
            QuadTree = new QuadTree<Body>(0, 0, (double)width, (double)height, Body.Bounds);
        }

        public uint Width { get; }
        public uint Height { get; }
        public Vector2 Gravity { get; set; }
        protected List<Body> Bodies { get; set; }
        protected QuadTree<Body> QuadTree { get; set; }

        public void Step(float deltaTime)
        {
            Simulate(deltaTime);
            UpdateQuadTree();
        }

        public void Add(params Body[] bodies)
        {
            Bodies.AddRange(bodies);
        }

        protected void Simulate(float deltaTime)
        {
            foreach(var body in Bodies)
            {
                body.ApplyForce(Gravity, impulse: true);
                body.Verlet(deltaTime);
            }
        }

        protected void UpdateQuadTree()
        {
            QuadTree.Clear();
            QuadTree.InsertRange(Bodies);
        }

        protected void HandleCollisions()
        {
            var bodies = QuadTree.GetObjects().ToArray();
            for(var index = 0; index < bodies.Length; index++)
            {
                var body = bodies[index];
                var otherBodies = QuadTree.GetNearestObjects(body).ToArray();
                for(var otherIndex = 0; otherIndex < otherBodies.Length; otherIndex++)
                {
                    var other = otherBodies[otherIndex];
                    if (body == other) continue;
                    if (!body.IsColliding(other)) continue;
                    body.Collider.ResolveCollision(other.Collider);
                }
            }
        }
    }
}
