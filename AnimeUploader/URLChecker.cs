using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace AnimeUploader
{
    class UrlChecker : IDisposable
    {
        private const int ThreadCount = 3;
        private CountdownEvent _countdownEvent;
        private SemaphoreSlim _throttler;
        private List<Anime> AnimeList {get;set;} 
        
        public void Dispose()
        {
            _countdownEvent.Dispose();
            _throttler.Dispose();
        }
        //possibly convert to list of anime types
        public List<Anime> Check(IList<string> urls)
        {
          AnimeList = new List<Anime>();
            _countdownEvent = new CountdownEvent(urls.Count);
            _throttler = new SemaphoreSlim(ThreadCount);
            
            Task.Run( // prevent UI thread lock
                async () => {
                                foreach (var url in urls)
                                {
                                    // do an async wait until we can schedule again
                                    await _throttler.WaitAsync();
                                   ProccessUrl(url); // NOT await
                                }
                               
                                _countdownEvent.Wait();
                });
            return AnimeList;

        }

        private async void ProccessUrl(string url)
        {
            try
            {
                var page = await new WebClient().DownloadStringTaskAsync(new Uri(url));
                var id = url.Split('/');
             ProccessResult(page,id[4]);
            }
            finally
            {
                _throttler.Release();
                _countdownEvent.Signal();
            }
           
        }
        private static List<Nodes> LoadJson()
        {
            var items = new List<Nodes>();
            using (var r = new StreamReader("NodeSettings.json"))
            {
                var json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<Nodes>>(json);
            }
            return items;
        }


        private void ProccessResult(string page, string id)
        {
            var anime = new Anime();
            var nodeSettings = LoadJson();
            var document = new HtmlDocument();
            document.LoadHtml(page);
            var type = document.DocumentNode.SelectSingleNode(nodeSettings[0].Type).InnerText.Replace("Type:", "").Trim();
            var episode = document.DocumentNode.SelectSingleNode(nodeSettings[0].Episode).InnerText.Replace("Episodes:", "").Trim();
            var status = document.DocumentNode.SelectSingleNode(nodeSettings[0].Status).InnerText.Replace("Status:", "").Trim();
            var aired = document.DocumentNode.SelectSingleNode(nodeSettings[0].Aired).InnerText.Replace("Aired:", "").Trim();
            var duration = document.DocumentNode.SelectSingleNode(nodeSettings[0].Duration).InnerText.Replace("Duration:", "").Trim();
            var rating = document.DocumentNode.SelectSingleNode(nodeSettings[0].Rating).InnerText.Replace("Rating:", "").Trim();
            var synopsis = document.DocumentNode.SelectSingleNode(nodeSettings[0].Synopsis).GetAttributeValue("content", "").Replace("'", "''").Trim();
            var genres = document.DocumentNode.SelectSingleNode(nodeSettings[0].Genres).InnerText.Replace("Genres:", "").Replace(", ", ",").Trim();
            var prequelId = document.DocumentNode.SelectSingleNode(nodeSettings[0].PrequelId);
            var prequel = document.DocumentNode.SelectSingleNode(nodeSettings[0].Prequel);
            var sequelId = document.DocumentNode.SelectSingleNode(nodeSettings[0].SequelId);
            var sequel = document.DocumentNode.SelectSingleNode(nodeSettings[0].Sequel);
            var title = document.DocumentNode.SelectSingleNode(nodeSettings[0].Title).GetAttributeValue("content", "").Replace("'", "''").Trim();

            string[] newPrequelId = null;
            var newPrequel = "";
            string[] newSequelId = null;
            var newSequel = "";
            //Try catch for these two as these are the only things that might or might not exist.
            if(prequelId != null) {
                newPrequelId = prequelId.GetAttributeValue("href", "").Split('/');
                newPrequel = prequel.InnerText.Replace("Prequel:", "").Trim().Replace("'", "''");
            }

            if (sequelId != null)
            {
                newSequelId = sequelId.GetAttributeValue("href", "").Split('/');
                newSequel = sequel.InnerText.Replace("Sequel:", "").Trim().Replace("'", "''");

            }

            anime.ID = Convert.ToInt32(id);
            anime.Rating = Anime.GetRating(rating);
            anime.Type = Anime.GetType(type);
            anime.Episodes = episode == "Unknown" ? 0 : Convert.ToInt32(episode);
            anime.Duration = duration;
            anime.Aired = aired;
            anime.Description = synopsis;
            anime.Status = Anime.GetStatus(status);
            anime.PrequelID = newPrequelId == null ? 0 : Convert.ToInt32(newPrequelId[2]);
            anime.Prequel = newPrequel;
            anime.SequelID = newSequelId == null ? 0 : Convert.ToInt32(newSequelId[2]);
            anime.Sequel = newSequel;
            anime.Genre = genres;
            anime.Title = title;

            AnimeList.Add(anime);
           
        }
    }
}
