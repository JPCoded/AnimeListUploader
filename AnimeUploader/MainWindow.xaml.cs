#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

#endregion

//REFACTOR whole thing badly

namespace AnimeUploader
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow
    {
        internal MainWindow()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            var animeElements = GetElements("http://myanimelist.net/malappinfo.php?u=CWarlord87&status=all&type=anime");

            txtResults.Text += "START: " + GetTime() + "\n";
            foreach (var anime in animeElements)
            {
                var myanimeObject = new MyAnime();

                var status = anime.Element("my_status")?.Value;
                var episodes = Convert.ToInt32(anime.Element("my_watched_episodes")?.Value);
                var score = Convert.ToInt32(anime.Element("my_score")?.Value);
                var animeId = Convert.ToInt32(anime.Element("series_animedb_id")?.Value);

                AnimeFunction(animeId);

                if (DatabaseControl.AnimeExists(animeId, true))
                {
                    myanimeObject.AnimeID = animeId;
                    myanimeObject.WatchedEpisodes = episodes;
                    myanimeObject.Score = score;
                    myanimeObject.Status = status;
                    DatabaseControl.InsertAnime(myanimeObject);
                }
                else
                {
                    var myanime = DatabaseControl.GetMyAnimeById(animeId);
                    var myStatus = MyAnime.GetStatus(status);
                    //change to just update specific items
                    if (Convert.ToInt32(myanime.Status) != MyAnime.GetStatus(status) ||
                        score != Convert.ToInt32(myanime.Score) || episodes != Convert.ToInt32(myanime.WatchedEpisodes))
                        DatabaseControl.UpdateAnime(animeId, score, episodes, myStatus);
                }
            }

            txtResults.Text += "DONE: " + GetTime() +
                               "\n";
        }

        private static IEnumerable<XElement> GetElements(string url)
        {
            var animedoc = XElement.Load(url);
            return animedoc.Elements("anime");
        }

        private void AnimeFunction(int animeId)
        {
            while (true)
            {
                var animeObject = PageScraper.GetAnimeInfo(animeId);
                if (DatabaseControl.AnimeExists(animeId, false))
                {
                    DatabaseControl.InsertGenre(animeId, animeObject.Genre);
                    DatabaseControl.InsertAnime(animeObject);
                    if (animeObject.SequelID != 0)
                    {
                        if (DatabaseControl.AnimeExists((int) animeObject.SequelID, false))
                        {
                            animeId = animeObject.SequelID.Value;
                            continue;
                        }
                        if (animeObject.PrequelID != 0)
                        {
                            if (DatabaseControl.AnimeExists((int) animeObject.PrequelID, false))
                            {
                                animeId = animeObject.PrequelID.Value;
                                continue;
                            }
                        }
                    }
                }
                else
                {
                    var oldAnime = DatabaseControl.GetAnimeById(animeId);
                    var updateAnime = new UpdateAnime
                    {
                        Aired = animeObject.Aired == oldAnime.Aired ? null : animeObject.Aired,
                        Episodes = animeObject.Episodes == oldAnime.Episodes ? null : animeObject.Episodes,
                        Status = animeObject.Status == oldAnime.Status ? null : animeObject.Status,
                        Duration = animeObject.Duration == oldAnime.Duration ? null : animeObject.Duration,
                        Rating = animeObject.Rating == oldAnime.Rating ? null : animeObject.Rating,
                        Prequel = animeObject.Prequel == oldAnime.Prequel ? null : animeObject.Prequel,
                        Sequel = animeObject.Sequel == oldAnime.Sequel ? null : animeObject.Sequel,
                        ID = animeId
                    };

                    if (updateAnime.Status != null || updateAnime.Aired != null || updateAnime.Duration != null ||
                        updateAnime.Rating != null || updateAnime.Prequel != null || updateAnime.Sequel != null ||
                        updateAnime.Episodes != null)
                    {
                        txtResults.Text += "\nAnimeID: " + animeId + "\n";
                        UpdateTxtResults(oldAnime, updateAnime);
                        DatabaseControl.UpdateAnime(updateAnime);
                    }
                }
                break;
            }
        }

        private void MyAnimeFun(IEnumerable<XElement> animeElements)
        {
            var animeDbId = new List<int>();
            txtResults.Text += "START: " + GetTime() + "\n";
            foreach (var anime in animeElements)
            {
                var myanimeObject = new MyAnime();

                var status = anime.Element("my_status")?.Value;
                var episodes = Convert.ToInt32(anime.Element("my_watched_episodes")?.Value);
                var score = Convert.ToInt32(anime.Element("my_score")?.Value);
                var animeId = Convert.ToInt32(anime.Element("series_animedb_id")?.Value);
                animeDbId.Add(animeId);

                if (DatabaseControl.AnimeExists(animeId, true))
                {
                    myanimeObject.AnimeID = animeId;
                    myanimeObject.WatchedEpisodes = episodes;
                    myanimeObject.Score = score;
                    myanimeObject.Status = status;
                    DatabaseControl.InsertAnime(myanimeObject);
                }
                else
                {
                    var myanime = DatabaseControl.GetMyAnimeById(animeId);
                    var myStatus = MyAnime.GetStatus(status);
                    //change to just update specific items
                    if (Convert.ToInt32(myanime.Status) != MyAnime.GetStatus(status) ||
                        score != Convert.ToInt32(myanime.Score) || episodes != Convert.ToInt32(myanime.WatchedEpisodes))
                    {
                        DatabaseControl.UpdateAnime(animeId, score, episodes, myStatus);
                    }
                }
            }

            txtResults.Text += "DONE: " + GetTime() + "\n";
        }

        private void btnUpdateMyAnime_Click(object sender, RoutedEventArgs e)
        {
            var animeElements = GetElements("http://myanimelist.net/malappinfo.php?u=CWarlord87&status=all&type=anime");
            MyAnimeFun(animeElements);
        }

        private void btnUpdateAnime_Click(object sender, RoutedEventArgs e)
        {
            var animes = DatabaseControl.GetAnime();
            var anime = animes.ToList();
            var urllist = anime.Select(id => $"http://myanimelist.net/anime/{id.ID}").ToList();

            RunAnime(urllist);
        }

        private void RunAnime(IList<string> animeList)
        {
            var urlChecker = new UrlChecker();
            var AnimeList = urlChecker.Check(animeList);

            foreach (var anime in AnimeList)
            {
                if (DatabaseControl.AnimeExists(anime.ID, false))
                {
                    DatabaseControl.InsertGenre(anime.ID, anime.Genre);
                    DatabaseControl.InsertAnime(anime);
                }
                else
                {
                    var oldAnime = DatabaseControl.GetAnimeById(anime.ID);
                    var updateAnime = new UpdateAnime
                    {
                        Aired = anime.Aired == oldAnime.Aired ? null : anime.Aired,
                        Episodes = anime.Episodes == oldAnime.Episodes ? null : anime.Episodes,
                        Status = anime.Status == oldAnime.Status ? null : anime.Status,
                        Duration = anime.Duration == oldAnime.Duration ? null : anime.Duration,
                        Rating = anime.Rating == oldAnime.Rating ? null : anime.Rating,
                        Prequel = anime.Prequel == oldAnime.Prequel ? null : anime.Prequel,
                        Sequel = anime.Sequel == oldAnime.Sequel ? null : anime.Sequel,
                        ID = anime.ID
                    };
                    if (updateAnime.Status != null || updateAnime.Aired != null || updateAnime.Duration != null ||
                        updateAnime.Rating != null || updateAnime.Prequel != null || updateAnime.Sequel != null ||
                        updateAnime.Episodes != null)
                    {
                        txtResults.Text += "\nAnimeID: " + anime.ID + "\n";
                        UpdateTxtResults(oldAnime,updateAnime);
                        DatabaseControl.UpdateAnime(updateAnime);
                    }
                }
            }
        }


        void UpdateTxtResults(Anime oldAnime, UpdateAnime updateAnime)
        {
            if (updateAnime.Status != null)
            {
                txtResults.Text += $"Status: {oldAnime.Status} -> {updateAnime.Status}\n";
            }
            if (updateAnime.Aired != null)
            {
                txtResults.Text += $"Aired: {oldAnime.Aired} -> {updateAnime.Aired}\n";
            }
            if (updateAnime.Duration != null)
            {
                txtResults.Text += $"Duration: {oldAnime.Duration} -> {updateAnime.Duration}\n";
            }
            if (updateAnime.Rating != null)
            {
                txtResults.Text += $"Rating: {oldAnime.Rating} -> {updateAnime.Rating}\n";
            }
            if (updateAnime.Prequel != null)
            {
                txtResults.Text += $"Prequel: {oldAnime.Prequel} -> {updateAnime.Prequel}\n";
            }
            if (updateAnime.Sequel != null)
            {
                txtResults.Text += $"Sequel: {oldAnime.Sequel} -> {updateAnime.Sequel}\n";
            }
            if (updateAnime.Episodes != null)
            {
                txtResults.Text += $"Episodes: {oldAnime.Episodes} -> {updateAnime.Episodes}\n";
            }
        }
        private static string GetTime()
        {
            return $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}";
        }
    }
}