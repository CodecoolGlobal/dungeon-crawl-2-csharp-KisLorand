using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawl.Actors.Items
{
    public class Stick : Item
    {
        public override int DefaultSpriteId => 177;
        public override string DefaultName => "Stick";

        public override bool Detectable => false;

    }
}
