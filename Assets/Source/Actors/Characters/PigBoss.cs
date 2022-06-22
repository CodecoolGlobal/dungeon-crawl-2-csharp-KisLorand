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

		private const int ATTACK_START = 900;
		private const int ATTACK_WINDUP = 5;
		private const int ATTACK_LENGTH = 25;
		private const int ATTACK_RECOVERY = 10;

		private List<Fire> _effects;
		private List<Wall> _walls;

		public PigBoss() : base(HEALTH, DAMAGE)
		{
			_effects = new List<Fire>();
		}

		protected override void OnUpdate(float deltaTime)
		{
			List<(int x, int y)> firePositions = GetAdjPositions();

			if (HIT == ATTACK_START)
			{
				//windup
				Wall wall;
				for (int i = 0; i < firePositions.Count; i++)
				{
					wall = ActorManager.Singleton.Spawn<Wall>(firePositions[i]);
					_walls.Add(wall);
				}
				//windup
			}
			else if (HIT == ATTACK_START + ATTACK_WINDUP)
			{
				//spawnEffect(T effect, List<(int x, int y)> positions)
				Fire fire;
				for (int i = 0; i < firePositions.Count; i++)
				{
					fire = ActorManager.Singleton.Spawn<Fire>(firePositions[i]);
					_effects.Add(fire);
				}
				//spawn fire
			}
			else if (HIT == ATTACK_START + ATTACK_WINDUP + ATTACK_LENGTH)
			{
				for(int i=0; i<_effects.Count; i++)
				{
					ActorManager.Singleton.DestroyActor(_effects[i]);
					_effects[i].PutOut();
					_effects[i] = null;
				}
				_effects = new List<Fire>();
			}
			else if (HIT == ATTACK_START + ATTACK_WINDUP + ATTACK_LENGTH + ATTACK_RECOVERY)
			{
				//recovery
				Wall[] wallCopy = new Wall[8];
				_walls.CopyTo(wallCopy);
				for (int i = 0; i < wallCopy.Length; i++)
				{
					_walls.Remove(_walls[i]);
					ActorManager.Singleton.DestroyActor(_walls[i]);
				}
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
					if ((posX, posY) != this.Position)
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
