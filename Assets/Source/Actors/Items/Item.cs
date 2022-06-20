using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonCrawl.Actors.Characters;
using DungeonCrawl.Core;
using Assets.Source.Core;

namespace DungeonCrawl.Actors.Items
{
    public abstract class Item : Actor
    {
        protected abstract void OnPickUp();

        public override int Z => -1;

        
    }
}
