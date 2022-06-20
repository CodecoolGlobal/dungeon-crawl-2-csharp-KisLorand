using Assets.Source.Actors.Static;
using DungeonCrawl.Actors.Characters;
using DungeonCrawl.Actors.Static;
using DungeonCrawl.Core;
using UnityEngine;

namespace DungeonCrawl.Actors.Static
{
    public class Wall : Actor
    {
		public override bool OnCollision(Actor anotherActor)
		{
			if (anotherActor is Character)
			{
				Character character = (Character)anotherActor;
				if (character is PigBeast)
				{
					ActorManager.Singleton.DestroyActor(anotherActor);
					ActorManager.Singleton.Spawn<Portal>(this.Position);
					ActorManager.Singleton.DestroyActor(this);
				}
			}
			return true;
		}
		public override int DefaultSpriteId => 825;
        public override string DefaultName => "Wall";
    }
}
