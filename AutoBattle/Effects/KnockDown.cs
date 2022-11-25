namespace AutoBattle
{
    public class KnockDownEffect : StatusEffect
    {
        public override void Apply(Character target)
        {
            base.Apply(target);
            target.IsStunned = true;
        }
    }
}
