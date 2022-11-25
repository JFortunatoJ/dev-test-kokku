namespace AutoBattle
{
    public class BleedEffect : StatusEffect
    {
        public override void Apply(Character target)
        {
            base.Apply(target);
            target.TakeDamage(1);
        }
    }
}
