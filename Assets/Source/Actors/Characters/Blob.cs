using DungeonCrawl.Actors.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using DungeonCrawl.Core;
using DungeonCrawl.Actors.Static;
using DungeonCrawl.Actors;

namespace Assets.Source.Actors.Characters
{
    public class Blob : Character
    {

        private const int _health = 10;
        private const int _damage = 1;
        public Blob() : base(_health, _damage)
        {

        }
        public override int DefaultSpriteId => 315;

        public override string DefaultName => "Blob";

        protected override void OnDeath()
        {
            Debug.Log("Well, I was already blobblib anyway...");

            ActorManager.Singleton.Spawn<Blood>(this.Position);
            _spriteRenderer = GetComponent<SpriteRenderer>();
            ActorManager.Singleton.ColorTile("Blood", Color.red);
            SetSprite(DefaultSpriteId);
        }

        public override bool OnCollision(Actor anotherActor)
        {
            anotherActor.OnCollision(this);
            return true;
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (Input.anyKeyDown)

            {
               MoveToPlayer();
            }
        }

        private void MoveToPlayer()
        {
            (int x, int y) playerPos = ActorManager.Singleton.GetPlayer().Position;
            (int, int) targetPosition;
            if (Position.x < playerPos.x)
                targetPosition = (Position.x + 1, Position.y);
            else if (Position.x == playerPos.x)
                targetPosition = Position;
            else
                targetPosition = (Position.x - 1, Position.y);
            Collision(targetPosition);

            if (Position.y < playerPos.y)
                targetPosition = (Position.x, Position.y + 1);
            else if (Position.y == playerPos.y)
                targetPosition = Position;
            else
                targetPosition = (Position.x, Position.y - 1);
            Collision(targetPosition);
        }

        private void Collision((int,int) targetPosition)
        {   
            Actor actor = ActorManager.Singleton.GetActorAt(targetPosition);
            if (actor != null && actor.Detectable == true)
            {
                Position = Position;
            }
            else Position = targetPosition;
        }
    }
}
