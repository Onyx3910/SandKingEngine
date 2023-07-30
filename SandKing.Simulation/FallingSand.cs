using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Windows.UI;
using Windows.UI.Xaml;

namespace SandKing.Simulation
{
    public class FallingSand : ISimulation
    {
        const int DefaultCellWidth = 2;
        const int DefaultCellHeight = 2;

        private static FallingSand _instance;

        protected Color[,] _cells;
        protected Color[,] _displayedCells;
        protected List<((int x, int y) source, (int x, int y) dest)> _changes = new List<((int x, int y) source, (int x, int y) dest)>();

        public FallingSand() : this(DefaultCellWidth, DefaultCellHeight) { }

        public FallingSand(int cellWidth, int cellHeight)
        {
            var coreWindow = Window.Current.CoreWindow;
            var screenWidth = (int)coreWindow.Bounds.Width;
            var screenHeight = (int)coreWindow.Bounds.Height;
            var cellsWidth = screenWidth / cellWidth;
            var cellsHeight = screenHeight / cellHeight;

            _cells = new Color[cellsWidth, cellsHeight];
            _displayedCells = new Color[cellsWidth, cellsHeight];

            CellWidth = cellWidth;
            CellHeight = cellHeight;

            Instance = this;

            for (var i = 0; i < 167; i++)
            {
                _cells[50, cellsHeight - i - 1] = Colors.LightYellow;
            }

            for (var i = 0; i < 167; i++)
            {
                //var sand = new Sand(50, i);
                _cells[51, i] = Colors.LightYellow;
                //_displayedCells[10, i] = Colors.LightYellow;
            }
        }

        public bool PlacingSand { get; set; }
        public int CellWidth { get; }
        public int CellHeight { get; }
        public Color[,] Cells => _cells;
        public Color[,] DisplayedCells => _displayedCells;

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

        public void PlaceSand()
        {
            var x = (int)(Window.Current.CoreWindow.PointerPosition.X - Window.Current.Bounds.X) / CellWidth;
            var y = (int)(Window.Current.CoreWindow.PointerPosition.Y - Window.Current.Bounds.Y) / CellHeight;
            if (!IsEmpty(x, y)) return;
            _cells[x, y] = Colors.LightYellow;
        }

        public void MoveElement(Element element, int x, int y, int destX, int destY)
        {
            if (IsEmpty(destX, destY))
            {
                //_cells[destX, destY] = element;
                //_cells[x, y] = null;
            }
        }

        public bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < Cells.GetLength(0) && y < Cells.GetLength(1);

        public bool IsEmpty(int x, int y) => InBounds(x, y) && (_cells[x, y] == default || _cells[x, y] == Colors.Black);

        public virtual void MoveCell(int sourceX, int sourceY, int destX, int destY)
        {
            _changes.Add(((sourceX, sourceY), (destX, destY)));
        }

        public virtual void CommitCells()
        {
            var orderedCells = new Dictionary<(int x, int y), LinkedList<(int x, int y)>>();
            var cellGroups = new List<LinkedList<(int x, int y)>>();
            for (var x = 0; x < _displayedCells.GetLength(0); x++)
            {
                for (var y = 0; y < _displayedCells.GetLength(1); y++)
                {
                    var cell = _cells[x, y];
                    if (cell != Colors.LightYellow) continue;
                    var linkedList = new LinkedList<(int x, int y)>();
                    if (orderedCells.ContainsKey((x, y))) continue;
                    else orderedCells.Add((x, y), linkedList);
                    linkedList.AddLast((x, y));
                    var subX = x;
                    var subY = y;
                    while (cell != default || cell != Colors.Black) 
                    {
                        if (!InBounds(subX, subY + 1)) break;
                        subY += 1;
                        cell = _cells[subX, subY];
                        if (cell != Colors.LightYellow) break;
                        if (orderedCells.ContainsKey((subX, subY))) break;
                        else orderedCells.Add((subX, subY), linkedList);
                        linkedList.AddLast((subX, subY));
                    }
                    cellGroups.Add(linkedList);
                }
            }

            foreach(var group in cellGroups)
            {
                var cell = group.Last;
                while (cell != null)
                {
                    var moveDown =      IsEmpty(cell.Value.x,     cell.Value.y + 1);
                    var moveDownLeft =  IsEmpty(cell.Value.x - 1, cell.Value.y + 1);
                    var moveDownRight = IsEmpty(cell.Value.x + 1, cell.Value.y + 1);

                    if (moveDown)
                    {
                        _cells[cell.Value.x, cell.Value.y + 1] = Colors.LightYellow;
                        _cells[cell.Value.x, cell.Value.y    ] = Colors.Black;
                    }
                    else if(moveDownLeft)
                    {
                        _cells[cell.Value.x - 1, cell.Value.y + 1] = Colors.LightYellow;
                        _cells[cell.Value.x,     cell.Value.y    ] = Colors.Black;
                    }
                    else if(moveDownRight)
                    {
                        _cells[cell.Value.x + 1, cell.Value.y + 1] = Colors.LightYellow;
                        _cells[cell.Value.x,     cell.Value.y    ] = Colors.Black;
                    }    
                    
                    cell = cell.Previous;
                }
            }
        }

        public virtual void Simulate(CanvasDrawingSession session) 
        {
            //for(var x = 0; x < _displayedCells.GetLength(0); x++)
            //{
            //    for(var y = 0; y < _displayedCells.GetLength(1); y++)
            //    {
            //        var cell = _displayedCells[x, y];
            //        if(cell == default || cell == Colors.Black) continue;
            //        //element.Update(x, y);
            //        //element.Draw(session);

            //        MoveCell(x, y, x, y + 1);

            //        //var moveDown =      IsEmpty(x,     y + 1);
            //        //var moveDownLeft =  IsEmpty(x - 1, y + 1);
            //        //var moveDownRight = IsEmpty(x + 1, y + 1);

            //        //if(moveDown)
            //        //{
            //        //    MoveCell(x, y, x, y + 1);
            //        //}
            //        //else if(moveDownLeft)
            //        //{
            //        //    MoveCell(x, y, x - 1, y + 1);
            //        //}
            //        //else if(moveDownRight)
            //        //{
            //        //    MoveCell(x, y, x + 1, y + 1);
            //        //}

            //        //if (moveDown || moveDownLeft || moveDownRight) _cells[x, y] = Colors.Black;
            //    }
            //}

            if (PlacingSand) PlaceSand();
            CommitCells();
            Display(session);
        }

        protected void Display(CanvasDrawingSession session)
        {
            Array.Copy(_cells, _displayedCells, _displayedCells.Length);

            for (var x = 0; x < _displayedCells.GetLength(0); x++)
            {
                for (var y = 0; y < _displayedCells.GetLength(1); y++)
                {
                    var cell = _displayedCells[x, y];
                    if (cell == default || cell == Colors.Black) continue;
                    session.FillRectangle(x * CellWidth, y * CellHeight, CellWidth, CellHeight, cell);
                }
            }
        }
    }
}