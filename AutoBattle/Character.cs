using System;
using System.Drawing;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Character
    {
        public int PlayerIndex
        {
            get => _playerIndex;
            private set => _playerIndex = value;
        }
        
        public string Name
        {
            get => _name;
            private set => _name = value;
        }

        public float Health
        {
            get => _health;
            private set => _health = value;
        }

        public float BaseDamage
        {
            get => _baseDamage;
            private set => _baseDamage = value;
        }

        public float DamageMultiplier
        {
            get => _damageMultiplier;
            private set => _damageMultiplier = value;
        }

        public CharacterClass CharacterClass
        {
            get => _characterClass;
            private set => _characterClass = value;
        }

        public ConsoleColor Color
        {
            get;
            private set;
        }

        public Tile CurrentLocation
        {
            get;
            private set;
        }

        private int _playerIndex;
        private string _name;
        private float _health;
        private float _baseDamage;
        private float _damageMultiplier;
        private CharacterClass _characterClass;
        
        public Character Target { get; set; }

        public Character(int playerIndex, string name, CharacterClass characterClass, ConsoleColor color, float health = 100, float baseDamage = 20, float damageMultiplier = 1)
        {
            PlayerIndex = playerIndex;
            Name = name;
            Color = color;
            CharacterClass = characterClass;
            Health = health;
            BaseDamage = baseDamage;
            DamageMultiplier = damageMultiplier;
        }

        public bool TakeDamage(float amount)
        {
            if ((Health -= BaseDamage) <= 0)
            {
                Die();
                return true;
            }

            return false;
        }

        public void Die()
        {
            //TODO >> maybe kill him?
        }

        public void WalkTo(Tile tile)
        {
            if (CurrentLocation != null)
            {
                CurrentLocation.character = null;
            }

            CurrentLocation = tile;
            CurrentLocation.character = this;
        }

        public void StartTurn(Battlefield battlefield)
        {
            if (CheckCloseTargets(battlefield))
            {
                Attack(Target);
                return;
            }
            else
            {
                /*
                // if there is no target close enough, calculates in wich direction this character should move to be closer to a possible target
                if (this.CurrentBox.lineIndex > Target.CurrentBox.lineIndex)
                {
                    if ((battlefield.grid.Exists(x => x.index == CurrentBox.index - 1)))
                    {
                        CurrentBox.isOcupied = false;
                        battlefield.grid[CurrentBox.index] = CurrentBox;
                        CurrentBox = (battlefield.grid.Find(x => x.index == CurrentBox.index - 1));
                        CurrentBox.isOcupied = true;
                        battlefield.grid[CurrentBox.index] = CurrentBox;
                        Console.WriteLine($"Player {PlayerIndex} walked left\n");
                        battlefield.DrawBattlefield(5, 5);

                        return;
                    }
                }
                else if (CurrentBox.lineIndex < Target.CurrentBox.lineIndex)
                {
                    CurrentBox.isOcupied = false;
                    battlefield.grid[CurrentBox.index] = CurrentBox;
                    CurrentBox = (battlefield.grid.Find(x => x.index == CurrentBox.index + 1));
                    CurrentBox.isOcupied = true;
                    return;
                    battlefield.grid[CurrentBox.index] = CurrentBox;
                    Console.WriteLine($"Player {PlayerIndex} walked right\n");
                    battlefield.DrawBattlefield(5, 5);
                }

                if (this.CurrentBox.columnIndex > Target.CurrentBox.columnIndex)
                {
                    battlefield.DrawBattlefield(5, 5);
                    this.CurrentBox.isOcupied = false;
                    battlefield.grid[CurrentBox.index] = CurrentBox;
                    this.CurrentBox = (battlefield.grid.Find(x => x.index == CurrentBox.index - battlefield.Lines));
                    this.CurrentBox.isOcupied = true;
                    battlefield.grid[CurrentBox.index] = CurrentBox;
                    Console.WriteLine($"Player {PlayerIndex} walked up\n");
                    return;
                }
                else if (this.CurrentBox.columnIndex < Target.CurrentBox.columnIndex)
                {
                    this.CurrentBox.isOcupied = true;
                    battlefield.grid[CurrentBox.index] = this.CurrentBox;
                    this.CurrentBox = (battlefield.grid.Find(x => x.index == CurrentBox.index + battlefield.Lines));
                    this.CurrentBox.isOcupied = false;
                    battlefield.grid[CurrentBox.index] = CurrentBox;
                    Console.WriteLine($"Player {PlayerIndex} walked down\n");
                    battlefield.DrawBattlefield(5, 5);

                    return;
                }
                */
            }
        }

        // Check in x and y directions if there is any character close enough to be a target.
        private bool CheckCloseTargets(Battlefield battlefield)
        {
            /*
            bool left = battlefield.grid[CurrentLocation.lineIndex, CurrentLocation.columnIndex - 1].isOcupied;
            bool right = (battlefield.grid.Find(x => x.index == CurrentBox.index + 1).ocupied);
            bool up = (battlefield.grid.Find(x => x.index == CurrentBox.index + battlefield.Lines).ocupied);
            bool down = (battlefield.grid.Find(x => x.index == CurrentBox.index - battlefield.Lines).ocupied);
            

            if (left & right & up & down)
            {
                return true;
            }
*/
            return false;
        }

        public void Attack(Character target)
        {
            var rand = new Random();
            target.TakeDamage(rand.Next(0, (int)BaseDamage));
            Console.WriteLine(
                $"Player {PlayerIndex} is attacking the player {Target.PlayerIndex} and did {BaseDamage} damage\n");
        }
    }
}