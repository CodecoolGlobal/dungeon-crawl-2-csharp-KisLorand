using DungeonCrawl.Actors;
using DungeonCrawl.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Actors.Static
{
    public class Portal : Actor
    {
        private int _mapId = 1;

        
        public Portal((int,int) position)
        {
            Position = position;
            _mapId++;

        }

        public override int DefaultSpriteId => 548;

        public override string DefaultName => "Portal";

        public override bool Detectable => false;

        public void PortToNextMap()
        {
                MapLoader.LoadMap(_mapId);
        }
    }

    
}
