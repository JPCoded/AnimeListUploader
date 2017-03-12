using System;
using HtmlAgilityPack;


namespace AnimeUploader
{
    internal static class PageScraper
    {


        public static Anime GetAnimeInfo(int animeId)
        {
            IAnime anime = new Anime();
           var nodeSettings = JsonLoader.LoadNodeSettings();
         
            var document = GetPage(animeId);

            var type = document.DocumentNode.SelectSingleNode(nodeSettings[0].Type).InnerText.Replace("Type:", "").Trim();
            var episode = document.DocumentNode.SelectSingleNode(nodeSettings[0].Episode).InnerText.Replace("Episodes:", "").Trim();
            var status = document.DocumentNode.SelectSingleNode(nodeSettings[0].Status).InnerText.Replace("Status:", "").Trim();
            var aired = document.DocumentNode.SelectSingleNode(nodeSettings[0].Aired).InnerText.Replace("Aired:", "").Trim();
            var duration = document.DocumentNode.SelectSingleNode(nodeSettings[0].Duration).InnerText.Replace("Duration:", "").Trim();
            var rating = document.DocumentNode.SelectSingleNode(nodeSettings[0].Rating).InnerText.Replace("Rating:", "").Trim();
            var synopsis = document.DocumentNode.SelectSingleNode(nodeSettings[0].Synopsis).GetAttributeValue("content","").Replace("'", "''").Trim();
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

            anime.ID = animeId;
            anime.Rating = anime.GetRating(rating);
            anime.Type = anime.GetType(type);
            anime.Episodes = episode == "Unknown" ? 0 : Convert.ToInt32(episode);
            anime.Duration = duration;
            anime.Aired = aired;
            anime.Description = synopsis;
            anime.Status = anime.GetStatus(status);
            anime.PrequelID = newPrequelId == null ? 0 : Convert.ToInt32(newPrequelId[2]);
            anime.Prequel = newPrequel;
            anime.SequelID = newSequelId == null ? 0 : Convert.ToInt32(newSequelId[2]);
            anime.Sequel = newSequel;
            anime.Genre = genres;
            anime.Title = title;

            return (Anime)anime;
        }

       
        private static HtmlDocument GetPage(int idToFetch)
        {
            
            var getHtmlWeb = new HtmlWeb();
            return getHtmlWeb.Load(string.Format("http://myanimelist.net/anime/{0}", idToFetch));
        }
    }
}