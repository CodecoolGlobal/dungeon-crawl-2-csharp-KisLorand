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
			//HIT += deltaTime;

			(int x, int y) adjPos;
			if (HIT == 30 + 900)
			{
				adjPos.x = this.Position.x + 1;
				adjPos.y = this.Position.y + 1;
			}
			else if (HIT == 40 + 900)
			{
				adjPos.x = this.Position.x + 1;
				adjPos.y = this.Position.y + 1;
				var fire = ActorManager.Singleton.Spawn<Fire>(adjPos);
				_effects.Add(fire);
			}
			else if (HIT == 40 + 900 + 30)
			{
				Debug.Log("Extinguish Thy flames");
				//var x = Object.FindObjectsOfType<Fire>();
				
				for(int i=0; i<_effects.Count; i++)
				{
					ActorManager.Singleton.DestroyActor(_effects[i]);
					_effects.Remove(_effects[i]);
					_effects[i].PutOut();
				}
			}
			else if (HIT == 40 + 900 + 30 + 20)
			{ 
				HIT = 0;
			}
			HIT++;
			Debug.Log("pig  " + HIT + "/n" + "meager flames : " + _effects.Count);

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
