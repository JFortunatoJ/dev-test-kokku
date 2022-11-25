using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            Team _playerTeam = null;
            Team _enemyTeam = null;

            int currentTurn = 0;
            Battlefield battlefield = null;
            int numberOfPossibleTiles;

            Setup();


            void Setup()
            {
                SetupCharacters(() =>
                {
                    Console.ForegroundColor = _playerTeam.Color;
                    Console.Write(_playerTeam.Name);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($" Class Choice: {_playerTeam.});
                    
                    GetBattlefieldSize(StartGame);
                });
            }

            void SetupCharacters(Action callback)
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

                _playerTeam = CreateTeam("Player", 3, ConsoleColor.Blue, (CharacterClass)choice);

                var rand = new Random();
                int randomInteger = rand.Next(1, 5);
                CharacterClass enemyClass = (CharacterClass)randomInteger;

                _enemyTeam = CreateTeam("Enemy", 3, ConsoleColor.Red, enemyClass);
                Console.Write(Environment.NewLine + Environment.NewLine);

                callback?.Invoke();
            }

            Team CreateTeam(string name, int charactersAmount, ConsoleColor color, CharacterClass characterClass)
            {
                Team newTeam = new Team(name, color);
                for (int i = 0; i < charactersAmount; i++)
                {
                    Character newCharacter = new Character(i, newTeam, characterClass);
                    newTeam.characters.Add(newCharacter);
                }

                return newTeam;
            }

            void AllocatePlayer(Action callback)
            {
                for (int i = 0; i < _playerTeam.characters.Count; i++)
                {
                    Tile randomLocation;
                    do
                    {
                        (int, int) randomPosition = GetRandomPositionAtGrid();
                        randomLocation = battlefield.grid[randomPosition.Item1, randomPosition.Item2];
                    } while (randomLocation.IsOccupied);

                    Console.Write($"[{randomLocation.position.x},{randomLocation.position.y}]\n");
                    _playerTeam.characters[i].WalkTo(randomLocation);
                }

                callback?.Invoke();
            }

            void GetBattlefieldSize(Action callback)
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

                callback?.Invoke();
            }

            void AllocateEnemy(Action callback)
            {
                for (int i = 0; i < _enemyTeam.characters.Count; i++)
                {
                    Tile randomLocation;
                    do
                    {
                        (int, int) randomPosition = GetRandomPositionAtGrid();
                        randomLocation = battlefield.grid[randomPosition.Item1, randomPosition.Item2];
                    } while (randomLocation.IsOccupied);

                    Console.Write($"[{randomLocation.position.x},{randomLocation.position.y}]\n");
                    _enemyTeam.characters[i].WalkTo(randomLocation);
                }

                callback?.Invoke();
            }

            void StartGame()
            {
                AllocatePlayer(() => { AllocateEnemy(() => { StartTurn(); }); });
            }

            void StartTurn()
            {
                //ClearConsoleLines(battlefield.Lines + 9);

                if (currentTurn == 0)
                {
                    Random rand = new Random();
                    int x = rand.Next(0, 2);
                    if (x == 0)
                    {
                        _playerTeam.StartTurn(battlefield);
                    }
                    else
                    {
                        _enemyTeam.StartTurn(battlefield);
                    }
                }
                else
                {
                    _playerTeam.StartTurn(battlefield);
                    _enemyTeam.StartTurn(battlefield);
                }

                currentTurn++;
                HandleTurn();
            }

            void HandleTurn()
            {
                battlefield.DrawBattlefield();

                Team winner = GetWinner();
                if (winner == null)
                {
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

                    return;
                }

                Console.WriteLine("Game Over!");
                Console.Write("Team: ");
                Console.ForegroundColor = winner.Color;
                Console.Write(winner.Name);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("  has won!");
            }

            Team GetWinner()
            {
                if (_playerTeam.AllDead)
                {
                    return _enemyTeam;
                }


                if (_enemyTeam.AllDead)
                {
                    return _playerTeam;
                }

                return null;
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