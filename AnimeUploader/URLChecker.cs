using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

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

             ProccessResult(page);
            }
            finally
            {
                _throttler.Release();
                _countdownEvent.Signal();
            }
           
        }

        private void ProccessResult(string page)
        {
            var anime = new Anime();

            var document = new HtmlDocument();
            document.LoadHtml(page);
            var type = document.DocumentNode.SelectSingleNode("//*[text()='Type:']/parent::div").InnerText.Replace("Type:", "").Trim();
            var episode = document.DocumentNode.SelectSingleNode("//*[text()='Episodes:']/parent::div").InnerText.Replace("Episodes:", "").Trim();
            var status = document.DocumentNode.SelectSingleNode("//*[text()='Status:']/parent::div").InnerText.Replace("Status:", "").Trim();
            var aired = document.DocumentNode.SelectSingleNode("//*[text()='Aired:']/parent::div").InnerText.Replace("Aired:", "").Trim();
            var duration = document.DocumentNode.SelectSingleNode("//*[text()='Duration:']/parent::div").InnerText.Replace("Duration:", "").Trim();
            var rating = document.DocumentNode.SelectSingleNode("//*[text()='Rating:']/parent::div").InnerText.Replace("Rating:", "").Trim();
            var synopsis = document.DocumentNode.SelectSingleNode("//meta[@property='og:description']").GetAttributeValue("content", "").Replace("'", "''").Trim();
            var genres = document.DocumentNode.SelectSingleNode("//*[text()='Genres:']/parent::div").InnerText.Replace("Genres:", "").Replace(", ", ",").Trim();
            var prequelId = document.DocumentNode.SelectSingleNode("//*[text()='Prequel:']/parent::tr/descendant::a");
            var prequel = document.DocumentNode.SelectSingleNode("//*[text()='Prequel:']/parent::tr");
            var sequelId = document.DocumentNode.SelectSingleNode("//*[text()='Sequel:']/parent::tr/descendant::a");
            var sequel = document.DocumentNode.SelectSingleNode("//*[text()='Sequel:']/parent::tr");
            var title = document.DocumentNode.SelectSingleNode("//meta[@property='og:title']").GetAttributeValue("content", "").Replace("'", "''").Trim();

            string[] newPrequelId = null;
            var newPrequel = "";
            string[] newSequelId = null;
            var newSequel = "";
            //Try catch for these two as these are the only things that might or might not exist.
            try
            {
                newPrequelId = prequelId.GetAttributeValue("href", "").Split('/');
                newPrequel = prequel.InnerText.Replace("Prequel:", "").Trim().Replace("'", "''");
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                newSequelId = sequelId.GetAttributeValue("href", "").Split('/');
                newSequel = sequel.InnerText.Replace("Sequel:", "").Trim().Replace("'", "''");
            }
            catch (Exception)
            {
                // ignored
            }

           // anime.ID = animeId;
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
