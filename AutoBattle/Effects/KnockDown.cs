namespace AutoBattle
{
    public class KnockDownEffect : StatusEffect
    {
        public KnockDownEffect(string id, int turnsDuration) : base(id, turnsDuration)
        {
        }

        public override void Apply(Character target)
        {
            base.Apply(target);
            target.IsStunned = true;
        }
    }
}
