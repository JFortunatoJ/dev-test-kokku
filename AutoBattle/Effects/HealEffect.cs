namespace AutoBattle
{
    public class HealEffect : StatusEffect
    {
        public override void Apply(Character target)
        {
            base.Apply(target);
            target.Heal(5);
        }
    }
}
