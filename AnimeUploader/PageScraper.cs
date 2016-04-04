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

            var type = document.DocumentNode.SelectSingleNode("//*[text()='Type:']/parent::div").InnerText.Replace("Type:", "").Trim(); 
            var episode = document.DocumentNode.SelectSingleNode("//*[text()='Episodes:']/parent::div").InnerText.Replace("Episodes:", "").Trim(); ;
            var status = document.DocumentNode.SelectSingleNode("//*[text()='Status:']/parent::div").InnerText.Replace("Status:", "").Trim(); ;
            var aired = document.DocumentNode.SelectSingleNode("//*[text()='Aired:']/parent::div").InnerText.Replace("Aired:", "").Trim(); ;
            var duration = document.DocumentNode.SelectSingleNode("//*[text()='Duration:']/parent::div").InnerText.Replace("Duration:", "").Trim(); ;
            var rating = document.DocumentNode.SelectSingleNode("//*[text()='Rating:']/parent::div").InnerText.Replace("Rating:", "").Trim(); ;
            var synopsis = document.DocumentNode.SelectSingleNode("//span[@itemprop='description']").InnerText.Replace("'", "''").Trim(); ;
            var genres = document.DocumentNode.SelectSingleNode("//*[text()='Genres:']/parent::div").InnerText.Replace("Genres:","").Trim().Split(',');
            var prequel = document.DocumentNode.SelectSingleNode("//*[text()='Prequel:']/parent::tr");
            var sequel = document.DocumentNode.SelectSingleNode("//*[text()='Sequel:']/parent::tr");

           
            
            var newPrequel ="";
            var newSequel = "";
            //Try catch for these two as these are the only things that might or might not exist.
            try
            {
                newPrequel = prequel.InnerText.Replace("Prequel:","").Trim().Replace("'","''");
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                newSequel = sequel.InnerText.Replace("Sequel:", "").Trim().Replace("'", "''");
            }
            catch (Exception)
            {
                // ignored
            }

            anime.ID = AnimeID;
            anime.Rating = Anime.GetRating(rating);
            anime.Type = Anime.GetType(type);
            anime.Episodes = episode == "Unknown"? 0: Convert.ToInt32(episode);
            anime.Duration = duration;
            anime.Aired =aired;
            anime.Description = synopsis;
            anime.Status = Anime.GetStatus(status);
            anime.Prequel = newPrequel;
            anime.Sequel = newSequel;
           // anime.Genres = genres;

            return anime;
        }
    }
}