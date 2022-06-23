using UnityEngine;
using DungeonCrawl.Core;
using DungeonCrawl.Actors.Static;

namespace DungeonCrawl.Actors.Static
{
	public class BossFog : Actor
	{
		public override int DefaultSpriteId => 5;
		public override string DefaultName => "BossFog";
		public override bool Detectable => true;
		private (int x, int y) bossPosition = (-10,-10);

		protected override void OnUpdate(float deltaTime)
		{
			(int x, int y) playerPos = ActorManager.Singleton.GetPlayer().Position;
			if (playerPos.x - 3 > this.Position.x)
			{
				this.SetSprite(447);
			}
		}
	}
}
