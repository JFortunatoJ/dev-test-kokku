namespace AutoBattle
{
    public abstract class StatusEffect
    {
        public string id;
        public int turnsDuration;

        protected StatusEffect(){}

        protected StatusEffect(string id, int turnsDuration)
        {
            this.id = id;
            this.turnsDuration = turnsDuration;
        }

        public virtual void Apply(Character target)
        {
            turnsDuration--;
        }
    }
}
