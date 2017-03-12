using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace AnimeUploader
{
   public class JsonLoader
    {
        public static List<Nodes> LoadNodeSettings()
        {
            List<Nodes> items;
            using (var r = new StreamReader("NodeSettings.json"))
            {
                var json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<Nodes>>(json);
            }
            return items;
        }
        public static List<Item> LoadDatabaseSettings()
        {
            List<Item> items;
            using (var r = new StreamReader("DatabaseSettings.json"))
            {
                var json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<Item>>(json);
            }
            return items;
        }

       public struct Item
        {
            public string DataSource;
            public string InitialCatalog;
            public string IntegratedSecurity;
        }
    }
}
