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
                Console.WriteLine($"Player Class Choice: {characterClass}");
                playerCharacter = new Character(0, "Player", characterClass, ConsoleColor.Blue);
                playerCharacters.Add(playerCharacter);
                GetBattlefieldSize();
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
                numberOfPossibleTiles = battlefield.Lines * battlefield.Columns;

                CreateEnemyCharacter();
            }

            void CreateEnemyCharacter()
            {
                //randomly choose the enemy class and set up vital variables
                var rand = new Random();
                int randomInteger = rand.Next(1, 5);
                CharacterClass enemyClass = (CharacterClass)randomInteger;
                Console.WriteLine($"Enemy Class Choice: {enemyClass}");
                enemyCharacter = new Character(1, "Enemy", enemyClass, ConsoleColor.Red);
                enemyCharacters.Add(enemyCharacter);
                StartGame();
            }

            void StartGame()
            {
                enemyCharacter.Target = playerCharacter;
                playerCharacter.Target = enemyCharacter;
                
                AllocatePlayers();
                StartTurn();
            }

            void StartTurn()
            {
                if (currentTurn == 0)
                {
                    //TODO: get random initial player
                }

                foreach (Character character in playerCharacters)
                {
                    character.StartTurn(battlefield);
                }

                currentTurn++;
                HandleTurn();
            }

            void HandleTurn()
            {
                if (playerCharacter.Health == 0)
                {
                    return;
                }
                else if (enemyCharacter.Health == 0)
                {
                    Console.Write(Environment.NewLine + Environment.NewLine);

                    // endgame?

                    Console.Write(Environment.NewLine + Environment.NewLine);

                    return;
                }
                else
                {
                    Console.Write(Environment.NewLine + Environment.NewLine);
                    Console.WriteLine("Click on any key to start the next turn...\n");
                    Console.Write(Environment.NewLine + Environment.NewLine);

                    ConsoleKeyInfo key = Console.ReadKey();
                    StartTurn();
                }
            }

            int GetRandomInt(int min, int max)
            {
                var rand = new Random();
                return rand.Next(min, max);
            }

            void AllocatePlayers()
            {
                AllocatePlayerCharacter();
            }

            void AllocatePlayerCharacter()
            {
                (int, int) randomPosition = GetRandomPositionAtGrid();
                Tile randomLocation = battlefield.grid[randomPosition.Item1, randomPosition.Item2];
                Console.Write($"[{randomPosition.Item1},{randomPosition.Item2}]\n");

                if (!randomLocation.IsOccupied)
                {
                    playerCharacter.WalkTo(randomLocation);
                    AllocateEnemyCharacter();
                }
                else
                {
                    AllocatePlayerCharacter();
                }
            }

            void AllocateEnemyCharacter()
            {
                (int, int) randomPosition = GetRandomPositionAtGrid();
                Tile randomLocation = battlefield.grid[randomPosition.Item1, randomPosition.Item2];
                Console.Write($"[{randomPosition.Item1},{randomPosition.Item2}]\n");
                
                if (!randomLocation.IsOccupied)
                {
                    enemyCharacter.WalkTo(randomLocation);
                    battlefield.DrawBattlefield();
                }
                else
                {
                    AllocateEnemyCharacter();
                }
            }
            
            (int, int) GetRandomPositionAtGrid()
            {
                return (GetRandomInt(0, battlefield.Lines), GetRandomInt(0, battlefield.Columns));
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