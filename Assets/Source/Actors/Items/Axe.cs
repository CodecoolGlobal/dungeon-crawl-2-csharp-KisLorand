using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawl.Actors.Items
{
    public class Axe : Item
    {
        public override int DefaultSpriteId => 425;
        public override string DefaultName => "Axe";

        public override bool Detectable => false;

    }
}
