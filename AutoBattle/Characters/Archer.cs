using System;

namespace AutoBattle
{
    public class Archer : Character
    {
        public override string Name => "Archer";
        
        public Archer(int playerIndex, Team team, float maxHealth = 10, float baseDamage = 3,
            float damageMultiplier = 1) : base(playerIndex, team, maxHealth, baseDamage, damageMultiplier)
        {
        }

        public override void StatusEffect(Character target)
        {
            BleedEffect bleed = new BleedEffect("bleed", 3);
            target.AddEffect(bleed);
            
            Console.ForegroundColor = Team.Color;
            Console.Write($"{Name}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" apply the effect 'Bleed' on ");
            Console.ForegroundColor = target.Team.Color;
            Console.Write($"{target.Name} ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"for {bleed.turnsDuration} turns.\n");
        }
        public override void SpecialAbility()
        {
            throw new System.NotImplementedException();
        }
    }
}
