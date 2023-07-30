namespace SandKing.Simulation
{
    internal class Sand : Element
    {
        public Sand() : this(0, 0) { }

        public Sand(int x, int y) : base(x, y)
        {
            //FallingSand.Instance.Cells[x, y] = this;
        }

        public override void Update(int x, int y)
        {
            var moveDown =      FallingSand.Instance.IsEmpty(X    , Y + 1);
            var moveDownLeft =  FallingSand.Instance.IsEmpty(X - 1, Y + 1);
            var moveDownRight = FallingSand.Instance.IsEmpty(X + 1, Y + 1);

            if (moveDown)
            {
                FallingSand.Instance.MoveElement(this, x, y, X, Y + 1);
            }
            else if (moveDownLeft)
            {
                FallingSand.Instance.MoveElement(this, x, y, X - 1, Y + 1);
            }
            else if (moveDownRight)
            {
                FallingSand.Instance.MoveElement(this, x, y, X + 1, Y + 1);
            }
        }
    }
}
