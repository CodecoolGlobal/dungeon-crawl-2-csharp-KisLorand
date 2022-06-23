using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawl.Actors.Items
{
    public class Armor : Item
    {
        public override int DefaultSpriteId => 182;
        public override string DefaultName => "Armor";

        public override bool Detectable => false;

    }
}