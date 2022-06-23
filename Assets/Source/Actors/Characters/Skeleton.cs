using UnityEngine;
using System;
using DungeonCrawl.Actors.Static;
using DungeonCrawl.Core;
using Random = UnityEngine.Random;

namespace DungeonCrawl.Actors.Characters
{
    public class Skeleton : Character
    {
        private const int _HEALTH = 6;
        private const int _DAMAGE = 2;
        
        

        public Skeleton() : base(_HEALTH, _DAMAGE)
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
            _limit = Random.Range(1f, 6f);
            _timer += deltaTime;
            if (_timer > _limit)
            {
                _direction = Random.Range(0, 3);
                switch (_direction)
                {
                    case 0:
                        TryMove(Direction.Down);
                        break;
                    case 1:
                        TryMove(Direction.Up);
                        break;
                    case 2:
                        TryMove(Direction.Left);
                        break;
                    case 3:
                        TryMove(Direction.Right);
                        break;

                }

                _timer = 0f;
            }
        }

        protected override void OnDeath()
        {
            Debug.Log("Well, I was already dead anyway...");
            ActorManager.Singleton.Spawn<Bonepile>(this.Position);
            _spriteRenderer = GetComponent<SpriteRenderer>();
            ActorManager.Singleton.ColorTile("Bonepile", Color.white);
            SetSprite(DefaultSpriteId);
        }

        public override int DefaultSpriteId => 316;
        public override string DefaultName => "Skeleton";

    
    }
}
