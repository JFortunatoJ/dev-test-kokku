using System;
using System.Collections.Generic;

namespace AutoBattle
{
    public class Team
    {
        public string Name { get; private set; }
        public ConsoleColor Color { get; private set; }
        public List<Character> characters;

        public int CurrentCharacterIndex
        {
            get => _currentCharacterIndex;
            private set => _currentCharacterIndex = value % characters.Count;
        }

        private int _currentCharacterIndex;

        public bool AllDead
        {
            get
            {
                foreach (var character in characters)
                {
                    if (!character.IsDead)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public Team(string name, ConsoleColor color)
        {
            Name = name;
            Color = color;
            characters = new List<Character>();
            _currentCharacterIndex = 0;
        }

        public void StartTurn(Battlefield battlefield)
        {
            characters[CurrentCharacterIndex].StartTurn(battlefield);
            CurrentCharacterIndex++;
        }
    }
}