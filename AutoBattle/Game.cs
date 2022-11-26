using System;
using System.Collections.Generic;

namespace AutoBattle
{
    public class Game
    {
        private Team _playerTeam = null;
        private Team _enemyTeam = null;
        private Team _currentTeam;

        int currentTurn = 0;
        Battlefield battlefield = null;
        int numberOfPossibleTiles;

        public void Init()
        {
            SetupCharacters(() => { GetBattlefieldSize(StartGame); });
        }

        private void SetupCharacters(Action callback)
        {
            int charactersAmount;
            bool validInput;
            do
            {
                Console.WriteLine("How many character do you want on each team?");
                string input = Console.ReadLine();

                validInput = int.TryParse(input, out charactersAmount) && charactersAmount > 0 &&
                             charactersAmount <= 5;

                if (!validInput)
                {
                    Program.ClearConsoleLines(2);
                }
            } while (!validInput);

            _playerTeam = CreateTeam("Player", charactersAmount, ConsoleColor.Blue);
            Console.Write(Environment.NewLine);
            _enemyTeam = CreateTeam("Enemy", charactersAmount, ConsoleColor.Red, true);
            Console.Write(Environment.NewLine);
            
            callback?.Invoke();
        }

        private Team CreateTeam(string name, int charactersAmount, ConsoleColor color, bool randomClasses = false)
        {
            Team newTeam = new Team(name, color);
            for (int i = 0; i < charactersAmount; i++)
            {
                Character newCharacter = CreateCharacter(i, newTeam, randomClasses);
                newTeam.characters.Add(newCharacter);
            }

            return newTeam;
        }

        private Character CreateCharacter(int index, Team team, bool randomClass)
        {
            int characterClass;
            if (!randomClass)
            {
                bool validInput;
                do
                {
                    Console.WriteLine("Choose Between One of this Classes:");
                    Console.WriteLine("[1] Paladin, [2] Warrior, [3] Cleric, [4] Archer");
                    string choiceString = Console.ReadLine();

                    validInput = int.TryParse(choiceString, out characterClass) && characterClass > 0 &&
                                 characterClass <= 4;

                    if (!validInput)
                    {
                        Program.ClearConsoleLines(3);
                    }
                    else
                    {
                        Program.ClearConsoleLines(1);
                    }
                } while (!validInput);
            }
            else
            {
                var rand = new Random();
                characterClass = rand.Next(1, 5);
            }

            Character newCharacter;
            switch ((Types.CharacterClass)characterClass)
            {
                case Types.CharacterClass.Paladin:
                    newCharacter = new Paladin(index, team);
                    break;
                case Types.CharacterClass.Warrior:
                    newCharacter = new Warrior(index, team);
                    break;
                case Types.CharacterClass.Cleric:
                    newCharacter = new Cleric(index, team);
                    break;
                case Types.CharacterClass.Archer:
                    newCharacter = new Archer(index, team);
                    break;
                default:
                    newCharacter = new Warrior(index, team);
                    break;
            }

            Console.ForegroundColor = team.Color;
            Console.Write("Character");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($" class choice: {((Types.CharacterClass)characterClass).ToString()}");
            Console.Write(Environment.NewLine);

            return newCharacter;
        }

        private void AllocatePlayer(Action callback)
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

        private void GetBattlefieldSize(Action callback)
        {
            int columns = 0;
            int lines = 0;

            bool validValue;

            bool bigEnough;
            do
            {
                do
                {
                    Console.WriteLine("How many columns should the battlefield have?");
                    string columnsString = Console.ReadLine();

                    validValue = int.TryParse(columnsString, out columns) && columns > 1;
                    if (!validValue)
                    {
                        Program.ClearConsoleLines(2);
                    }
                } while (!validValue);

                do
                {
                    Console.WriteLine("And how many lines should the battlefield have?");
                    string linesString = Console.ReadLine();

                    validValue = int.TryParse(linesString, out lines) && lines > 1;
                    if (!validValue)
                    {
                        Program.ClearConsoleLines(2);
                    }
                } while (!validValue);

                bigEnough = columns * lines >= _playerTeam.characters.Count + _enemyTeam.characters.Count;

                if (!bigEnough)
                {
                    Console.Write(Environment.NewLine);
                    Console.WriteLine("Set a battlefield size large enough to fit all characters.");
                    Console.ReadKey();
                    
                    Program.ClearConsoleLines(6);
                }
            } while (!bigEnough);

            battlefield = new Battlefield(lines, columns);
            numberOfPossibleTiles = battlefield.SizeX * battlefield.SizeY;

            callback?.Invoke();
        }

        private void AllocateEnemy(Action callback)
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

        private void StartGame()
        {
            AllocatePlayer(() => { AllocateEnemy(() => { StartTurn(); }); });
        }

        private void StartTurn()
        {
            Console.Clear();
            Console.Clear();

            if (currentTurn == 0)
            {
                Random rand = new Random();
                int x = rand.Next(0, 2);
                _currentTeam = x == 0 ? _playerTeam : _enemyTeam;
            }
            else
            {
                _currentTeam = _currentTeam == _playerTeam ? _enemyTeam : _playerTeam;
                _currentTeam.StartTurn(battlefield);
            }

            currentTurn++;
            HandleTurn();
        }

        private void HandleTurn()
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
                        Program.ClearConsoleLines(4);
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
            Console.ReadKey();
        }

        private Team GetWinner()
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

        private int GetRandomInt(int min, int max)
        {
            var rand = new Random();
            return rand.Next(min, max);
        }

        private (int, int) GetRandomPositionAtGrid()
        {
            return (GetRandomInt(0, battlefield.SizeX), GetRandomInt(0, battlefield.SizeY));
        }
    }
}