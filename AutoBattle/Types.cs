namespace AutoBattle
{
    public class Types
    {
        public struct CharacterClassSpecific
        {
            public CharacterClass CharacterClass;
            public float hpModifier;
            public float classDamage;
            public CharacterSkills[] skills;
        }

        public struct CharacterSkills
        {
            public string name;
            public float damage;
            public float damageMultiplier;
        }

        public enum CharacterClass : uint
        {
            Paladin = 1,
            Warrior = 2,
            Cleric = 3,
            Archer = 4
        }
    }
}