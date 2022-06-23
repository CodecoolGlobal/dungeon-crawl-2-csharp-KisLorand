using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawl.Actors.Items
{
    public class Helm : Item
    {
        public override int DefaultSpriteId => 31;
        public override string DefaultName => "Helm";

        public override bool Detectable => false;

    }
}