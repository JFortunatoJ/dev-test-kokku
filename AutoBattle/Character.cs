using System;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Character
    {
        public int PlayerIndex
        {
            get => _playerIndex;
            private set => _playerIndex = value;
        }

        public float Health
        {
            get => _health;
            private set => _health = value;
        }

        public float BaseDamage
        {
            get => _baseDamage;
            private set => _baseDamage = value;
        }

        public float DamageMultiplier
        {
            get => _damageMultiplier;
            private set => _damageMultiplier = value;
        }

        public CharacterClass CharacterClass
        {
            get => _characterClass;
            private set => _characterClass = value;
        }

        public Team Team { get; private set; }

        public Tile CurrentTile
        {
            get;
            private set;
        }

        public bool IsDead => Health <= 0;

        private int _playerIndex;
        private float _health;
        private float _baseDamage;
        private float _damageMultiplier;
        private CharacterClass _characterClass;
        private Tile _closestTileWithOpponent;

        public Character(int playerIndex, Team team, CharacterClass characterClass, float health = 100, float baseDamage = 20, float damageMultiplier = 1)
        {
            PlayerIndex = playerIndex;
            Team = team;
            CharacterClass = characterClass;
            Health = health;
            BaseDamage = baseDamage;
            DamageMultiplier = damageMultiplier;
        }

        public void StartTurn(Battlefield battlefield)
        {
            _closestTileWithOpponent = GetClosestTileWithOpponent(battlefield);
            
            if (CanAttack(_closestTileWithOpponent))
            {
                Attack(_closestTileWithOpponent.character);
                return;
            }

            WalkTo(GetTileToMove(battlefield));
        }
        
        public void Attack(Character target)
        {
            target.TakeDamage(_baseDamage * _damageMultiplier);
            Console.WriteLine($"Player {PlayerIndex} is attacking the player {target.PlayerIndex} and did {BaseDamage * _damageMultiplier} damage\n");
        }
        
        public bool TakeDamage(float amount)
        {
            if ((Health -= amount) <= 0)
            {
                Die();
                return true;
            }

            return false;
        }

        public void Die()
        {
            //TODO >> maybe kill him?
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

        private bool CanAttack(Tile targetTile)
        {
            Types.Vector2 distance = Types.Vector2.Distance(CurrentTile.position, targetTile.position);
            if ((distance.x == 1 && CurrentTile.position.y == targetTile.position.y) ||
                (distance.y == 1 && CurrentTile.position.x == targetTile.position.x))
            {
                return true;
            }

            return false;
        }

        private Tile GetClosestTileWithOpponent(Battlefield battlefield)
        {
            float closestTargetDistance = battlefield.TilesAmount;
            Tile closestTileWithOpponent = null;

            for (int i = 0; i < battlefield.SizeX; i++)
            {
                for (int j = 0; j < battlefield.SizeY; j++)
                {
                    Tile tile = battlefield.grid[i, j];
                    if(!tile.IsOccupied || tile.character._playerIndex == PlayerIndex)
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

        private Tile GetTileToMove(Battlefield battlefield)
        {
            //TODO: Maybe A* Pathfinding?
            
            Types.Vector2 tilePosition = CurrentTile.position;
            Types.Vector2 direction =  _closestTileWithOpponent.position - CurrentTile.position;

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