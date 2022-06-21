using DungeonCrawl.Actors.Items;
﻿using System;
using DungeonCrawl.Actors.Static;
using Assets.Source.Core;
using DungeonCrawl.Core;
using UnityEngine;
using Assets.Source.Actors.Static;
using Assets.Source.Actors.Inventory;


namespace DungeonCrawl.Actors.Characters
{
    public class Player : Character
    {
        private const int _health = 10;
        private const int _damage = 5;

        private bool doorIsLocked = true;

        private Inventory _inventory;

        public Player() : base(_health, _damage)
        {
            _inventory = new Inventory();
        }
        protected override void OnUpdate(float deltaTime)
        {
            Portal portal = ActorManager.Singleton.GetPortal();
            if (portal != null && Position == portal.Position)
            {
                ActorManager.Singleton.DestroyAllActors();
                MapLoader.LoadMap(2);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                // Move up
                TryMove(Direction.Up);
                
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                // Move down
                TryMove(Direction.Down);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                // Move left
                TryMove(Direction.Left);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                // Move right
                TryMove(Direction.Right);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                // Pick up item
                _inventory.AddItem(TryToPickUpItem());

            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                // Use potion
                TryToUsePotion();
            }


            CameraController.Singleton.Position = Position;
            UserInterface.Singleton.SetText($"Hp: {Health}", Assets.Source.Core.UserInterface.TextPosition.BottomLeft);
            UserInterface.Singleton.SetText($"Inventory:\n {_inventory.ToString()}", Assets.Source.Core.UserInterface.TextPosition.TopLeft);

        }

        public override bool OnCollision(Actor anotherActor)
        {
            if (anotherActor is Character)
            {
                Character anotherCharacter = (Character)anotherActor;
                if (anotherCharacter is not Player)
                {
                    DoDamage(anotherCharacter);
                }
            }
            if (anotherActor is Door)
            {
                TryToOpenDoor((Door)anotherActor);
            }
            return false;
        }

        protected override void OnDeath()
        {
            Debug.Log("YOU DIED");
        }

        public override int DefaultSpriteId => 24;
        public override string DefaultName => "Player";

        public void TryToUsePotion()
        {
            string itemName = "Potion";
            if (IsInInventory(itemName))
            {
                ApplyHeal(Potion.GetHeal());
                Item usedPotion = GetItemFromInventory(itemName);
                _inventory.RemoveItem(usedPotion);
            }
        }


        public Item TryToPickUpItem()
        {
            var player = ActorManager.Singleton.GetPlayer();
            var items = ActorManager.Singleton.GetListOfValidItems();

            foreach (var item in items)
            {
                if (item != null)
                {
                    if (item.Position == player.Position)
                    {
                        ActorManager.Singleton.DestroyActor(item);
                        return item;
                    }
                }
            }
            return null;
        }

        public void TryToOpenDoor(Door door)
        {
            if (doorIsLocked)
            {
                if (IsInInventory("Key"))
                {
                    var currentDoor = ActorManager.Singleton.GetCurrentDoor(door);
                    ActorManager.Singleton.DestroyActor(currentDoor);
                    doorIsLocked = false;
                }
                else
                {
                    Debug.Log("You don't have a key!");
                }
            }
            Item usedKey = GetItemFromInventory("Key");
            _inventory.RemoveItem(usedKey);
            doorIsLocked = true;
        }

        public void DoDamage(Character anotherCharacter)
        {
            anotherCharacter.ApplyDamage(Damage);
            if (anotherCharacter.Health > 0)
            {
                ApplyDamage(anotherCharacter.Damage);
                Console.WriteLine(_inventory.Items);
            }
            
        }


        public bool IsInInventory(string itemName)
        {
            foreach (var item in _inventory.Items)
            {
                if (item.DefaultName == itemName)
                {
                    return true;
                }
            }

            return false;
        }

        public Item GetItemFromInventory(string itemName)
        {
            foreach (var item in _inventory.Items)
            {
                if (item.DefaultName == itemName)
                {
                    return item;
                }
            }

            return null;
        }


    }

}
