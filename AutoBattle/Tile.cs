using System;
using System.Drawing;
using System.Globalization;

namespace AutoBattle
{
    public class Tile
    {
        public int lineIndex;
        public int columnIndex;
        public Character character;

        public bool IsOccupied => character != null;

        public Tile(int lineIndex, int columnIndex)
        {
            this.lineIndex = lineIndex;
            this.columnIndex = columnIndex;
        }

        public string GetTileContent()
        {
            return !IsOccupied ? "   " : character.Health.ToString(CultureInfo.InvariantCulture);
        }

        public ConsoleColor GetColor()
        {
            return IsOccupied ? character.Color : ConsoleColor.White;
        }
    }
}