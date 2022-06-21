using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawl.Actors.Items
{
    public class Potion : Item
    {
        private static readonly int _heal = GetHealValue();
        public override int DefaultSpriteId => 656;
        public override string DefaultName => "Potion";

        public override bool Detectable => false;

        public static int GetHeal()
        {
            return _heal;
        }

        private static int GetHealValue()
        {
            Random random = new Random();
            return random.Next(2, 5);
        }

    }
}
