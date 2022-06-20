using DungeonCrawl.Actors.Characters;
using DungeonCrawl.Core;
using UnityEngine;

namespace DungeonCrawl.Actors.Characters
{
	public class PigBeast : Character
	{
		private Direction _direction;
		public PigBeast((int x, int y) position) : base(27, 7)
		{
			Position = position;
			DecideInitDirection();
		}

		private void DecideInitDirection()
		{
			(int x, int y) playerPos = ActorManager.Singleton.GetPlayer().Position;

			if (playerPos.x == this.Position.x)
			{
				if (playerPos.y < this.Position.y)
					_direction = Direction.Up;
				else
					_direction = Direction.Down;
			}
			else if (playerPos.y == this.Position.y)
			{
				if (playerPos.x < this.Position.x)
					_direction = Direction.Right;
				else
					_direction = Direction.Left;
			}

		}

		protected override void OnUpdate(float deltaTime)
		{
			DecideInitDirection();
			TryMove(_direction);
		}

		protected override void OnDeath()
		{
			Debug.Log("....");
		}

		public override int DefaultSpriteId => 413;

		public override string DefaultName => "PigBeast";
	}
}
