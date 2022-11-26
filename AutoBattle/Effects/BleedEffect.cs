namespace AutoBattle
{
    public class BleedEffect : StatusEffect
    {
        public BleedEffect(string id, int turnsDuration) : base(id, turnsDuration)
        {
        }

        public override void Apply(Character target)
        {
            base.Apply(target);
            target.TakeDamage(1);
        }
    }
}
