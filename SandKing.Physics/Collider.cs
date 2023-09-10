namespace SandKing.Physics
{
    public abstract class Collider
    {
        public Body Body { get; set; }

        public abstract bool IsColliding(Collider other);
        public abstract void ResolveCollision(Collider other);
    }
}