using UnityEngine;
using DungeonCrawl.Core;
using Assets.Source.Actors.Static;
using DungeonCrawl.Actors.Characters;
using DungeonCrawl.Actors.Static;

namespace DungeonCrawl.Actors.Static
{
    internal class BossDoor : Actor
    {
        public override int DefaultSpriteId => 5;
        public override string DefaultName => "BossDoor";
        public override bool Detectable => false;

		protected override void OnUpdate(float deltaTime)
		{
			(int x, int y) playerPos = ActorManager.Singleton.GetPlayer().Position;
			if (playerPos.x - 3 > this.Position.x)
			{
				(int x, int y) bossPos = (this.Position.x + 7, this.Position.y);
				ActorManager.Singleton.Spawn<BossFog>(this.Position);
				ActorManager.Singleton.Spawn<PigBoss>(bossPos);
				Destroy(this);
			}
		}
	}
}
