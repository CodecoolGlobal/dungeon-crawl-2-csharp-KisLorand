using UnityEngine;
using DungeonCrawl.Core;
using Assets.Source.Actors.Static;
using DungeonCrawl.Actors.Characters;
using DungeonCrawl.Actors.Static;

namespace DungeonCrawl.Actors.Static
{
	public class Fire : Actor
    {
        public override int DefaultSpriteId => 494;
        public override string DefaultName => "Fire";

        public override bool Detectable => true;

        public override int Z => -1;

        public override bool OnCollision(Actor anotherActor)
        {
            anotherActor.OnCollision(this);
            return true;
        }


    }
}
