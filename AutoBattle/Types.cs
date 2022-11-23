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

        public struct GridBox
        {
            public int xIndex;
            public int yIndex;
            public bool ocupied;
            public int index;

            public GridBox(int x, int y, bool ocupied, int index)
            {
                xIndex = x;
                yIndex = y;
                this.ocupied = ocupied;
                this.index = index;
            }
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