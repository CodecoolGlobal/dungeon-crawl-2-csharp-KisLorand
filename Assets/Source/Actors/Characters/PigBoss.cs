using UnityEngine;
using DungeonCrawl.Core;

namespace DungeonCrawl.Actors.Characters
{
	public class PigBoss : Character
	{
		private const int HEALTH = 42;
		private const int DAMAGE = 10;

		public PigBoss() : base(HEALTH, DAMAGE)
		{
		}

		protected override void OnUpdate(float deltaTime)
		{
		}

		protected override void OnDeath()
		{
			Debug.Log("Pig SLAYED");
			ActorManager.Singleton.Spawn<PigBeast>(this.Position);
			Destroy(this);
		}

		public override int DefaultSpriteId => 413;

		public override string DefaultName => "PigBoss";
	}
}
