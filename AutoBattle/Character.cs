using System;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Character
    {
        public string Name { get; set; }
        public float health;
        public float baseDamage;
        public float DamageMultiplier { get; set; }
        public GridBox currentBox;
        public int playerIndex;
        public Character Target { get; set; }

        public Character(CharacterClass characterClass)
        {
        }


        public bool TakeDamage(float amount)
        {
            if ((health -= baseDamage) <= 0)
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
                if (this.currentBox.xIndex > Target.currentBox.xIndex)
                {
                    if ((battlefield.grids.Exists(x => x.index == currentBox.index - 1)))
                    {
                        currentBox.ocupied = false;
                        battlefield.grids[currentBox.index] = currentBox;
                        currentBox = (battlefield.grids.Find(x => x.index == currentBox.index - 1));
                        currentBox.ocupied = true;
                        battlefield.grids[currentBox.index] = currentBox;
                        Console.WriteLine($"Player {playerIndex} walked left\n");
                        battlefield.DrawBattlefield(5, 5);

                        return;
                    }
                }
                else if (currentBox.xIndex < Target.currentBox.xIndex)
                {
                    currentBox.ocupied = false;
                    battlefield.grids[currentBox.index] = currentBox;
                    currentBox = (battlefield.grids.Find(x => x.index == currentBox.index + 1));
                    currentBox.ocupied = true;
                    return;
                    battlefield.grids[currentBox.index] = currentBox;
                    Console.WriteLine($"Player {playerIndex} walked right\n");
                    battlefield.DrawBattlefield(5, 5);
                }

                if (this.currentBox.yIndex > Target.currentBox.yIndex)
                {
                    battlefield.DrawBattlefield(5, 5);
                    this.currentBox.ocupied = false;
                    battlefield.grids[currentBox.index] = currentBox;
                    this.currentBox = (battlefield.grids.Find(x => x.index == currentBox.index - battlefield.XLength));
                    this.currentBox.ocupied = true;
                    battlefield.grids[currentBox.index] = currentBox;
                    Console.WriteLine($"Player {playerIndex} walked up\n");
                    return;
                }
                else if (this.currentBox.yIndex < Target.currentBox.yIndex)
                {
                    this.currentBox.ocupied = true;
                    battlefield.grids[currentBox.index] = this.currentBox;
                    this.currentBox = (battlefield.grids.Find(x => x.index == currentBox.index + battlefield.XLength));
                    this.currentBox.ocupied = false;
                    battlefield.grids[currentBox.index] = currentBox;
                    Console.WriteLine($"Player {playerIndex} walked down\n");
                    battlefield.DrawBattlefield(5, 5);

                    return;
                }
            }
        }

        // Check in x and y directions if there is any character close enough to be a target.
        private bool CheckCloseTargets(Grid battlefield)
        {
            bool left = (battlefield.grids.Find(x => x.index == currentBox.index - 1).ocupied);
            bool right = (battlefield.grids.Find(x => x.index == currentBox.index + 1).ocupied);
            bool up = (battlefield.grids.Find(x => x.index == currentBox.index + battlefield.XLength).ocupied);
            bool down = (battlefield.grids.Find(x => x.index == currentBox.index - battlefield.XLength).ocupied);

            if (left & right & up & down)
            {
                return true;
            }

            return false;
        }

        public void Attack(Character target)
        {
            var rand = new Random();
            target.TakeDamage(rand.Next(0, (int)baseDamage));
            Console.WriteLine(
                $"Player {playerIndex} is attacking the player {Target.playerIndex} and did {baseDamage} damage\n");
        }
    }
}