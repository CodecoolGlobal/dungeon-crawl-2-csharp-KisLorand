﻿using DungeonCrawl.Actors.Items;
﻿using System;
using DungeonCrawl.Actors.Static;
using Assets.Source.Core;
using DungeonCrawl.Core;
using UnityEngine;
using Assets.Source.Actors.Static;
using Assets.Source.Actors.Inventory;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;


namespace DungeonCrawl.Actors.Characters
{
    public class Player : Character
    {
        private const int _health = 10;
        private const int _damage = 5;

        private int _killCount;

        private bool doorIsLocked = true;
        private bool isQuestOk = false;
        private bool _hasBeastSlayer = false;

        private const int _heal = 5;
        private SfxPlayer _soundPlayer;
        public Inventory Inventory { get; set; }

        public Player() : base(_health, _damage)
        {       
            if(Inventory == null)
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
                Inventory.AddItem(TryToPickUpItem());
                _soundPlayer.PlayPick();

            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                // Use potion
                TryToUsePotion();
            }
            if (Input.GetKeyDown(KeyCode.F9))
            {
                Debug.Log("Game Saved!");
                ActorManager.JsonManager.JsonifyAllActors();
            }
            if (Input.GetKeyDown(KeyCode.F10))
            {
                MapLoader.LoadSavedGAme();
                
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }

            
            CameraController.Singleton.Position = Position;
            UserInterface.Singleton.SetText($"Hp: {Health}", Assets.Source.Core.UserInterface.TextPosition.BottomLeft);
            UserInterface.Singleton.SetText($"Inventory:\n {Inventory.ToString()}", Assets.Source.Core.UserInterface.TextPosition.TopLeft);
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
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
                Inventory.RemoveItem(usedPotion);
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
                        EquipItem(item);
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
            Inventory.RemoveItem(usedKey);
            doorIsLocked = true;
        }

        public void DoDamage(Character anotherCharacter)
        {
            int additionalDamage = 0;

            if (_hasBeastSlayer)
            {
                additionalDamage = 10;
            }

            anotherCharacter.ApplyDamage(Damage + additionalDamage);
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
            foreach (var item in Inventory.Items)
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
            foreach (var item in Inventory.Items)
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
                isQuestOk = true;
            }
        }
        

        public Item TryToPickUpKey(Item item)
        {
            string questDescription = $"Current quest: Kill 3 skeletons to get the key!";

            if (isQuestOk)
            {
                ActorManager.Singleton.DestroyActor(item);
                return item;
            }
            UserInterface.Singleton.SetText(questDescription, Assets.Source.Core.UserInterface.TextPosition.TopCenter);
            return null;
        }

        public void EquipItem(Item item)
        {
            if (item.DefaultName == "Stick")
            {
                this.SetSprite(25);
            }
            if (item.DefaultName == "Armor")
            {
                EquipShield();
                this.SetSprite(26);
            }
            if (item.DefaultName == "Helm")
            {
                EquipHelm();
                isQuestOk = true;
                this.SetSprite(27);
            }
            if (item.DefaultName == "BeastSlayer")
            {
                EquipBeastSlayer();
                this.SetSprite(28);
            }
        }

        public void EquipBeastSlayer()
        {
            _hasBeastSlayer = true;
        }


    }

}
