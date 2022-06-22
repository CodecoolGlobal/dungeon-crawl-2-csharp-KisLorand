using System;

namespace DungeonCrawl.Actors.Static
{
    public class Floor : Actor
    {
        public override int DefaultSpriteId => GetFloorID();
        public override string DefaultName => "Floor";

        public override bool Detectable => false;

        private int GetFloorID()
        {
            Random random = new Random();
            return random.Next(4, 6);
        }
    }
}
