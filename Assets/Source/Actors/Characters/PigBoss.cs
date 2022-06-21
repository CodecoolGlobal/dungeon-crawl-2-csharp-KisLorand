using UnityEngine;
using DungeonCrawl.Core;
using DungeonCrawl.Actors.Static;
using System.Collections.Generic;

namespace DungeonCrawl.Actors.Characters
{
	public class PigBoss : Character
	{
		private const int HEALTH = 42;
		private const int DAMAGE = 10;
		private float HIT = 0;
		private List<Actor> _effects;

		public PigBoss() : base(HEALTH, DAMAGE)
		{
			_effects = new List<Actor>(9);
		}

		protected override void OnUpdate(float deltaTime)
		{
			(int x, int y) playerPos = ActorManager.Singleton.GetPlayer().Position;
			//HIT += deltaTime;

			(int x, int y) adjPos;
			if (HIT == 8)
			{
				adjPos.x = this.Position.x + 1;
				adjPos.y = this.Position.y + 1;
			}
			else if (HIT == 10)
			{
				adjPos.x = this.Position.x + 1;
				adjPos.y = this.Position.y + 1;
				var fire = ActorManager.Singleton.Spawn<Fire>(adjPos);
				Destroy(fire);
				/*_effects.Add(fire);

				foreach (Actor flame in _effects)
				{
					_effects.Remove(flame);
					Destroy(flame);
				}*/
			}
			else if (HIT == 13)
			{
				foreach (Actor flame in _effects)
					Destroy(flame);
				HIT = 0;
			}
			HIT++;

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
