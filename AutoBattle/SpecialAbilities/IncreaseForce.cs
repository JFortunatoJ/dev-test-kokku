namespace AutoBattle.SpecialAbilities
{
    public class IncreaseStrength : SpecialAbility
    {
        public override void ActivateAbility(Character target)
        {
            target.DamageMultiplier = 2;
        }
    }
}
