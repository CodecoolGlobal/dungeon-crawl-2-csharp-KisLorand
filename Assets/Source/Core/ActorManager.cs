﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Source.Actors.Static;
using DungeonCrawl.Actors;
using DungeonCrawl.Actors.Characters;
using DungeonCrawl.Actors.Items;
using DungeonCrawl.Actors.Static;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.U2D;
using Newtonsoft.Json.Linq;
using System.IO;

namespace DungeonCrawl.Core
{
    /// <summary>
    ///     Main class for Actor management - spawning, destroying, detecting at positions, etc
    /// </summary>
    public class ActorManager : MonoBehaviour
    {
        /// <summary>
        ///     ActorManager singleton
        /// </summary>
        public static ActorManager Singleton { get; private set; }

        private SpriteAtlas _spriteAtlas;
        private HashSet<Actor> _allActors;

        private void Awake()
        {
            if (Singleton != null)
            {
                Destroy(this);
                return;
            }

            Singleton = this;

            _allActors = new HashSet<Actor>();
            _spriteAtlas = Resources.Load<SpriteAtlas>("Spritesheet");
        }

        /// <summary>
        ///     Returns actor present at given position (returns null if no actor is present)
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Actor GetActorAt((int x, int y) position)
        {
            return _allActors.FirstOrDefault(actor => actor.Detectable && actor.Position == position);
        }

        /// <summary>
        ///     Returns actor of specific subclass present at given position (returns null if no actor is present)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="position"></param>
        /// <returns></returns>
        public T GetActorAt<T>((int x, int y) position) where T : Actor
        {
            return _allActors.FirstOrDefault(actor => actor.Detectable && actor is T && actor.Position == position) as T;
        }

        /// <summary>
        ///     Unregisters given actor (use when killing/destroying)
        /// </summary>
        /// <param name="actor"></param>
        /// 
        public Player GetPlayer()
        {
            foreach (var actor in _allActors)
            {
                if (actor.DefaultName == "Player")
                    return (Player)actor;
            }
            return null;
        }
        public void DestroyActor(Actor actor)
        {
            _allActors.Remove(actor);
            Destroy(actor.gameObject);

        }

        /// <summary>
        ///     Used for cleaning up the scene before loading a new map
        /// </summary>
        public void DestroyAllActors()
        {
            var actors = _allActors.ToArray();

            foreach (var actor in actors)
                DestroyActor(actor);
        }

        /// <summary>
        ///     Returns sprite with given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sprite GetSprite(int id)
        {
            return _spriteAtlas.GetSprite($"kenney_transparent_{id}");
        }

        /// <summary>
        ///     Spawns given Actor type at given position
        /// </summary>
        /// <typeparam name="T">Actor type</typeparam>
        /// <param name="position">Position</param>
        /// <param name="actorName">Actor's name (optional)</param>
        /// <returns></returns>
        public T Spawn<T>((int x, int y) position, string actorName = null) where T : Actor
        {
            return Spawn<T>(position.x, position.y, actorName);
        }

        /// <summary>
        ///     Spawns given Actor type at given position
        /// </summary>
        /// <typeparam name="T">Actor type</typeparam>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="actorName">Actor's name (optional)</param>
        /// <returns></returns>
        public T Spawn<T>(int x, int y, string actorName = null) where T : Actor
        {
            var go = new GameObject();  // <= creates new GameBoject
            go.AddComponent<SpriteRenderer>(); // <= add component to GO (SpriteRenderer unity component)

            var component = go.AddComponent<T>(); // <= add custom class component to GO

            go.name = actorName ?? component.DefaultName;
            component.Position = (x, y);

            _allActors.Add(component);

            return component;
        }

        public List<Item> GetListOfValidItems()
        {
            List<Item> items = new();

            foreach (var actor in _allActors)
            {
                if (actor is Item item)
                {
                    items.Add(item);
                }
            }

            return items;
        }

        public Door GetCurrentDoor(Actor door)
        {
       
            foreach (var actor in _allActors)
            {
                if (actor.Position == door.Position)
                {
                    return (Door)actor;
                }
            }

            return null;
        }


        public void ColorTile(string type, Color color)
        {
            foreach (var actor in _allActors)
            {
                if (actor.DefaultName == type)
                {
                    actor._spriteRenderer.color = color;
                }
            }
        }

        public Portal GetPortal()
        {
            foreach (var actor in _allActors)
            {
                if (actor is Portal portal)
                {
                    return portal;
                }
            }

            return null;

        }

        public void JsonifyAllActors()
        {
            int objectId = 0;
            string json = "{\n";
            foreach (var actor in _allActors)
            {
                var comma = ",";
                if (actor == _allActors.Last())
                {
                    comma = "";
                }
                json += $"\"Object{objectId}\":\n\t{{\n";
                var position = JsonConvert.SerializeObject(actor.Position);
                var defaultName = JsonConvert.SerializeObject(actor.DefaultName);
                var defaultSpriteId = JsonConvert.SerializeObject(actor.DefaultSpriteId);
                var decetable = JsonConvert.SerializeObject(actor.Detectable);
                var z = JsonConvert.SerializeObject(actor.Z);
                if (actor is Character character)
                {
                    var damage = JsonConvert.SerializeObject(character.Damage);
                    var health = JsonConvert.SerializeObject(character.Health);
                    json += $"\t\"damage\": {damage},\n" +
                            $"\t\"health\": {health},\n";
                    if (character is Player player && !player.Inventory.Items.Any())
                    {
                        var invetory = JsonConvert.SerializeObject(player.Inventory);
                        json += $"\t\"inventory\": {invetory},\n";
                    }
                }
                json +=
                     $"\t\"position\": {position},\n" +
                    $"\t\"defaultName\": {defaultName},\n" +
                    $"\t\"defaultSpriteId\": {defaultSpriteId},\n" +
                    $"\t\"decectable\": {decetable},\n" +
                    $"\t\"z\": {z}\n";
                json += $"\t}}{comma}\n";
                objectId++;

            }
            json += "}";
            File.WriteAllText("gameSave.json", json);
        }
    }
}
