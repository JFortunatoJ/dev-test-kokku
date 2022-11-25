using System;
using System.Collections.Generic;
using AutoBattle.SpecialAbilities;
using static AutoBattle.Types;

namespace AutoBattle
{
    public abstract class Character
    {
        public int PlayerIndex { get; set; }

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

        public CharacterClass CharacterClass { get; set; }

        public Team Team { get; private set; }

        public Tile CurrentTile { get; set; }

        public bool IsDead => Health <= 0;

        public bool IsStunned { get; set; }

        private float _health;
        private Tile _closestTileWithOpponent;
        private float _maxHealth;

        public Character() {}

        public Character(int playerIndex, Team team, CharacterClass characterClass, float maxHealth = 100, float baseDamage = 20, float damageMultiplier = 1)
        {
            PlayerIndex = playerIndex;
            Team = team;
            CharacterClass = characterClass;
            Health = _maxHealth = maxHealth;
            BaseDamage = baseDamage;
            DamageMultiplier = damageMultiplier;

            //_effects = new List<StatusEffect>();
        }

        public void StartTurn(Battlefield battlefield)
        {
            ApplyEffects();

            if (IsDead || IsStunned) return;

            _closestTileWithOpponent = GetClosestTileWithOpponent(battlefield);

            if (CanAttack(_closestTileWithOpponent))
            {
                Random rand = new Random();
                if (rand.Next(0, 10) <= 7)
                {
                    Attack(_closestTileWithOpponent.character);
                }
                else
                {
                    _closestTileWithOpponent.character.AddEffect(new KnockDownEffect());
                }

                return;
            }

            WalkTo(GetTileToMove(battlefield));
        }

        private void Attack(Character target)
        {
            target.TakeDamage(BaseDamage * DamageMultiplier);
            Console.WriteLine($"Player {PlayerIndex} is attacking the player {target.PlayerIndex} and did {BaseDamage * DamageMultiplier} damage\n");
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
            /*
            //Prevents the effect from being applied again
            if (_effects.Find(statusEffect => statusEffect.id.Equals(effect.id)) != null)
            {
                return;
            }

            _effects.Add(effect);
            */
        }

        protected void ApplyEffects()
        {
            /*
            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].Apply(this);

                if (_effects[i].turnsDuration <= 0)
                {
                    _effects.Remove(_effects[i]);
                    i--;
                }
            }
            */
        }

        public void WalkTo(Tile tile)
        {
            if (CurrentTile != null)
            {
                CurrentTile.character = null;
            }

            CurrentTile = tile;
            CurrentTile.character = this;
        }

        public abstract void StatusEffect(Character target);

        public abstract void SpecialAbility();

        protected bool CanAttack(Tile targetTile)
        {
            Types.Vector2 distance = Types.Vector2.Distance(CurrentTile.position, targetTile.position);
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

                    float distance = Types.Vector2.Distance(CurrentTile.position, tile.position).Magnitude;
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

            Types.Vector2 tilePosition = CurrentTile.position;
            Types.Vector2 direction = _closestTileWithOpponent.position - CurrentTile.position;

            direction.Normalize();
            Console.WriteLine($"X:{direction.x}|Y:{direction.y}");

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
