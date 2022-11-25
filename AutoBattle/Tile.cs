using System;
using System.Globalization;

namespace AutoBattle
{
    public class Tile
    {
        public Types.Vector2 position;
        public Character character;

        public bool IsOccupied => character != null;

        public Tile(Types.Vector2 position)
        {
            this.position = position;
        }

        public string GetTileContent()
        {
            if (!IsOccupied)
            {
                return "   ";
            }

            if (!character.IsDead)
            {
                return character.Health.ToString(CultureInfo.InvariantCulture);
            }

            return "XXX";
        }

        public ConsoleColor GetColor()
        {
            return IsOccupied ? character.Team.Color : ConsoleColor.White;
        }
    }
}