using System;
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

        public GridBox CurrentBox;

        private int _playerIndex;
        private string _name;
        private float _health;
        private float _baseDamage;
        private float _damageMultiplier;
        private CharacterClass _characterClass;
        
        public Character Target { get; set; }

        public Character(int playerIndex, string name, CharacterClass characterClass, float health = 100, float baseDamage = 20, float damageMultiplier = 1)
        {
            PlayerIndex = playerIndex;
            Name = name;
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

        public void WalkTo(bool canWalk)
        {
        }

        public void StartTurn(Grid battlefield)
        {
            if (CheckCloseTargets(battlefield))
            {
                Attack(Target);
                return;
            }
            else
            {
                // if there is no target close enough, calculates in wich direction this character should move to be closer to a possible target
                if (this.CurrentBox.xIndex > Target.CurrentBox.xIndex)
                {
                    if ((battlefield.grids.Exists(x => x.index == CurrentBox.index - 1)))
                    {
                        CurrentBox.ocupied = false;
                        battlefield.grids[CurrentBox.index] = CurrentBox;
                        CurrentBox = (battlefield.grids.Find(x => x.index == CurrentBox.index - 1));
                        CurrentBox.ocupied = true;
                        battlefield.grids[CurrentBox.index] = CurrentBox;
                        Console.WriteLine($"Player {PlayerIndex} walked left\n");
                        battlefield.DrawBattlefield(5, 5);

                        return;
                    }
                }
                else if (CurrentBox.xIndex < Target.CurrentBox.xIndex)
                {
                    CurrentBox.ocupied = false;
                    battlefield.grids[CurrentBox.index] = CurrentBox;
                    CurrentBox = (battlefield.grids.Find(x => x.index == CurrentBox.index + 1));
                    CurrentBox.ocupied = true;
                    return;
                    battlefield.grids[CurrentBox.index] = CurrentBox;
                    Console.WriteLine($"Player {PlayerIndex} walked right\n");
                    battlefield.DrawBattlefield(5, 5);
                }

                if (this.CurrentBox.yIndex > Target.CurrentBox.yIndex)
                {
                    battlefield.DrawBattlefield(5, 5);
                    this.CurrentBox.ocupied = false;
                    battlefield.grids[CurrentBox.index] = CurrentBox;
                    this.CurrentBox = (battlefield.grids.Find(x => x.index == CurrentBox.index - battlefield.XLength));
                    this.CurrentBox.ocupied = true;
                    battlefield.grids[CurrentBox.index] = CurrentBox;
                    Console.WriteLine($"Player {PlayerIndex} walked up\n");
                    return;
                }
                else if (this.CurrentBox.yIndex < Target.CurrentBox.yIndex)
                {
                    this.CurrentBox.ocupied = true;
                    battlefield.grids[CurrentBox.index] = this.CurrentBox;
                    this.CurrentBox = (battlefield.grids.Find(x => x.index == CurrentBox.index + battlefield.XLength));
                    this.CurrentBox.ocupied = false;
                    battlefield.grids[CurrentBox.index] = CurrentBox;
                    Console.WriteLine($"Player {PlayerIndex} walked down\n");
                    battlefield.DrawBattlefield(5, 5);

                    return;
                }
            }
        }

        // Check in x and y directions if there is any character close enough to be a target.
        private bool CheckCloseTargets(Grid battlefield)
        {
            bool left = (battlefield.grids.Find(x => x.index == CurrentBox.index - 1).ocupied);
            bool right = (battlefield.grids.Find(x => x.index == CurrentBox.index + 1).ocupied);
            bool up = (battlefield.grids.Find(x => x.index == CurrentBox.index + battlefield.XLength).ocupied);
            bool down = (battlefield.grids.Find(x => x.index == CurrentBox.index - battlefield.XLength).ocupied);

            if (left & right & up & down)
            {
                return true;
            }

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