using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonCrawl.Core;

namespace DungeonCrawl.Actors.Items 
{
    public class Key : Item
    {
        public override int DefaultSpriteId => 559;
        public override string DefaultName => "Key";

        public override bool Detectable => false;

        protected override void OnPickUp()
        {
        }
        
    }
}
