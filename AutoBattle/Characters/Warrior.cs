using System;

namespace AutoBattle
{
    public class Warrior : Character
    {
        public override string Name => "Warrior";
        
        public Warrior(int playerIndex, Team team, float maxHealth = 20, float baseDamage = 5,
            float damageMultiplier = 1) : base(playerIndex, team, maxHealth, baseDamage, damageMultiplier)
        {
        }

        public override void StatusEffect(Character target)
        {
            KnockDownEffect knockDown = new KnockDownEffect("knockDown", 3);
            target.AddEffect(knockDown);
            
            Console.ForegroundColor = Team.Color;
            Console.Write($"{Name}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" apply the effect 'Knock Down' on ");
            Console.ForegroundColor = target.Team.Color;
            Console.Write($"{target.Name} ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"for {knockDown.turnsDuration} turns.\n");
        }
        public override void SpecialAbility()
        {
            throw new System.NotImplementedException();
        }
    }
}
