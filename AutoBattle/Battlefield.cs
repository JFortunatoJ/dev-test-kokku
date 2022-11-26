using System;

namespace AutoBattle
{
    public class Battlefield
    {
        private int _sizeX;
        private int _sizeY;

        public int SizeX => _sizeX;
        public int SizeY => _sizeY;

        public int TilesAmount => SizeX * SizeY;

        public Tile[,] grid;

        public Battlefield(int sizeX, int sizeY)
        {
            _sizeX = sizeX;
            _sizeY = sizeY;

            grid = new Tile[sizeX, sizeY];

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    grid[i, j] = new Tile(new Types.Vector2(i, j));
                }
            }
        }

        // prints the matrix that indicates the tiles of the battlefield
        public void DrawBattlefield()
        {
            Console.Write(Environment.NewLine);
            
            for (int i = 0; i < _sizeX; i++)
            {
                for (int j = 0; j < _sizeY; j++)
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
        }
    }
}