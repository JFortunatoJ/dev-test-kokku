namespace AutoBattle
{
    public class HealEffect : StatusEffect
    {
        public HealEffect(string id, int turnsDuration) : base(id, turnsDuration)
        {
        }

        public override void Apply(Character target)
        {
            base.Apply(target);
            target.Heal(5);
        }
    }
}
