using System;
using System.Collections.Generic;
using static AutoBattle.Types;

namespace AutoBattle
{
    public abstract class Character
    {
        public int PlayerIndex { get; set; }
        public virtual string Name
        {
            get => Health.ToString();
        }
        public float Health
        {
            get => _health;
            private set
            {
                _health = Math.Clamp(value, 0, _maxHealth);
            }
        }
        public float BaseDamage { get; private set; }
        public float DamageMultiplier { get; set; }
        public Team Team { get; set; }
        public Tile CurrentTile { get; set; }
        public bool IsDead => Health <= 0;
        public bool IsStunned { get; set; }

        private float _health;
        private Tile _closestTileWithOpponent;
        private float _maxHealth;
        private List<StatusEffect> _effects;

        public Character() {}

        protected Character(int playerIndex, Team team, float maxHealth, float baseDamage, float damageMultiplier)
        {
            PlayerIndex = playerIndex;
            Team = team;
            Health = _maxHealth = maxHealth;
            BaseDamage = baseDamage;
            DamageMultiplier = damageMultiplier;

            _effects = new List<StatusEffect>();
        }

        public void StartTurn(Battlefield battlefield)
        {
            ApplyEffects();

            if (IsDead || IsStunned) return;

            _closestTileWithOpponent = GetClosestTileWithOpponent(battlefield);

            if (CanAttack(_closestTileWithOpponent))
            {
                // 70% -> Normal Attack
                // 30% -> Effect
                Random rand = new Random();
                if (rand.Next(0, 10) <= 7)
                {
                    Attack(_closestTileWithOpponent.character);
                }
                else
                {
                    StatusEffect(_closestTileWithOpponent.character);
                }

                return;
            }

            WalkTo(GetTileToMove(battlefield));
        }

        private void Attack(Character target)
        {
            target.TakeDamage(BaseDamage * DamageMultiplier);
            
            Console.ForegroundColor = Team.Color;
            Console.Write($"{Name}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" attacked the ");
            Console.ForegroundColor = target.Team.Color;
            Console.Write($"{target.Name}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" of team ");
            Console.Write($"and did {BaseDamage * DamageMultiplier} damage.\n");
        }

        public void TakeDamage(float amount)
        {
            if (IsDead) return;

            Health -= amount;
        }

        public void Heal(float amount)
        {
            if (IsDead) return;

            Health += amount;
        }

        public void AddEffect(StatusEffect effect)
        {
            //Prevents the effect from being applied again
            if (_effects.Find(statusEffect => statusEffect.id.Equals(effect.id)) != null)
            {
                return;
            }

            _effects.Add(effect);
        }

        protected void ApplyEffects()
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].Apply(this);

                if (_effects[i].turnsDuration <= 0)
                {
                    _effects.Remove(_effects[i]);
                    i--;
                }
            }
        }

        public void WalkTo(Tile tile)
        {
            if (CurrentTile != null)
            {
                CurrentTile.character = null;
            }

            CurrentTile = tile;
            CurrentTile.character = this;
            
            Console.ForegroundColor = Team.Color;
            Console.Write($"{Name}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($" has moved to tile [{tile.position.y},{tile.position.x}]\n");
        }

        public abstract void StatusEffect(Character target);

        public abstract void SpecialAbility();

        protected bool CanAttack(Tile targetTile)
        {
            Vector2 distance = Vector2.Distance(CurrentTile.position, targetTile.position);
            if (((distance.x == 1 && CurrentTile.position.y == targetTile.position.y) ||
                    (distance.y == 1 && CurrentTile.position.x == targetTile.position.x)) && !targetTile.character.IsDead)
            {
                return true;
            }

            return false;
        }

        protected Tile GetClosestTileWithOpponent(Battlefield battlefield)
        {
            float closestTargetDistance = battlefield.TilesAmount;
            Tile closestTileWithOpponent = null;

            for (int i = 0; i < battlefield.SizeX; i++)
            {
                for (int j = 0; j < battlefield.SizeY; j++)
                {
                    Tile tile = battlefield.grid[i, j];
                    if (!tile.IsOccupied || tile.character.Team.Name == Team.Name || tile.character.IsDead)
                        continue;

                    float distance = Vector2.Distance(CurrentTile.position, tile.position).Magnitude;
                    if (distance < closestTargetDistance)
                    {
                        closestTargetDistance = distance;
                        closestTileWithOpponent = tile;
                    }
                }
            }

            return closestTileWithOpponent;
        }

        protected Tile GetTileToMove(Battlefield battlefield)
        {
            //TODO: Maybe A* Pathfinding?

            Vector2 tilePosition = CurrentTile.position;
            Vector2 direction = _closestTileWithOpponent.position - CurrentTile.position;

            direction.Normalize();

            if (direction.x > 0)
            {
                int targetX = (int)(CurrentTile.position.x + direction.x);
                if (targetX < battlefield.SizeX && !battlefield.grid[targetX, (int)CurrentTile.position.y].IsOccupied)
                {
                    tilePosition.x = targetX;
                    return battlefield.grid[(int)tilePosition.x, (int)tilePosition.y];
                }
            }

            if (direction.x < 0)
            {
                int targetX = (int)(CurrentTile.position.x + direction.x);
                if (targetX >= 0 && !battlefield.grid[targetX, (int)CurrentTile.position.y].IsOccupied)
                {
                    tilePosition.x = targetX;
                    return battlefield.grid[(int)tilePosition.x, (int)tilePosition.y];
                }
            }

            if (direction.y > 0)
            {
                int targetY = (int)(CurrentTile.position.y + direction.y);
                if (targetY < battlefield.SizeY && !battlefield.grid[(int)CurrentTile.position.x, targetY].IsOccupied)
                {
                    tilePosition.y = targetY;
                    return battlefield.grid[(int)tilePosition.x, (int)tilePosition.y];
                }
            }

            if (direction.y < 0)
            {
                int targetY = (int)(CurrentTile.position.y + direction.y);
                if (targetY >= 0 && !battlefield.grid[(int)CurrentTile.position.x, targetY].IsOccupied)
                {
                    tilePosition.y = targetY;
                    return battlefield.grid[(int)tilePosition.x, (int)tilePosition.y];
                }
            }

            return CurrentTile;
        }
    }
}
