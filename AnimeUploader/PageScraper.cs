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

            var type = documentNodes.GetType();

            var episode = documentNodes.GetEpisode();

            var status = documentNodes.GetStatus();

            var aired = documentNodes.GetAired();

            var duration = documentNodes.GetDuration();

            var rating = documentNodes.GetRating();

            var synopsis = documentNodes.GetSynopsis();

            var genres = documentNodes.GetGenres();

            var prequelId = documentNodes.GetPrequelId();
            var prequel = documentNodes.GetPrequel();
            var sequelId = documentNodes.GetSequelId();
            var sequel = documentNodes.GetSequel();
            var title = documentNodes.GetTitles();

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
            return getHtmlWeb.Load($"http://myanimelist.net/anime/{idToFetch}");
        }
    }
}