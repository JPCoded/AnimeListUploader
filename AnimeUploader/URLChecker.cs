﻿#region

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

#endregion

namespace AnimeUploader
{
    internal sealed class UrlChecker : IDisposable
    {
        private const int ThreadCount = 3;
        private CountdownEvent _countdownEvent;
        private SemaphoreSlim _throttler;
        private List<Anime> AnimeList { get; set; }

        public void Dispose()
        {
            _countdownEvent.Dispose();
            _throttler.Dispose();
        }

        //possibly convert to list of anime types
        public IEnumerable<Anime> Check(IList<string> urls)
        {
            AnimeList = new List<Anime>();
            _countdownEvent = new CountdownEvent(urls.Count);
            _throttler = new SemaphoreSlim(ThreadCount);

            Task.Run( // prevent UI thread lock
                async () =>
                {
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
        //find way to make this not return void
        private async void ProccessUrl(string url)
        {
            try
            {
                var page = await new WebClient().DownloadStringTaskAsync(new Uri(url));
                var id = url.Split('/');
                ProccessResult(page, id[4]);
            }
            finally
            {
                _throttler.Release();
                _countdownEvent.Signal();
            }
        }

        //Figure way to combine with PageScraper function
        private void ProccessResult(string page, string id)
        {
            IAnime anime = new Anime();
            var document = new HtmlDocument();
            document.LoadHtml(page);
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
            if (prequelId != null)
            {
                newPrequelId = prequelId.GetAttributeValue("href", "").Split('/');
                newPrequel = prequel.InnerText.Replace("Prequel:", "").Trim().Replace("'", "''");
            }

            if (sequelId != null)
            {
                newSequelId = sequelId.GetAttributeValue("href", "").Split('/');
                newSequel = sequel.InnerText.Replace("Sequel:", "").Trim().Replace("'", "''");
            }

            anime.ID = Convert.ToInt32(id);
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

            AnimeList.Add((Anime)anime);
        }
    }
}