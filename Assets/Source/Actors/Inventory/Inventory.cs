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
    internal class Inventory
    {
        //Contructor(s)
        public Inventory()
        {
            Items = new List<Item>();
        }
        //Propeties
        public List<Item> Items { get; private set; }

        public Sprite emptyChest;
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
            int potionCount = 0;
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].DefaultName == "Potion")
                {
                    potionCount++;
                }
                else
                {
                    inventoryContent += Items[i].DefaultName + "\n";
                }
                
            }

            if (potionCount > 0)
            {
                inventoryContent += $"Potion [{potionCount}]\n";

            }
            
            return inventoryContent;
        }

        public void RemoveItem(Item key)
        {
            Items.Remove(key);
        }
    }
}


