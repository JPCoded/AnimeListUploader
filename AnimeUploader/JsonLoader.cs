using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace AnimeUploader
{
   public static class JsonLoader
    {
        public static  Nodes LoadNodeSettings()
        {
            Nodes items;
            using (var r = new StreamReader("NodeSettings.json"))
            {
                var json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<Nodes>>(json)[0];
            }
            return items;
        }

         public static Item LoadDatabaseSettings()
        {
            Item items;
            using (var r = new StreamReader("DatabaseSettings.json"))
            {
                var json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<Item>>(json)[0];
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
