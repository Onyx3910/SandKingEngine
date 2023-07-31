using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;

namespace SandKing.Simulation
{
    public enum CellProperties
    {
        None = 0b00000000,
        MoveDown = 0b00000001,
        MoveDownSide = 0b00000010,
        MoveSide = 0b00000100
    }

    public enum CellType
    {
        Solid,
        Liquid,
        Gas
    }

    public readonly struct Cell
    {
        public Cell(CellType type, CellProperties properties, Color color)
        {
            Type = type;
            Properties = properties;
            Color = color;
        }

        public CellType Type { get; }
        public CellProperties Properties { get; }
        public Color Color { get; }

        public static bool operator ==(Cell left, Cell right) => left.Type == right.Type && left.Properties == right.Properties && left.Color == right.Color;

        public static bool operator !=(Cell left, Cell right) => left.Type != right.Type || left.Properties != right.Properties || left.Color != right.Color;

        public bool HasProperty(CellProperties property) => (Properties & property) == property;

        public override bool Equals(object obj)
        {
            return obj is Cell cell &&
                   Type == cell.Type &&
                   Properties == cell.Properties &&
                   EqualityComparer<Color>.Default.Equals(Color, cell.Color);
        }

        public override int GetHashCode()
        {
            int hashCode = 1072091153;
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + Properties.GetHashCode();
            hashCode = hashCode * -1521134295 + Color.GetHashCode();
            return hashCode;
        }
    }

    public static class Elements
    {
        public static readonly Cell Empty = new Cell();

        public static readonly Cell Sand = new Cell(CellType.Solid, 
                                                    CellProperties.MoveDown | CellProperties.MoveDownSide, 
                                                    Color.FromArgb(255, 235, 200, 175));

        public static readonly Cell Water = new Cell(CellType.Liquid,
                                                     CellProperties.MoveDown | CellProperties.MoveDownSide | CellProperties.MoveSide,
                                                     Colors.Blue);

        public static readonly Cell Stone = new Cell(CellType.Solid,
                                                     CellProperties.None,
                                                     Colors.Gray);
    }
}
