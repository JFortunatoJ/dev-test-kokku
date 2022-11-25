using System;

namespace AutoBattle
{
    public class Types
    {
        public struct Vector2
        {
            public float x;
            public float y;
            
            public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.x + b.x, a.y + b.y);
            public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.x - b.x, a.y - b.y);
            public static Vector2 operator /(Vector2 a, Vector2 b) => new Vector2(a.x / b.x, a.y / b.y);
            public static Vector2 operator /(Vector2 a, float b) => new Vector2(a.x / b, a.y / b);
            public static Vector2 Distance(Vector2 a, Vector2 b) => new Vector2(Math.Abs(b.x - a.x), Math.Abs(b.y - a.y));
            public float Magnitude => MathF.Sqrt((x * x + y * y));
            
            public void Normalize()
            {
                float num = Magnitude;
                if (num > 9.999999747378752E-06)
                    this /= (int)num;
                else
                    this = new Vector2(0,0);
            }

            public Vector2(float x, float y)
            {
                this.x = x;
                this.y = y;
            }
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