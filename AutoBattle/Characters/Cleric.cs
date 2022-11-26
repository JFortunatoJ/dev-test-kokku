using System;

namespace AutoBattle
{
    public class Cleric : Character
    {
        public override string Name => "Cleric";
        
        public Cleric(int playerIndex, Team team, float maxHealth = 8, float baseDamage = 3,
            float damageMultiplier = 1) : base(playerIndex, team, maxHealth, baseDamage, damageMultiplier)
        {
        }

        public override void StatusEffect(Character target)
        {
            HealEffect heal = new HealEffect("heal", 1);
            target.AddEffect(heal);
            
            Console.ForegroundColor = Team.Color;
            Console.Write($"{Name}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" apply the effect 'Heal' on ");
            Console.ForegroundColor = target.Team.Color;
            Console.Write($"{target.Name} ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"for {heal.turnsDuration} turns.\n");
        }
        public override void SpecialAbility()
        {
            throw new System.NotImplementedException();
        }
    }
}
