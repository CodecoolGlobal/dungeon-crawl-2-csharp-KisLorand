using DungeonCrawl.Actors.Items;
﻿using System;
using DungeonCrawl.Actors.Static;
using Assets.Source.Core;
using DungeonCrawl.Core;
using UnityEngine;
using Assets.Source.Actors.Static;
using Assets.Source.Actors.Inventory;
using UnityEngine.Playables;


namespace DungeonCrawl.Actors.Characters
{
    public class Player : Character
    {
        private const int _health = 10;
        private const int _damage = 5;

        private int _killCount;

        private bool doorIsLocked = true;

        private Inventory _inventory;

        private const int _heal = 5;
        private SfxPlayer _soundPlayer;
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
                _soundPlayer.PlayWalk();

            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                // Move down
                TryMove(Direction.Down);
                _soundPlayer.PlayWalk();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                // Move left
                TryMove(Direction.Left);
                _soundPlayer.PlayWalk();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                // Move right
                TryMove(Direction.Right);
                _soundPlayer.PlayWalk();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                // Pick up item
                _inventory.AddItem(TryToPickUpItem());
                _soundPlayer.PlayPick();

            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                // Use potion
                TryToUsePotion();
            }
            if (Input.GetKeyDown(KeyCode.F9))
            {
                ActorManager.Singleton.JsonifyAllActors();
            }


            CameraController.Singleton.Position = Position;
            UserInterface.Singleton.SetText($"Hp: {Health}", Assets.Source.Core.UserInterface.TextPosition.BottomLeft);
            UserInterface.Singleton.SetText($"Inventory:\n {_inventory.ToString()}", Assets.Source.Core.UserInterface.TextPosition.TopLeft);
            UserInterface.Singleton.SetText($"Kills: {_killCount}", Assets.Source.Core.UserInterface.TextPosition.BottomRight);
        }

        public override bool OnCollision(Actor anotherActor)
        {
            if (anotherActor is Character)
            {
                Character anotherCharacter = (Character)anotherActor;
                if (anotherCharacter is not Player)
                {
                    
                    _soundPlayer.PlayHit();
                    DoDamage(anotherCharacter);
                }
            }
            if (anotherActor is Door)
            {
                TryToOpenDoor((Door)anotherActor);
                _soundPlayer.PlayUnlock();
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
                        if (item.DefaultName == "Key")
                        {
                            var value = TryToPickUpKey(item);
                            return value;
                        }
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

            }

            if (anotherCharacter.Health <= 0)
            {
                _killCount++;
                CheckIfQuestCompleted();

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
            

        public void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            SetSprite(DefaultSpriteId);
            _soundPlayer = new SfxPlayer();
        }

        public void CheckIfQuestCompleted()
        {
            if (_killCount == 4)
            {
                UserInterface.Singleton.RemoveTopCenterText();
            }
        }
        

        public Item TryToPickUpKey(Item item)
        {
            string questDescription = $"Current quest: Kill 3 skeletons to get the key!";

            if (_killCount == 4)
            {
                ActorManager.Singleton.DestroyActor(item);
                return item;
            }
            UserInterface.Singleton.SetText(questDescription, Assets.Source.Core.UserInterface.TextPosition.TopCenter);
            return null;
        }
        
    }

}
