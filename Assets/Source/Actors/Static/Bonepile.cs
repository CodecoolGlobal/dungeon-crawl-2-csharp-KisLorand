using DungeonCrawl.Core;

namespace DungeonCrawl.Actors.Static
{
    internal class Bonepile : Actor
    {
        public override int DefaultSpriteId => 66;
        public override string DefaultName => "Bonepile";
        public override bool Detectable => false;
    }
}