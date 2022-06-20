using UnityEngine;
using DungeonCrawl.Core;

namespace DungeonCrawl.Actors.Characters
{
	public class Pig : Character
	{
		private bool _isDead = false;
		private int _dangerdistance = 3;
		public Pig() : base(10,0)
		{
		}

		protected override void OnUpdate(float deltaTime)
		{
			(int x, int y) playerPos = ActorManager.Singleton.GetPlayer().Position;
			var dx = Mathf.Abs(playerPos.x - this.Position.x);
			var dy = Mathf.Abs(playerPos.y - this.Position.y);
			if (dx < _dangerdistance && dy < _dangerdistance)
			{
				this.ApplyDamage(Health);
			}
		}
		public override bool OnCollision(Actor anotherActor)
		{
			this.ApplyDamage(Health);
			anotherActor.OnCollision(this);
			return true;
		}

		public bool IsPigDead()
		{ 
			if (_isDead)
				return true;
			return false;
		}

		protected override void OnDeath()
		{
			_isDead = true;
			Debug.Log("Pig death noises...");
			ActorManager.Singleton.Spawn<PigBeast>(this.Position);
			Destroy(this);
		}

		public override int DefaultSpriteId => 364;

		public override string DefaultName => "Pig";
	}
}
