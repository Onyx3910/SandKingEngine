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
using VectorInt;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace SandKing.Simulation
{
    public class FallingSand : ISimulation
    {
        private static FallingSand _instance;

        protected IMaterial[,] _grid;

        public FallingSand(int chunkLength = 1, bool debug = false)
        {
            if (_instance != null) throw new Exception("Only one instance of FallingSand is allowed.");

            _instance = this;

            Debug = debug;

            var coreWindow = Window.Current.CoreWindow;
            var screenWidth = (int)coreWindow.Bounds.Width;
            var screenHeight = (int)coreWindow.Bounds.Height;
            var cellsWidth = screenWidth / Material.Size;
            var cellsHeight = screenHeight / Material.Size;

            _grid = new IMaterial[cellsWidth, cellsHeight];

            Instance = this;

            Camera = new Camera(cellsWidth, cellsHeight, new Vector2(0, 0), 1f);
            World = new World(cellsWidth, cellsHeight, new Vector2(0f, 9.80665f));
            Chunks = new Chunk[chunkLength, chunkLength];
            Chunk.Width = cellsWidth / chunkLength;
            Chunk.Height = cellsHeight / chunkLength;
            for(var i = 0; i < chunkLength; i++)
            {
                for(var j = 0; j < chunkLength; j++)
                {
                    Chunks[i, j] = new Chunk(this, (i * Chunk.Width, j * Chunk.Height));
                }
            }

            _ = new Sand(this, (50, 0))
            {
                IsParticle = true
            };
            _ = new Sand(this, (50, cellsHeight - 1))
            {
                IsParticle = false
            };
        }

        public bool Debug { get; set; }
        public Camera Camera { get; }
        public World World { get; }
        public Chunk[,] Chunks { get; }
        public MaterialType MaterialToPlace { get; set; }
        public bool MouseDown { get; set; }
        public int BrushSize { get; set; } = 10;
        public IMaterial[,] Grid => _grid;

        public static FallingSand Instance
        {
            get
            {
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
                            new Sand(this, (x, y), Debug);
                            break;
                        case MaterialType.Water:
                            new Water(this, (x, y), Debug);
                            break;
                        case MaterialType.Stone:
                            new Stone(this, (x, y), Debug);
                            break;
                    }
                }
            }
        }

        public bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < Grid.GetLength(0) && y < Grid.GetLength(1);

        public bool IsEmpty(int x, int y) => InBounds(x, y) && (_grid[x, y] == null);

        public virtual void Update()
        {
            var random = new Random();
            foreach (var x in Enumerable.Range(0, Grid.GetLength(0)).OrderBy(num => random.Next()))
            {
                foreach(var y in Enumerable.Range(0, Grid.GetLength(1)).OrderBy(num => random.Next()))
                {
                    if (Grid[x, y] is null) continue;
                    Grid[x, y].Update();
                }
            }
        }

        public virtual void Simulate() 
        {
            World.Step(1f / 60f);
            if (MouseDown) PlaceCell();
            Update();
        }

        public void ToggleDebug()
        {
            Debug = !Debug;

            foreach (var chunk in Chunks)
            {
                chunk.Debug = Debug;
            }

            foreach (var material in _grid)
            {
                if (material == null) continue;
                material.Debug = Debug;
            }
        }
    }
}