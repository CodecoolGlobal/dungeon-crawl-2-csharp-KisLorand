using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawl.Actors.Items
{
    public class BeastSlayer : Item
    {
        public override int DefaultSpriteId => 416;
        public override string DefaultName => "BeastSlayer";

        public override bool Detectable => false;

    }
}