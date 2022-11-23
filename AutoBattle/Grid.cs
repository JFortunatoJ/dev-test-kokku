using System;
using System.Collections.Generic;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Grid
    {
        private int _xLenght;
        private int _yLength;

        public int XLength => _xLenght;
        public int YLength => _yLength;

        public List<GridBox> grids = new List<GridBox>();

        public Grid(int lines, int columns)
        {
            _xLenght = lines;
            _yLength = columns;

            for (int i = 0; i < lines; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    GridBox newBox = new GridBox(j, i, false, (columns * i + j));
                    grids.Add(newBox);
                    Console.Write($"{newBox.index}\n");
                }
            }

            Console.WriteLine("The battle field has been created\n");
        }

        // prints the matrix that indicates the tiles of the battlefield
        public void DrawBattlefield(int lines, int columns)
        {
            for (int i = 0; i < lines; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    GridBox currentgrid = new GridBox();
                    if (currentgrid.ocupied)
                    {
                        //if()
                        Console.Write("[X]\t");
                    }
                    else
                    {
                        Console.Write($"[ ]\t");
                    }
                }

                Console.Write(Environment.NewLine + Environment.NewLine);
            }

            Console.Write(Environment.NewLine + Environment.NewLine);
        }
    }
}