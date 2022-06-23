using Assets.Source.Actors.Inventory;
using DungeonCrawl.Actors;
using DungeonCrawl.Actors.Characters;
using DungeonCrawl.Actors.Items;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Core
{
    public class JsonManager
    {
        private  HashSet<Actor> _allActors;
        private string _fileRoute = "C:\\Users\\balázs\\source\\repos\\OOPC#Projets\\dungeon-crawl-2-csharp-KisLorand\\gameSave.json";

        public JsonManager(HashSet<Actor> allActors)
        {
            _allActors = allActors;
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
                    if (character is Player player && player.Inventory.Items.Any())
                    {
                        var invetory = "[" + string.Join(", ", player.Inventory.Items.Select(s => $"\"{s.DefaultName}\"")) + "]";
                        json += $"\t\"inventory\": {invetory},\n";
                    }
                    else
                    {
                        json += $"\t\"inventory\":[],\n";
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

        public JObject LoadJson()
        {
            string jsonString = File.ReadAllText(_fileRoute);
            JObject json = JObject.Parse(jsonString);
            return json;
        }

        public Inventory LoadPlayerInventory()
        {
            string jsonString = File.ReadAllText(_fileRoute);
            JObject json = JObject.Parse(jsonString);
            Inventory inventory = new Inventory();
            foreach (JProperty item in json.Properties())
            {
                if ((string)item.Value["defaultName"] == "Player")
                {
                    var items = item.Value["inventory"];

                    foreach (var inventoryItem in items)
                    {
                        switch ((string)inventoryItem)
                        {
                            case "Potion":
                                inventory.Items.Add(new Potion());
                                break;
                            case "Key":
                                inventory.Items.Add(new Key());
                                break;
                            case "Helm":
                                inventory.Items.Add(new Helm());
                                break;
                            case "BeastSlayer":
                                inventory.Items.Add(new BeastSlayer());
                                break;
                            case "Stick":
                                inventory.Items.Add(new Stick());
                                break;
                            case "Armor":
                                inventory.Items.Add(new Armor());
                                break;
                        }
                    }

                        
                }

            
            }
            return inventory;
        }
    }
}
