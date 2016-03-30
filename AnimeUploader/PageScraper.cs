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

            var newType = type.InnerText.Trim().Remove(0, 6).Trim();
            var newEpisodes = episode.InnerText.Trim().Remove(0, 9).Trim();
            var newStatus = statu.InnerText.Trim().Remove(0, 7).Trim();
            var newAired = aired.InnerText.Trim().Remove(0, 6).Trim();
            var newDuration = duration.InnerText.Trim().Remove(0, 10).Trim();
            var newRating = rating.InnerText.Trim().Remove(0, 9).Trim();
            var newSynopsis = synopsis.InnerText.Trim().Replace("'","''");
            var newPrequel ="";
            try
            {
                newPrequel = prequel.InnerText.Trim().Remove(0, 8);
            }
            catch(Exception ex)
            { }

            anime.ID = AnimeID;
            anime.Rating = GetRating(newRating);
            anime.Type = GetType(newType);
            anime.Episodes = newEpisodes == "Unknown"? 0: Convert.ToInt32(newEpisodes);
            anime.Duration = newDuration;
            anime.Aired = newAired;
            anime.Description = newSynopsis;
            anime.Status = GetStatus(newStatus);

            return anime;
        }

        public int GetRating(string rating)
        {
            var ratingNumber = 0;

            switch (rating)
            {
                case "G - All Ages":
                    ratingNumber = 1;
                    break;
                case "PG - Children":
                    ratingNumber = 2;
                    break;
                case "PG-13 - Teens 13 or older":
                    ratingNumber = 3;
                    break;
                case "R - 17+ (violence &amp; profanity)":
                    ratingNumber = 4;
                    break;
                case "R+ - Mild Nudity":
                    ratingNumber = 5;
                    break;
                default:
                    ratingNumber = 6;
                    break;
            }
            return ratingNumber;
        }

        public int GetType(string type)
        {
            var typeNumber = 0;

            switch (type)
            {
                case "TV":
                    typeNumber = 1;
                    break;
                case "OVA":
                    typeNumber = 2;
                    break;
                case "ONA":
                    typeNumber = 3;
                    break;
                case "Special":
                    typeNumber = 4;
                    break;
                case "Movie":
                    typeNumber = 5;
                    break;
            }
            return typeNumber;
        }

        public int GetStatus(string status)
        {
            var statusNumber = 0;

            switch (status)
            {
                case "Not yet aired":
                    statusNumber = 1;
                    break;
                case "Currently Airing":
                    statusNumber = 2;
                    break;
                case "Finished Airing":
                    statusNumber = 3;
                    break;
            }
            return statusNumber;
        }
    }
}