using System;
using System.Collections.Generic;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            Grid grid = new Grid(5, 5);
            CharacterClass playerCharacterClass;
            GridBox playerCurrentLocation;
            GridBox enemyCurrentLocation;
            Character playerCharacter;
            Character enemyCharacter;
            List<Character> allPlayers = new List<Character>();
            int currentTurn = 0;
            int numberOfPossibleTiles = grid.grids.Count;

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

                switch (choice)
                {
                    case 1:
                        CreatePlayerCharacter(choice);
                        break;
                    case 2:
                        CreatePlayerCharacter(choice);
                        break;
                    case 3:
                        CreatePlayerCharacter(choice);
                        break;
                    case 4:
                        CreatePlayerCharacter(choice);
                        break;
                    default:
                        GetPlayerChoice();
                        break;
                }
            }

            void CreatePlayerCharacter(int classIndex)
            {
                CharacterClass characterClass = (CharacterClass)classIndex;
                Console.WriteLine($"Player Class Choice: {characterClass}");
                playerCharacter = new Character(0, "Player", characterClass);

                //CreateEnemyCharacter();
                GetBattlefieldSize();
            }

            void CreateEnemyCharacter()
            {
                //randomly choose the enemy class and set up vital variables
                var rand = new Random();
                int randomInteger = rand.Next(1, 5);
                CharacterClass enemyClass = (CharacterClass)randomInteger;
                Console.WriteLine($"Enemy Class Choice: {enemyClass}");
                enemyCharacter = new Character(1, "Enemy", enemyClass);
                StartGame();
            }

            void StartGame()
            {
                //populates the character variables and targets
                enemyCharacter.Target = playerCharacter;
                playerCharacter.Target = enemyCharacter;
                allPlayers.Add(playerCharacter);
                allPlayers.Add(enemyCharacter);
                AllocatePlayers();
                StartTurn();
            }

            void GetBattlefieldSize()
            {
                int columns = 0;
                int lines = 0;

                bool validValue;

                do
                {
                    Console.WriteLine("How many columns the battlefield should have?");
                    string columnsString = Console.ReadLine();

                    validValue = int.TryParse(columnsString, out columns);
                    if (!validValue)
                    {
                        ClearConsoleLines(2);
                    }
                } while (!validValue);

                do
                {
                    Console.WriteLine("And how many lines the battlefield should have?");
                    string linesString = Console.ReadLine();

                    validValue = int.TryParse(linesString, out lines);
                    if (!validValue)
                    {
                        ClearConsoleLines(2);
                    }
                } while (!validValue);

                grid = new Grid(lines, columns);
                
                CreateEnemyCharacter();
            }

            void StartTurn()
            {
                if (currentTurn == 0)
                {
                    //AllPlayers.Sort();  
                }

                foreach (Character character in allPlayers)
                {
                    character.StartTurn(grid);
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
                int index = rand.Next(min, max);
                return index;
            }

            void AllocatePlayers()
            {
                AllocatePlayerCharacter();
            }

            void AllocatePlayerCharacter()
            {
                int random = 0;
                GridBox randomLocation = (grid.grids.ElementAt(random));
                Console.Write($"{random}\n");
                if (!randomLocation.ocupied)
                {
                    GridBox PlayerCurrentLocation = randomLocation;
                    randomLocation.ocupied = true;
                    grid.grids[random] = randomLocation;
                    playerCharacter.CurrentBox = grid.grids[random];
                    AllocateEnemyCharacter();
                }
                else
                {
                    AllocatePlayerCharacter();
                }
            }

            void AllocateEnemyCharacter()
            {
                int random = 24;
                GridBox randomLocation = (grid.grids.ElementAt(random));
                Console.Write($"{random}\n");
                if (!randomLocation.ocupied)
                {
                    enemyCurrentLocation = randomLocation;
                    randomLocation.ocupied = true;
                    grid.grids[random] = randomLocation;
                    enemyCharacter.CurrentBox = grid.grids[random];
                    grid.DrawBattlefield(5, 5);
                }
                else
                {
                    AllocateEnemyCharacter();
                }
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