using System;

namespace DungeonCrawl.Actors.Static
{
    public class Floor : Actor
    {
        public override int DefaultSpriteId => Utilities.GetRandomNumberWithinRange(4, 6);
        public override string DefaultName => "Floor";

        public override bool Detectable => false;

    }
}
