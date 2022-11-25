using System;
using System.Collections.Generic;

namespace AutoBattle
{
    public class Team
    {
        public string Name { get; private set; }
        public ConsoleColor Color { get; private set; }
        public List<Character> characters;

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
        }

        public void StartTurn(Battlefield battlefield)
        {
            foreach (var character in characters)
            {
                character.StartTurn(battlefield);
            }
        }
    }
}