using DungeonCrawl.Core;

namespace DungeonCrawl.Actors.Static
{
    internal class Blood : Actor
    {
        public override int DefaultSpriteId => Utilities.GetRandomNumberWithinRange(0, 2);
        public override string DefaultName => "Blood";
        public override bool Detectable => false;
    }
}