using UnityEngine;
using DungeonCrawl.Core;
using DungeonCrawl.Actors.Static;
using System.Collections.Generic;
using System;

namespace DungeonCrawl.Actors.Characters
{
	public class PigBoss : Character
	{
		private const int HEALTH = 42;
		private const int DAMAGE = 10;
		private float HIT = 0;

		private const int ATTACK_START = 900;
		private const int ATTACK_WINDUP = 15;
		private const int ATTACK_LENGTH = 25;
		private const int ATTACK_RECOVERY = 5;

		private List<Fire> _fireEffects;
		private List<Lava> _lavaEffects;

		public PigBoss() : base(HEALTH, DAMAGE)
		{
			_fireEffects = new List<Fire>();
			_lavaEffects = new List<Lava>();
		}
		public override int DefaultSpriteId => 413;
		public override string DefaultName => "PigBoss";
		protected override void OnDeath()
		{
			Debug.Log("Pig SLAYED");
			Destroy(this);
		}


		protected override void OnUpdate(float deltaTime)
		{
			if (HIT < ATTACK_START && HIT%100 == 2)
			{
				TryMove(Direction.Right);
			}
			else
			{
				//CastFlames()
				List<(int x, int y)> firePositions = GetAdjPositions();
				if (HIT == ATTACK_START)
				{
					SpawnLavaEffect(firePositions);
				}
				else if (HIT == ATTACK_START + ATTACK_WINDUP)
				{
					SpawnEffect(firePositions);
				}
				else if (HIT == ATTACK_START + ATTACK_WINDUP + ATTACK_LENGTH)
				{
					RemoveFireEffect();
				}
				else if (HIT == ATTACK_START + ATTACK_WINDUP + ATTACK_LENGTH + ATTACK_RECOVERY)
				{
					RemoveLavaEffect();
					HIT = 0;
				}
				//CastFlames()
			}
			HIT++;
		}

		private void RemoveLavaEffect()
		{
			for (int i = 0; i < _lavaEffects.Count; i++)
			{
				ActorManager.Singleton.DestroyActor(_lavaEffects[i]);
				_lavaEffects[i] = null;
			}
			_lavaEffects = new List<Lava>();
		}

		private void RemoveFireEffect()
		{
			for (int i = 0; i < _fireEffects.Count; i++)
			{
				ActorManager.Singleton.DestroyActor(_fireEffects[i]);
				_fireEffects[i].PutOut();
				_fireEffects[i] = null;
			}
			_fireEffects = new List<Fire>();
		}

		private void SpawnLavaEffect(List<(int x, int y)> firePositions)
		{
			Lava lava;
			for (int i = 0; i < firePositions.Count; i++)
			{
				lava = ActorManager.Singleton.Spawn<Lava>(firePositions[i]);
				_lavaEffects.Add(lava);
			}
		}

		private void SpawnEffect(List<(int x, int y)> firePositions)
		{
			Fire effect;
			for (int i = 0; i < firePositions.Count; i++)
			{
				effect = ActorManager.Singleton.Spawn<Fire>(firePositions[i]);
				_fireEffects.Add(effect);
			}
		}


		private void SpawnEffect<T>(List<(int x, int y)> firePositions) where T : Actor
		{
			T effect;
			for (int i = 0; i < firePositions.Count; i++)
			{
				effect = ActorManager.Singleton.Spawn<T>(firePositions[i]);
				//_effects.Add(effect);
			}
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

	}
}
