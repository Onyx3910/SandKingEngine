using Microsoft.Graphics.Canvas;
using SandKing.Physics;
using SandKing.Simulation.Materials;
using SandKing.Simulation.Materials.Base;
using SandKing.Simulation.Materials.Enums;
using SandKing.Simulation.Materials.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
//using tainicom.Aether.Physics2D.Common;
//using tainicom.Aether.Physics2D.Dynamics;
using VectorInt;
using Windows.UI.Xaml;

namespace SandKing.Simulation
{
    public class FallingSand : ISimulation
    {
        private static FallingSand _instance;

        protected IMaterial[,] _grid;

        public FallingSand()
        {
            var coreWindow = Window.Current.CoreWindow;
            var screenWidth = (int)coreWindow.Bounds.Width;
            var screenHeight = (int)coreWindow.Bounds.Height;
            var cellsWidth = screenWidth / Material.Size;
            var cellsHeight = screenHeight / Material.Size;

            _grid = new IMaterial[cellsWidth, cellsHeight];

            Instance = this;

            World = new World(cellsWidth, cellsHeight, new Vector2(0f, 9.80665f));
            var sand1 = new Sand(this, (50, 0))
            {
                IsParticle = true
            };

            var sand2 = new Sand(this, (50, cellsHeight - 1))
            {
                IsParticle = false
            };
        }

        public World World { get; }
        public MaterialType MaterialToPlace { get; set; }
        public bool MouseDown { get; set; }
        public int BrushSize { get; set; } = 10;
        public IMaterial[,] Grid => _grid;

        public static FallingSand Instance
        {
            get
            {
                if(_instance == null) _instance = new FallingSand();
                return _instance;
            }
            private set
            {
                if(_instance == null) _instance = value;
            }
        }

        public void PlaceCell()
        {
            var mouseX = (int)(Window.Current.CoreWindow.PointerPosition.X - Window.Current.Bounds.X) / Material.Size;
            var mouseY = (int)(Window.Current.CoreWindow.PointerPosition.Y - Window.Current.Bounds.Y) / Material.Size;
            for (var x = mouseX - BrushSize / 2; x < mouseX + BrushSize / 2; x++)
            { 
                for(var y = mouseY - BrushSize / 2; y < mouseY + BrushSize / 2; y++)
                {
                    if (MaterialToPlace != MaterialType.None && (!IsEmpty(x, y) || !InBounds(x, y))) continue;
                    switch (MaterialToPlace)
                    {
                        case MaterialType.None:
                            Grid[x, y] = null;
                            break;
                        case MaterialType.Sand:
                            new Sand(this, (x, y))
                            {
                                IsParticle = true
                            };
                            break;
                        case MaterialType.Water:
                            new Water(this, (x, y));
                            break;
                        case MaterialType.Stone:
                            new Stone(this, (x, y));
                            break;
                    }
                }
            }
        }

        public bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < Grid.GetLength(0) && y < Grid.GetLength(1);

        public bool IsEmpty(int x, int y) => InBounds(x, y) && (_grid[x, y] == null);

        public virtual void Update(CanvasDrawingSession session)
        {
            var points = new List<List<VectorInt2>>();

            for (var y = _grid.GetLength(1) - 1; y >= 0; y--)
            {
                var row = new List<VectorInt2>();
                for (var x = 0; x < _grid.GetLength(0); x++)
                {
                    var material = _grid[x, y];
                    if (material == null) continue;
                    row.Add(new VectorInt2(x, y));
                }
                if(row.Count > 0) points.Add(row);
            }

            foreach (var row in points)
            {
                foreach (var index in Enumerable.Range(0, row.Count).OrderBy(x => new Random().Next()))
                {
                    var position = row[index];
                    var material = _grid[position.X, position.Y];

                    if (material == null) return;

                    material.Update(session);
                }
            }
        }

        public virtual void Simulate(CanvasDrawingSession session) 
        {
            World.Step(1f / 60f);
            if (MouseDown) PlaceCell();
            Update(session);
        }
    }
}