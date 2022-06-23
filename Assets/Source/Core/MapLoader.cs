using Assets.Source.Actors.Characters;
using Assets.Source.Actors.Static;
using DungeonCrawl.Actors.Characters;
using DungeonCrawl.Actors.Items;
using DungeonCrawl.Actors.Static;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DungeonCrawl.Core
{
    /// <summary>
    ///     MapLoader is used for constructing maps from txt files
    /// </summary>
    public static class MapLoader
    {
        /// <summary>
        ///     Constructs map from txt file and spawns actors at appropriate positions
        /// </summary>
        /// <param name="id"></param>
        public static int Width { get; private set; }
        public static int Height { get; private set; }
        public static void LoadMap(int id)
        {

            var lines = Regex.Split(Resources.Load<TextAsset>($"map_{id}").text, "\r\n|\r|\n");

            // Read map size from the first line
            var split = lines[0].Split(' ');
            var width = int.Parse(split[0]);
            Width = width;
            var height = int.Parse(split[1]);
            Height = height;    

            // Create actors
            for (var y = 0; y < height; y++)
            {
                var line = lines[y + 1];
                for (var x = 0; x < width; x++)
                {
                    var character = line[x];

                    SpawnActor(character, (x, -y));
                }
            }
            SfxPlayer SoundPlayer = new SfxPlayer();
            SoundPlayer.PlayBackground();

            // Set default camera size and position
            CameraController.Singleton.Size = 3;
            CameraController.Singleton.Position = (width / 2, -height / 2);
        }

        private static void SpawnActor(char c, (int x, int y) position)
        {
            switch (c)
            {
                case '#':
                    ActorManager.Singleton.Spawn<Wall>(position);
                    break;
                case '.':
                    ActorManager.Singleton.Spawn<Floor>(position);
                    break;
                case 'p':
                    ActorManager.Singleton.Spawn<Player>(position);
                    ActorManager.Singleton.Spawn<Floor>(position);
                    break;
                case 's':
                    ActorManager.Singleton.Spawn<Skeleton>(position);
                    ActorManager.Singleton.Spawn<Floor>(position);
                    break;
                case 'k':
                    ActorManager.Singleton.Spawn<Key>(position);
                    ActorManager.Singleton.Spawn<Floor>(position);
                    break;
                case 'a':
                    ActorManager.Singleton.Spawn<Stick>(position);
                    ActorManager.Singleton.Spawn<Floor>(position);
                    break;
                case 'h':
                    ActorManager.Singleton.Spawn<Potion>(position);
                    ActorManager.Singleton.Spawn<Floor>(position);
                    break;
                case 'b':
                    ActorManager.Singleton.Spawn<Blob>(position);
                    ActorManager.Singleton.Spawn<Floor>(position);
                    break;
                case 'd':
                    ActorManager.Singleton.Spawn<Door>(position);
                    ActorManager.Singleton.Spawn<Floor>(position);
                    break;
                case 'x':
                    ActorManager.Singleton.Spawn<Portal>(position);
                    ActorManager.Singleton.Spawn<Floor>(position);
                    break;
                case 'D':
                    ActorManager.Singleton.Spawn<Pig>(position);
                    ActorManager.Singleton.Spawn<Floor>(position);
                    break;
                case 'S':
                    ActorManager.Singleton.Spawn<Armor>(position);
                    ActorManager.Singleton.Spawn<Floor>(position);
                    break;
                case 'H':
                    ActorManager.Singleton.Spawn<Helm>(position);
                    ActorManager.Singleton.Spawn<Floor>(position);
                    break;
                case 'B':
                    ActorManager.Singleton.Spawn<PigBoss>(position);
                    ActorManager.Singleton.Spawn<Floor>(position);
                    break;
                case 'F':
                    ActorManager.Singleton.Spawn<BossFog>(position);
                    ActorManager.Singleton.Spawn<Floor>(position);
                    break;
                case 'f':
                    ActorManager.Singleton.Spawn<BossDoor>(position);
                    ActorManager.Singleton.Spawn<Floor>(position);
                    break;
                case 'X':
                    ActorManager.Singleton.Spawn<BeastSlayer>(position);
                    ActorManager.Singleton.Spawn<Floor>(position);
                    break;
                case ' ':
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        public static void LoadSavedGAme()
        {
            ActorManager.Singleton.DestroyAllActors();
            JObject savedGameJson = ActorManager.JsonManager.LoadJson();
            foreach (JProperty property in savedGameJson.Properties())
            {
                string defaultName = (string)property.Value["defaultName"];
                int posX = (int)property.Value["position"]["Item1"];
                int posY = (int)property.Value["position"]["Item2"];
                (int, int) position = (posX, posY);
                switch (defaultName)
                {
                    case "Wall":
                        ActorManager.Singleton.Spawn<Wall>(position);
                        break;
                    case "Floor":
                        ActorManager.Singleton.Spawn<Floor>(position);
                        break;
                    case "Player":
                        ActorManager.Singleton.SpawnPlayer(position, ActorManager.JsonManager.LoadPlayerInventory());
                        break;
                    case "Skeleton":
                        ActorManager.Singleton.Spawn<Skeleton>(position);
                        break;
                    case "Key":
                        ActorManager.Singleton.Spawn<Key>(position);
                        break;
                    case "Potion":
                        ActorManager.Singleton.Spawn<Potion>(position);
                        break;
                    case "Blob":
                        ActorManager.Singleton.Spawn<Blob>(position);
                        break;
                    case "Door":
                        ActorManager.Singleton.Spawn<Door>(position);;
                        break;
                    case "Portal":
                        ActorManager.Singleton.Spawn<Portal>(position);
                        break;
                    case "Pig":
                        ActorManager.Singleton.Spawn<Pig>(position);
                        break;
                    case "Stick":
                        ActorManager.Singleton.Spawn<Stick>(position);
                        break;
                    case "Armor":
                        ActorManager.Singleton.Spawn<Armor>(position);
                        break;
                    case "Helm":
                        ActorManager.Singleton.Spawn<Helm>(position);
                        break;
                    case "PigBoss":
                        ActorManager.Singleton.Spawn<PigBoss>(position);
                        break;
                    case "BossFog":
                        ActorManager.Singleton.Spawn<BossFog>(position);
                        break;
                    case "BossDoor":
                        ActorManager.Singleton.Spawn<BossDoor>(position);
                        break;
                    case "BeastSlayer":
                        ActorManager.Singleton.Spawn<BeastSlayer>(position);
                        break;
                    case "Bonepile":
                        ActorManager.Singleton.Spawn<Bonepile>(position);
                        break;
                    case "Blood":
                        ActorManager.Singleton.Spawn<Blood>(position);
                        break;
                    case "Lava":
                        ActorManager.Singleton.Spawn<Lava>(position);
                        break;
                    case "Fire":
                        ActorManager.Singleton.Spawn<Fire>(position);
                        break;
                    case " ":
                        break;

                    default:
                        Debug.LogError(defaultName);
                        throw new ArgumentOutOfRangeException();

                }
            }
            // Set default camera size and position
            CameraController.Singleton.Size = 3;
            CameraController.Singleton.Position = (Width / 2, -Height / 2);
        }
    }           
}
    

