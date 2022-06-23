using UnityEngine;
using DungeonCrawl.Core;
using DungeonCrawl.Actors.Static;
using System.Collections.Generic;
using System;
using Assets.Source.Core;

namespace DungeonCrawl.Actors.Characters
{
	public class PigBoss : Character
	{
		private const int HEALTH = 97;
		private const int DAMAGE = 7;
		private float MOVE_COUNTER = 0;

		private const int ATTACK_START = 900;
		private const int ATTACK_WINDUP = 15;
		private const int ATTACK_LENGTH = 25;
		private const int ATTACK_RECOVERY = 5;

		private Direction _pigDirection;

		private List<Fire> _fireEffects;
		private List<Lava> _lavaEffects;

		public PigBoss() : base(HEALTH, DAMAGE)
		{
			_fireEffects = new List<Fire>();
			_lavaEffects = new List<Lava>();
		}
		public override int DefaultSpriteId => 413;
		public override string DefaultName => "PigBoss";


		protected override void OnUpdate(float deltaTime)
		{
			if (MOVE_COUNTER < ATTACK_START && MOVE_COUNTER%45 == 2)
				Move();
			else
				CastFlames();
			MOVE_COUNTER++;
		}

		private void Move()
		{
			(int x, int y) playerPos = ActorManager.Singleton.GetPlayer().Position;

			if (playerPos.x == this.Position.x)
			{
				if (playerPos.y < this.Position.y)
					_pigDirection = Direction.Down;
				else
					_pigDirection = Direction.Up;
			}
			else if (playerPos.y == this.Position.y)
			{
				if (playerPos.x < this.Position.x)
					_pigDirection = Direction.Left;
				else
					_pigDirection = Direction.Right;
			}

			TryMove(_pigDirection);
		}

		private void CastFlames()
		{
			List<(int x, int y)> firePositions = GetAdjPositions();
			if (MOVE_COUNTER == ATTACK_START)
			{
				SpawnLavaEffect(firePositions);
			}
			else if (MOVE_COUNTER == ATTACK_START + ATTACK_WINDUP)
			{
				SpawnFireEffect(firePositions);
			}
			else if (MOVE_COUNTER == ATTACK_START + ATTACK_WINDUP + ATTACK_LENGTH)
			{
				RemoveFireEffect();
			}
			else if (MOVE_COUNTER == ATTACK_START + ATTACK_WINDUP + ATTACK_LENGTH + ATTACK_RECOVERY)
			{
				RemoveLavaEffect();
				MOVE_COUNTER = 0;
			}
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

		private void SpawnFireEffect(List<(int x, int y)> firePositions)
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

		protected override void OnDeath()
		{
			UserInterface.Singleton.SetText($"PIG SLAYED\nPress Esc to quit.", Assets.Source.Core.UserInterface.TextPosition.MiddleCenter);
			
			Debug.Log("PIG SLAYED");
			Destroy(this);
			ActorManager.Singleton.Spawn<Blood>(this.Position);
			_spriteRenderer = GetComponent<SpriteRenderer>();
			ActorManager.Singleton.ColorTile("Blood", Color.red);
			SetSprite(DefaultSpriteId);
		}
	}
}
