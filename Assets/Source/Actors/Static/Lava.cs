
namespace DungeonCrawl.Actors.Static
{
    internal class Lava :Actor
    { 
        public override int DefaultSpriteId => 101;
        public override string DefaultName => "Lava";
        public override bool Detectable => false;
    }
}
