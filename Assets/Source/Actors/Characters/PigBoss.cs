﻿using UnityEngine;
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
		private const int ATTACK_WINDUP = 5;
		private const int ATTACK_LENGTH = 25;
		private const int ATTACK_RECOVERY = 10;

		private List<Fire> _fireEffects;
		private List<Wall> _lavaEffects;

		public PigBoss() : base(HEALTH, DAMAGE)
		{
			_fireEffects = new List<Fire>();
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
			List<(int x, int y)> firePositions = GetAdjPositions();

			if (HIT == ATTACK_START)
			{
				//windup
				Wall lava;
				for (int i = 0; i < firePositions.Count; i++)
				{
					lava = ActorManager.Singleton.Spawn<Wall>(firePositions[i]);
					_lavaEffects.Add(lava);
				}
				//windup
			}
			else if (HIT == ATTACK_START + ATTACK_WINDUP)
			{
				//spawnEffect(T effect, List<(int x, int y)> positions)
				SpawnEffect(firePositions);
				//spawn fire
			}
			else if (HIT == ATTACK_START + ATTACK_WINDUP + ATTACK_LENGTH)
			{
				for(int i=0; i< _fireEffects.Count; i++)
				{
					Debug.Log(_fireEffects[i]);
					ActorManager.Singleton.DestroyActor(_fireEffects[i]);
					_fireEffects[i].PutOut();
					_fireEffects[i] = null;
				}
				_fireEffects = new List<Fire>();
			}
			else if (HIT == ATTACK_START + ATTACK_WINDUP + ATTACK_LENGTH + ATTACK_RECOVERY)
			{
				//recovery
				Wall[] wallCopy = new Wall[8];
				_lavaEffects.CopyTo(wallCopy);
				for (int i = 0; i < wallCopy.Length; i++)
				{
					_lavaEffects.Remove(_lavaEffects[i]);
					ActorManager.Singleton.DestroyActor(_lavaEffects[i]);
				}
				//recovery
				HIT = 0;
			}
			HIT++;
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


		/*private void SpawnEffect<T>(List<(int x, int y)> firePositions) where T : Actor
		{
			T effect;
			for (int i = 0; i < firePositions.Count; i++)
			{
				effect = ActorManager.Singleton.Spawn<T>(firePositions[i]);
				_effects.Add(effect);
			}
		}*/

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
