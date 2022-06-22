using UnityEngine;
using DungeonCrawl.Core;
using Assets.Source.Actors.Static;
using DungeonCrawl.Actors.Characters;
using DungeonCrawl.Actors.Static;

namespace DungeonCrawl.Actors.Static
{
	public class Fire : Actor
    {
        private int _damage = 3;
        private int HIT_COUNTER = 0;
        private const int HIT_LIMIT = 10;
        public override int DefaultSpriteId => 494;
        public override string DefaultName => "Fire";

        public override bool Detectable => false;

        public override bool OnCollision(Actor anotherActor)
        {
            if (anotherActor is Character)
            {
                Character anotherCharacter = (Character)anotherActor;
                if (anotherCharacter is not Player)
                {
                    DoDamage(anotherCharacter);
                }
            }
            return true;
        }

        private void DoDamage(Character anotherCharacter)
        {
            anotherCharacter.ApplyDamage(_damage);
        }

        protected override void OnUpdate(float deltaTime)
        {
            HIT_COUNTER++;
            if (HIT_COUNTER >= HIT_LIMIT)
            {
                (int x, int y) playerPos = ActorManager.Singleton.GetPlayer().Position;
                if (playerPos.x == this.Position.x && playerPos.y == this.Position.y)
                {
                    DoDamage(ActorManager.Singleton.GetPlayer());
                }
                HIT_COUNTER = 0;
            }
        }

        public void PutOut()
        {
            ActorManager.Singleton.DestroyActor(this);
        }
    }
}
