using DungeonCrawl.Actors.Items;
﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        private const int _heal = 5;

        public Inventory Inventory { get; private set; }

        public Player() : base(_health, _damage)
        {
            Inventory = new Inventory();
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
                Inventory.AddItem(CheckForItem());

            }
            if (Input.GetKeyDown(KeyCode.F9))
            {
                ActorManager.Singleton.JsonifyAllActors();
            }


            CameraController.Singleton.Position = Position;
            UserInterface.Singleton.SetText($"Hp: {Health}", Assets.Source.Core.UserInterface.TextPosition.BottomLeft);
            UserInterface.Singleton.SetText($"Inventory:\n {Inventory.ToString()}", Assets.Source.Core.UserInterface.TextPosition.TopLeft);

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
                TryToOpenDoor();
            }
            return false;
        }

        protected override void OnDeath()
        {
            Debug.Log("YOU DIED");
        }

        public override int DefaultSpriteId => 24;
        public override string DefaultName => "Player";

        //List<Item> inventory = new List<Item>();


        public Item CheckForItem()
        {
            var player = ActorManager.Singleton.GetPlayer();
            var items = ActorManager.Singleton.GetItemList();

            foreach (var item in items)
            {
                if (item != null)
                {
                    if (item.Position == player.Position)
                    {
                        if (item is Potion)
                        {
                            ApplyHeal(_heal);
                            ActorManager.Singleton.DestroyActor(item);
                            return null;
                        }
                        ActorManager.Singleton.DestroyActor(item);
                        return item;
                    }
                }
            }
            return null;
        }

        public void TryToOpenDoor()
        {
            var door = ActorManager.Singleton.GetDoor();

        /*    foreach(Item item in _inventory)
            {
                if (item is Key) 
                {
                    ActorManager.Singleton.DestroyActor(door);
                }
            }*/
            
        }

        public void DoDamage(Character anotherCharacter)
        {
            anotherCharacter.ApplyDamage(Damage);
            if (anotherCharacter.Health > 0)
            {
                ApplyDamage(anotherCharacter.Damage);
                Console.WriteLine(Inventory.Items);
            }
            
        }


    }

}
