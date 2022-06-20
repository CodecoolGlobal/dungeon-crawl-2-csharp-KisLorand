using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawl.Actors.Items
{
    public class Potion : Item
    {

        public override int DefaultSpriteId => 656;
        public override string DefaultName => "Potion";

        public override bool Detectable => false;

        protected override void OnPickUp()
        {
        }
    }
}
