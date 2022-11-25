using System;
using System.Collections.Generic;

namespace AutoBattle
{
    public class Team
    {
        public string Name { get; private set; }
        public ConsoleColor Color { get; private set; }
        public List<Character> characters;
        public Types.CharacterClass Class { get; set; }

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

        public Team(string name, ConsoleColor color, Types.CharacterClass characterClass)
        {
            Name = name;
            Color = color;
            Class = characterClass;
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