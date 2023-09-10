using SandKing.FallingSand.Materials;
using SandKing.Graphics;
using System;
using VectorInt;

namespace SandKing.FallingSand
{
    public class Simulation
    {
        public Simulation(uint width, uint height, Display display)
        {
            Display = display;
            Materials = new Material[width, height];
            MaterialsBuffer = new Material[width, height];
        }

        public void Simulate()
        {
            for(var x = 0; x < Materials.GetLength(0); x++)
            {
                for(var y = 0; y < Materials.GetLength(1); y++)
                {
                    if (Materials[x, y] is null) continue;
                    var (newX, newY) = Update(Materials[x, y]);
                    Display.Render(MaterialsBuffer[newX, newY]);
                }
            }

            TransferBuffer();
        }

        protected Display Display { get; set; }
        public Material[,] Materials { get; set; }
        public Material[,] MaterialsBuffer { get; set; }

        public void Add(params Material[] materials)
        {
            foreach(var material in materials)
            {
                Materials[material.MaterialPosition.X, material.MaterialPosition.Y] = material;
            }
        }

        public bool IsInBounds(VectorInt2 position)
        {
            return position.X >= 0 && position.X < Materials.GetLength(0) && position.Y >= 0 && position.Y < Materials.GetLength(1);
        }

        private (int x, int y) Update(Material material)
        {
            if (material is null) return (-1, -1);

            if (material.HasProperty(Property.MoveDown) && GetDown(material, out var down, out var bufferDown) && (down is null && bufferDown is null))
            {
                return MoveDown(material);
            }
            else if (material.HasProperty(Property.MoveUp) && GetUp(material, out var up) && up is null)
            {
                return MoveUp(material);
            }
            else if (material.HasProperty(Property.MoveDownLaterally) && GetDownLateral(material, out var downLeft, out var downRight) && (downLeft is null || downRight is null))
            {
                if (downLeft is null && downRight is null)
                {
                    var random = new Random();
                    if (random.Next(2) == 0) return MoveDownLeft(material);
                    else return MoveDownRight(material);
                }
                else if (downLeft is null) return MoveDownLeft(material);
                else return MoveDownRight(material);
            }
            else if (material.HasProperty(Property.MoveLaterally) && GetLateral(material, out var left, out var right) && (left is null || right is null))
            {
                if (left is null && right is null)
                {
                    var random = new Random();
                    if (random.Next(2) == 0) return MoveLeft(material);
                    else return MoveRight(material);
                }
                else if (left is null) return MoveLeft(material);
                else return MoveRight(material);
            }
            else
            {
                return Stay(material);
            }
        }

        private (int x, int y) Stay(Material material)
        {
            MaterialsBuffer[material.MaterialPosition.X, material.MaterialPosition.Y] = material;
            return (material.MaterialPosition.X, material.MaterialPosition.Y);
        }

        private (int x, int y) MoveDownRight(Material material)
        {
            var (x, y) = material.MaterialPosition + VectorInt2.Down + VectorInt2.Right;
            material.Position = new(x, y);
            MaterialsBuffer[x, y] = material;
            return (x, y);
        }

        private (int x, int y) MoveDownLeft(Material material)
        {
            var (x, y) = material.MaterialPosition + VectorInt2.Down + VectorInt2.Left;
            material.Position = new(x, y);
            MaterialsBuffer[x, y] = material;
            return (x, y);
        }

        private bool GetDownLateral(Material material, out Material downLeft, out Material downRight)
        {
            var downLeftPosition = material.MaterialPosition + VectorInt2.Down + VectorInt2.Left;
            var downRightPosition = material.MaterialPosition + VectorInt2.Down + VectorInt2.Right;
            var downLeftInBounds = IsInBounds(downLeftPosition);
            var downRightInBounds = IsInBounds(downRightPosition);

            if (downLeftInBounds)
            {
                downLeft = Materials[downLeftPosition.X, downLeftPosition.Y];
            }
            else
            {
                downLeft = OutOfBounds.Instance;
            }

            if (downRightInBounds)
            {
                downRight = Materials[downRightPosition.X, downRightPosition.Y];
            }
            else
            {
                downRight = OutOfBounds.Instance;
            }

            if(downLeftInBounds || downRightInBounds)
            {
                return true;
            }

            return false;
        }

        private (int x, int y) MoveRight(Material material)
        {
            var (x, y) = material.MaterialPosition + VectorInt2.Right;
            material.Position = new(x, y);
            MaterialsBuffer[x, y] = material;
            return (x, y);
        }

        private (int x, int y) MoveLeft(Material material)
        {
            var (x, y) = material.MaterialPosition + VectorInt2.Left;
            material.Position = new(x, y);
            MaterialsBuffer[x, y] = material;
            return (x, y);
        }

        private bool GetLateral(Material material, out Material left, out Material right)
        {
            var leftPosition = material.MaterialPosition + VectorInt2.Left;
            var rightPosition = material.MaterialPosition + VectorInt2.Right;
            var leftInBounds = IsInBounds(leftPosition);
            var rightInBounds = IsInBounds(rightPosition);

            if (leftInBounds)
            {
                left = Materials[leftPosition.X, leftPosition.Y];
            }
            else
            {
                left = OutOfBounds.Instance;
            }

            if (rightInBounds)
            {
                right = Materials[rightPosition.X, rightPosition.Y];
            }
            else
            { 
                right = OutOfBounds.Instance; 
            }

            if (leftInBounds || rightInBounds)
            {
                return true;
            }

            return false;
        }

        private (int x, int y) MoveUp(Material material)
        {
            var (x, y) = material.MaterialPosition + VectorInt2.Up;
            material.Position = new(x, y);
            MaterialsBuffer[x, y] = material;
            return (x, y);
        }

        private bool GetUp(Material material, out Material up)
        {
            var (x, y) = material.MaterialPosition + VectorInt2.Up;
            if (IsInBounds(new VectorInt2(x, y)))
            {
                up = Materials[x, y];
                return true;
            }

            up = OutOfBounds.Instance;
            return false;
        }

        private (int x, int y) MoveDown(Material material)
        {
            var (x, y) = material.MaterialPosition + VectorInt2.Down;
            material.Position = new(x, y);
            MaterialsBuffer[x, y] = material;
            return (x, y);
        }

        private bool GetDown(Material material, out Material down, out Material bufferDown)
        {
            var (x, y) = material.MaterialPosition + VectorInt2.Down;
            if (IsInBounds(new VectorInt2(x, y)))
            {
                down = Materials[x, y];
                bufferDown = MaterialsBuffer[x, y];
                return true;
            }

            down = OutOfBounds.Instance;
            bufferDown = OutOfBounds.Instance;
            return false;
        }

        private void TransferBuffer()
        {
            Array.Copy(MaterialsBuffer, Materials, MaterialsBuffer.Length);
            Array.Clear(MaterialsBuffer, 0, MaterialsBuffer.Length);
        }
    }
}