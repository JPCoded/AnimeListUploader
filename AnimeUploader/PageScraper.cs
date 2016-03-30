using System;
using HtmlAgilityPack;

namespace AnimeUploader
{
    internal class PageScraper
    {
        public Anime GetAnimeInfo(int AnimeID)
        {
            var anime = new Anime();
            var getHtmlWeb = new HtmlWeb();

            var document = getHtmlWeb.Load("http://myanimelist.net/anime/" + AnimeID);

            var type = document.DocumentNode.SelectSingleNode("//*[text()='Type:']/parent::div");
            var episode = document.DocumentNode.SelectSingleNode("//*[text()='Episodes:']/parent::div");
            var statu = document.DocumentNode.SelectSingleNode("//*[text()='Status:']/parent::div");
            var aired = document.DocumentNode.SelectSingleNode("//*[text()='Aired:']/parent::div");
            var duration = document.DocumentNode.SelectSingleNode("//*[text()='Duration:']/parent::div");
            var rating = document.DocumentNode.SelectSingleNode("//*[text()='Rating:']/parent::div");
            var synopsis = document.DocumentNode.SelectSingleNode("//span[@itemprop='description']");
            var prequel = document.DocumentNode.SelectSingleNode("//*[text()='Prequel:']/parent::tr");
            var sequel = document.DocumentNode.SelectSingleNode("//*[text()='Sequel:']/parent::tr");

            var newType = type.InnerText.Trim().Replace("Type:","").Trim();
            var newEpisodes = episode.InnerText.Trim().Remove(0, 9).Trim();
            var newStatus = statu.InnerText.Trim().Remove(0, 7).Trim();
            var newAired = aired.InnerText.Trim().Remove(0, 6).Trim();
            var newDuration = duration.InnerText.Trim().Remove(0, 10).Trim();
            var newRating = rating.InnerText.Trim().Remove(0, 9).Trim();
            var newSynopsis = synopsis.InnerText.Trim().Replace("'","''");
            var newPrequel ="";
            var newSequel = "";
            try
            {
                newPrequel = prequel.InnerText.Trim().Replace("Prequel:","").Trim().Replace("'","''");
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                newSequel = sequel.InnerText.Trim().Replace("Sequel:", "").Trim().Replace("'", "''");
            }
            catch (Exception)
            {
                // ignored
            }

            anime.ID = AnimeID;
            anime.Rating = anime.GetRating(newRating);
            anime.Type = anime.GetType(newType);
            anime.Episodes = newEpisodes == "Unknown"? 0: Convert.ToInt32(newEpisodes);
            anime.Duration = newDuration;
            anime.Aired = newAired;
            anime.Description = newSynopsis;
            anime.Status = anime.GetStatus(newStatus);
            anime.Prequel = newPrequel;
            anime.Sequel = newSequel;

            return anime;
        }
    }
}