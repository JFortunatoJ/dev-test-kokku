using System;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Battlefield
    {
        private int _lines;
        private int _columns;

        public int Lines => _lines;
        public int Columns => _columns;

        public int TilesAmount => Lines * Columns;

        public Tile[,] grid;

        public Battlefield(int lines, int columns)
        {
            _lines = lines;
            _columns = columns;

            grid = new Tile[lines, columns];

            for (int i = 0; i < lines; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    grid[i, j] = new Tile(i, j);
                }
            }

            Console.WriteLine("The battle field has been created\n");
        }

        // prints the matrix that indicates the tiles of the battlefield
        public void DrawBattlefield()
        {
            for (int i = 0; i < _lines; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("[");
                    Console.ForegroundColor = grid[i, j].GetColor();
                    Console.Write($"{grid[i, j].GetTileContent()}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("]\t");
                }

                Console.Write(Environment.NewLine + Environment.NewLine);
            }

            Console.Write(Environment.NewLine + Environment.NewLine);
        }
    }
}