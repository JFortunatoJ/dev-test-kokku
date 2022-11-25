using System;
using System.Collections.Generic;
using System.Drawing;
using static AutoBattle.Types;

namespace AutoBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            CharacterClass playerCharacterClass;
            Character playerCharacter;
            Character enemyCharacter;
            List<Character> playerCharacters = new List<Character>();
            List<Character> enemyCharacters = new List<Character>();
            int currentTurn = 0;
            Battlefield battlefield;
            int numberOfPossibleTiles;

            Setup();


            void Setup()
            {
                GetPlayerChoice();
            }

            void GetPlayerChoice()
            {
                int choice;
                bool validChoice;
                do
                {
                    Console.WriteLine("Choose Between One of this Classes:");
                    Console.WriteLine("[1] Paladin, [2] Warrior, [3] Cleric, [4] Archer");
                    string choiceString = Console.ReadLine();

                    validChoice = int.TryParse(choiceString, out choice) && choice > 0 && choice <= 4;

                    if (!validChoice)
                    {
                        ClearConsoleLines(3);
                    }
                } while (!validChoice);

                CreatePlayerCharacter(choice);
            }

            void CreatePlayerCharacter(int classIndex)
            {
                CharacterClass characterClass = (CharacterClass)classIndex;
                playerCharacter = new Character(0, "Player", characterClass, ConsoleColor.Blue);
                playerCharacters.Add(playerCharacter);

                Console.ForegroundColor = playerCharacter.Color;
                Console.Write("Player");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($" Class Choice: {characterClass}");

                CreateEnemyCharacter();
            }

            void AllocatePlayer()
            {
                Tile randomLocation;
                do
                {
                    (int, int) randomPosition = GetRandomPositionAtGrid();
                    randomLocation = battlefield.grid[randomPosition.Item1, randomPosition.Item2];
                } while (randomLocation.IsOccupied);

                Console.Write($"[{randomLocation.position.x},{randomLocation.position.y}]\n");
                
                playerCharacter.WalkTo(randomLocation);
            }

            void GetBattlefieldSize()
            {
                int columns = 0;
                int lines = 0;

                bool validValue;

                do
                {
                    Console.WriteLine("How many columns should the battlefield have?");
                    string columnsString = Console.ReadLine();

                    validValue = int.TryParse(columnsString, out columns);
                    if (!validValue)
                    {
                        ClearConsoleLines(2);
                    }
                } while (!validValue);

                do
                {
                    Console.WriteLine("And how many lines should the battlefield have?");
                    string linesString = Console.ReadLine();

                    validValue = int.TryParse(linesString, out lines);
                    if (!validValue)
                    {
                        ClearConsoleLines(2);
                    }
                } while (!validValue);

                battlefield = new Battlefield(lines, columns);
                numberOfPossibleTiles = battlefield.SizeX * battlefield.SizeY;

                StartGame();
            }

            void CreateEnemyCharacter()
            {
                //randomly choose the enemy class and set up vital variables
                var rand = new Random();
                int randomInteger = rand.Next(1, 5);
                CharacterClass enemyClass = (CharacterClass)randomInteger;
                enemyCharacter = new Character(1, "Enemy", enemyClass, ConsoleColor.Red);
                enemyCharacters.Add(enemyCharacter);

                Console.Write(Environment.NewLine);
                Console.ForegroundColor = enemyCharacter.Color;
                Console.Write("Enemy");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($" Class Choice: {enemyClass}");
                Console.Write(Environment.NewLine + Environment.NewLine);

                GetBattlefieldSize();
            }

            void AllocateEnemy()
            {
                Tile randomLocation;
                do
                {
                    (int, int) randomPosition = GetRandomPositionAtGrid();
                    randomLocation = battlefield.grid[randomPosition.Item1, randomPosition.Item2];
                } while (randomLocation.IsOccupied);

                Console.Write($"[{randomLocation.position.x},{randomLocation.position.y}]\n");
                enemyCharacter.WalkTo(randomLocation);
                
                StartTurn();
            }

            void StartGame()
            {
                AllocatePlayer();
                AllocateEnemy();
                
                
            }

            void StartTurn()
            {
                //ClearConsoleLines(battlefield.Lines + 9);

                if (currentTurn == 0)
                {
                    //TODO: get random initial player
                }

                foreach (Character character in playerCharacters)
                {
                    character.StartTurn(battlefield);
                }

                /*
                foreach (Character enemy in enemyCharacters)
                {
                    enemy.StartTurn(battlefield);
                }
                */

                currentTurn++;
                HandleTurn();
            }

            void HandleTurn()
            {
                battlefield.DrawBattlefield();

                if (playerCharacter.Health == 0)
                {
                    Console.WriteLine("Game Over");
                    return;
                }

                if (enemyCharacter.Health == 0)
                {
                    Console.WriteLine("Victory!");
                    return;
                }

                ConsoleKeyInfo key;
                do
                {
                    Console.Write(Environment.NewLine);
                    Console.WriteLine("Press enter to start the next turn...\n");

                    key = Console.ReadKey();

                    if (key.Key != ConsoleKey.Enter)
                    {
                        ClearConsoleLines(4);
                    }
                } while (key.Key != ConsoleKey.Enter);

                StartTurn();
            }

            int GetRandomInt(int min, int max)
            {
                var rand = new Random();
                return rand.Next(min, max);
            }
            
            (int, int) GetRandomPositionAtGrid()
            {
                return (GetRandomInt(0, battlefield.SizeX), GetRandomInt(0, battlefield.SizeY));
            }
        }

        public static void ClearConsoleLines(int linesAmount)
        {
            for (int i = 0; i < linesAmount; i++)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                ClearCurrentConsoleLine();
            }
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}