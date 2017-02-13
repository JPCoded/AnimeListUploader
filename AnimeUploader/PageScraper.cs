﻿using System;
using HtmlAgilityPack;

namespace AnimeUploader
{
    internal static class PageScraper
    {
        public static Anime GetAnimeInfo(int animeId)
        {
            IAnime anime = new Anime();

            var document = GetPage(animeId);

            var type = document.DocumentNode.SelectSingleNode("//*[text()='Type:']/parent::div").InnerText.Replace("Type:", "").Trim();
            var episode = document.DocumentNode.SelectSingleNode("//*[text()='Episodes:']/parent::div").InnerText.Replace("Episodes:", "").Trim();
            var status = document.DocumentNode.SelectSingleNode("//*[text()='Status:']/parent::div").InnerText.Replace("Status:", "").Trim();
            var aired = document.DocumentNode.SelectSingleNode("//*[text()='Aired:']/parent::div").InnerText.Replace("Aired:", "").Trim();
            var duration = document.DocumentNode.SelectSingleNode("//*[text()='Duration:']/parent::div").InnerText.Replace("Duration:", "").Trim();
            var rating = document.DocumentNode.SelectSingleNode("//*[text()='Rating:']/parent::div").InnerText.Replace("Rating:", "").Trim();
            var synopsis = document.DocumentNode.SelectSingleNode("//meta[@property='og:description']").GetAttributeValue("content","").Replace("'", "''").Trim();
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

            anime.ID = animeId;
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

            return (Anime)anime;
        }

        private static HtmlDocument GetPage(int idToFetch)
        {
            
            var getHtmlWeb = new HtmlWeb();
            return getHtmlWeb.Load(string.Format("http://myanimelist.net/anime/{0}", idToFetch));
        }
    }
}