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
		private List<Fire> _effects;

		public PigBoss() : base(HEALTH, DAMAGE)
		{
			_effects = new List<Fire>();
		}

		protected override void OnUpdate(float deltaTime)
		{
			(int x, int y) playerPos = ActorManager.Singleton.GetPlayer().Position;

			List<(int x, int y)> firePositions = GetAdjPositions();

			if (HIT == 30 + 900)
			{
				//windup
			}
			else if (HIT == 40 + 900)
			{
				var fire = ActorManager.Singleton.Spawn<Fire>(firePositions[0]);
				_effects.Add(fire);
			}
			else if (HIT == 40 + 900 + 30)
			{
				for(int i=0; i<_effects.Count; i++)
				{
					ActorManager.Singleton.DestroyActor(_effects[i]);
					_effects.Remove(_effects[i]);
					_effects[i].PutOut();
				}
			}
			else if (HIT == 40 + 900 + 30 + 20)
			{ 
				//recovery
				HIT = 0;
			}
			HIT++;
		}

		private List<(int x, int y)> GetAdjPositions()
		{
			List<(int x, int y)> adjPositions = new List<(int x, int y)>();
			List<int> offsetValues = new List<int>() { -1, 0, 1 };
			for (int i = 0; i < offsetValues.Count; i++)
			{
				for (int j = 0; j < offsetValues.Count; j++)
				{
					int posX = this.Position.x + offsetValues[i];
					int posY = this.Position.y + offsetValues[j];
					if (posX != this.Position.x && posY != this.Position.y)
						adjPositions.Add((posX, posY));
				}
			}
			return adjPositions;
		}

		protected override void OnDeath()
		{
			Debug.Log("Pig SLAYED");
			Destroy(this);
		}

		public override int DefaultSpriteId => 413;

		public override string DefaultName => "PigBoss";
	}
}
