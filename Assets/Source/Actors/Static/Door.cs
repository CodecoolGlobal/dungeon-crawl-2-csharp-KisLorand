namespace DungeonCrawl.Actors.Static
{
    public class Door: Actor
    {
        public override int DefaultSpriteId => 441;
        public override string DefaultName => "Door";

        public override bool Detectable => true;

        public override int Z => -1;

        public override bool OnCollision(Actor anotherActor)
        {
            anotherActor.OnCollision(this);
            return true;
        }


    }
}