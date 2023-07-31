using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using static SandKing.Simulation.Elements;

namespace SandKing.Simulation
{
    public class FallingSand : ISimulation
    {
        const int DefaultCellWidth = 2;
        const int DefaultCellHeight = 2;

        private static FallingSand _instance;

        protected Cell[,] _cells;

        public FallingSand() : this(DefaultCellWidth, DefaultCellHeight) { }

        public FallingSand(int cellWidth, int cellHeight)
        {
            CellWidth = cellWidth;
            CellHeight = cellHeight;

            var coreWindow = Window.Current.CoreWindow;
            var screenWidth = (int)coreWindow.Bounds.Width;
            var screenHeight = (int)coreWindow.Bounds.Height;
            var cellsWidth = screenWidth / cellWidth;
            var cellsHeight = screenHeight / cellHeight;

            _cells = new Cell[cellsWidth, cellsHeight];

            Instance = this;
        }

        public Cell CellToPlace { get; set; }
        public bool MouseDown { get; set; }
        public int BrushSize { get; set; } = 10;
        public int CellWidth { get; }
        public int CellHeight { get; }
        public Cell[,] Cells => _cells;

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
            var mouseX = (int)(Window.Current.CoreWindow.PointerPosition.X - Window.Current.Bounds.X) / CellWidth;
            var mouseY = (int)(Window.Current.CoreWindow.PointerPosition.Y - Window.Current.Bounds.Y) / CellHeight;
            for (var x = mouseX - BrushSize / 2; x < mouseX + BrushSize / 2; x++)
            { 
                for(var y = mouseY - BrushSize / 2; y < mouseY + BrushSize / 2; y++)
                {
                    if (CellToPlace != Empty && (!IsEmpty(x, y) || !InBounds(x, y))) continue;
                    _cells[x, y] = CellToPlace;
                }
            }
        }

        public bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < Cells.GetLength(0) && y < Cells.GetLength(1);

        public bool IsEmpty(int x, int y) => InBounds(x, y) && (_cells[x, y] == Empty);

        public virtual void Update()
        {
            var points = new List<List<(int x, int y)>>();

            for (var y = _cells.GetLength(1) - 1; y >= 0; y--)
            {
                var row = new List<(int x, int y)>();
                for (var x = 0; x < _cells.GetLength(0); x++)
                {
                    var cell = _cells[x, y];
                    if (cell == Empty) continue;
                    row.Add((x, y));
                }
                if(row.Count > 0) points.Add(row);
            }

            foreach (var row in points)
            {
                foreach (var index in Enumerable.Range(0, row.Count).OrderBy(x => new Random().Next()))
                {
                    var point = row[index];

                    var (x, y) = point;
                    var cell = _cells[x, y];

                    if (cell == Empty) return;

                    var canMoveDown = cell.HasProperty(CellProperties.MoveDown);
                    var canMoveDownSide = cell.HasProperty(CellProperties.MoveDownSide);
                    var canMoveSide = cell.HasProperty(CellProperties.MoveSide);

                    if (canMoveDown && MoveDown(x, y, cell)) { }
                    else if (canMoveDownSide && MoveDownSide(x, y, cell)) { }
                    else if (canMoveSide && MoveSide(x, y, cell)) { }
                }
            }
        }

        public virtual void Simulate(CanvasDrawingSession session) 
        {
            if (MouseDown) PlaceCell();
            Update();
            Display(session);
        }

        protected void Display(CanvasDrawingSession session)
        {
            for (var x = 0; x < _cells.GetLength(0); x++)
            {
                for (var y = 0; y < _cells.GetLength(1); y++)
                {
                    var cell = _cells[x, y];
                    if (cell == Empty) continue;
                    session.FillRectangle(x * CellWidth, y * CellHeight, CellWidth, CellHeight, cell.Color);
                }
            }
        }

        protected void MoveCell(int xFrom, int yFrom, int xTo, int yTo, Cell cell)
        {
            _cells[xTo, yTo] = cell;
            _cells[xFrom, yFrom] = Empty;
        }

        protected bool MoveDown(int x, int y, Cell cell)
        {
            var down = IsEmpty(x, y + 1);
            if(down) MoveCell(x, y, x, y + 1, cell);
            return down;
        }

        protected bool MoveDownSide(int x, int y, Cell cell)
        {
            var leftX = x - 1;
            var rightX = x + 1;
            var downY = y + 1;

            var downLeft = IsEmpty(leftX, downY);
            var downRight = IsEmpty(rightX, downY);

            if (downLeft && downRight)
            {
                downLeft = new Random().Next() >= int.MaxValue / 2;
                downRight = !downLeft;
            }

            if (downLeft) MoveCell(x, y, leftX, downY, cell);
            else if (downRight) MoveCell(x, y, rightX, downY, cell);

            return downLeft || downRight;
        }

        protected bool MoveSide(int x, int y, Cell cell)
        {
            var leftX = x - 1;
            var rightX = x + 1;

            var left = IsEmpty(leftX, y);
            var right = IsEmpty(rightX, y);

            if (left && right)
            {
                left = new Random().Next() >= int.MaxValue / 2;
                right = !left;
            }

            if (left) MoveCell(x, y, leftX, y, cell);
            else if (right) MoveCell(x, y, rightX, y, cell);

            return left || right;
        }
    }
}