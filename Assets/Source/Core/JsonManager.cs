using DungeonCrawl.Actors;
using DungeonCrawl.Actors.Characters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public JObject LoadJson()
        {
            string jsonString = File.ReadAllText(_fileRoute);
            JObject json = JObject.Parse(jsonString);
            return json;
        }
    }
}
