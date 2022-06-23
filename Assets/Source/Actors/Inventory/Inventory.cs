using DungeonCrawl.Actors.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonCrawl.Actors;
using UnityEngine;
using Assets.Source.Core;


namespace Assets.Source.Actors.Inventory
{
    public class Inventory
    {
        //Contructor(s)
        public Inventory()
        {
            Items = new List<Item>();
            Axe axe = new Axe();
            Potion potion = new Potion();
            Key key = new Key();
        }
        //Propeties
        public List<Item> Items { get; private set; }

        //public Sprite emptyChest;
        private bool collected;


        public void AddItem(Item newItem)
        {
            if (newItem != null)
            { Items.Add(newItem);
            }
        }

        public override string ToString()
        {
            string inventoryContent = string.Empty;
            for (int i = 0; i < Items.Count; i++)
            {
                inventoryContent+= Items[i].DefaultName + "\n";
            }
            return inventoryContent;
        }

        public void RemoveItem()
        {

        }
    }
}


