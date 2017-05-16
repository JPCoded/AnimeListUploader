#region

using System;
using HtmlAgilityPack;

#endregion

namespace AnimeUploader
{
    internal static class PageScraper
    {
        public static Anime GetAnimeInfo(int animeId)
        {
            IAnime anime = new Anime();
         
            var document = GetPage(animeId);
            var documentNodes = new GetDocumentNodes(document);

            var type = documentNodes.getType();

            var episode = documentNodes.getEpisode();

            var status = documentNodes.getStatus();

            var aired = documentNodes.getAired();

            var duration = documentNodes.getDuration();

            var rating = documentNodes.getRating();

            var synopsis = documentNodes.getSynopsis();

            var genres = documentNodes.getGenres();

            var prequelId = documentNodes.getPrequelId();
            var prequel = documentNodes.getPrequel();
            var sequelId = documentNodes.getSequelId();
            var sequel = documentNodes.getSequel();
            var title = documentNodes.getTitles();

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

            return (Anime) anime;
        }

        private static HtmlDocument GetPage(int idToFetch)
        {
            var getHtmlWeb = new HtmlWeb();
            return getHtmlWeb.Load(string.Format("http://myanimelist.net/anime/{0}", idToFetch));
        }
    }
}