using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

//REFACTOR whole thing badly
namespace AnimeUploader
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

//Need to refactor and create loop to get all prequels
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            var dbControl = new DatabaseControl();
            var animeElements = GetElements("http://myanimelist.net/malappinfo.php?u=CWarlord87&status=all&type=anime");

            txtResults.Text += "START: " + GetTime() + "\n";
            foreach (var anime in animeElements)
            {
                var myanimeObject = new MyAnime();

                var status = anime.Element("my_status").Value;
                var episodes = Convert.ToInt32(anime.Element("my_watched_episodes").Value);
                var score = Convert.ToInt32(anime.Element("my_score").Value);
                var animeId = Convert.ToInt32(anime.Element("series_animedb_id").Value);

                AnimeFunction(animeId, dbControl);

                if (dbControl.AnimeExists(animeId, true))
                {
                    myanimeObject.AnimeID = animeId;
                    myanimeObject.WatchedEpisodes = episodes;
                    myanimeObject.Score = score;
                    myanimeObject.Status = status;
                    dbControl.InsertAnime(myanimeObject);
                }
                else
                {
                    var myanime = dbControl.GetMyAnimeById(animeId);
                    var myStatus = myanime.GetStatus(status);
                    //change to just update specific items
                    if (Convert.ToInt32(myanime.Status) != myanime.GetStatus(status) ||
                        score != Convert.ToInt32(myanime.Score) || episodes != Convert.ToInt32(myanime.WatchedEpisodes))
                        dbControl.UpdateAnime(animeId, score, episodes, myStatus);
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

        private void AnimeFunction(int animeId, DatabaseControl dbControl)
        {
            while (true)
            {
                var animeObject = PageScraper.GetAnimeInfo(animeId);
                if (dbControl.AnimeExists(animeId, false))
                {
                    dbControl.InsertGenre(animeId, animeObject.Genre);
                    dbControl.InsertAnime(animeObject);
                    if (animeObject.SequelID != 0)
                    {
                        if (dbControl.AnimeExists((int) animeObject.SequelID, false))
                        {
                            animeId = animeObject.SequelID.Value;
                            continue;
                        }
                        if (animeObject.PrequelID != 0)
                        {
                            if (dbControl.AnimeExists((int) animeObject.PrequelID, false))
                            {
                                animeId = animeObject.PrequelID.Value;
                                continue;
                            }
                        }
                    }
                }
                else
                {
                    var oldAnime = dbControl.GetAnimeById(animeId);
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
                        if (updateAnime.Status != null)
                        {
                            txtResults.Text += "Status: " + oldAnime.Status + " -> " + updateAnime.Status + "\n";
                        }
                        if (updateAnime.Aired != null)
                        {
                            txtResults.Text += "Aired: " + oldAnime.Aired + " -> " + updateAnime.Aired + "\n";
                        }
                        if (updateAnime.Duration != null)
                        {
                            txtResults.Text += "Duration: " + oldAnime.Duration + " -> " + updateAnime.Duration + "\n";
                        }
                        if (updateAnime.Rating != null)
                        {
                            txtResults.Text += "Rating: " + oldAnime.Rating + " -> " + updateAnime.Rating + "\n";
                        }
                        if (updateAnime.Prequel != null)
                        {
                            txtResults.Text += "Prequel: " + oldAnime.Prequel + " -> " + updateAnime.Prequel + "\n";
                        }
                        if (updateAnime.Sequel != null)
                        {
                            txtResults.Text += "Sequel: " + oldAnime.Sequel + " -> " + updateAnime.Sequel + "\n";
                        }
                        if (updateAnime.Episodes != null)
                        {
                            txtResults.Text += "Episodes: " + oldAnime.Episodes + " -> " + updateAnime.Episodes + "\n";
                        }
                        dbControl.UpdateAnime(updateAnime);
                    }
                }
                break;
            }
        }

        private void MyAnimeFun(IEnumerable<XElement> animeElements)
        {
            var dbControl = new DatabaseControl();
            var animeDbId = new List<int>();
            txtResults.Text += "START: " + GetTime() + "\n";
            foreach (var anime in animeElements)
            {
                var myanimeObject = new MyAnime();

                var status = anime.Element("my_status").Value;
                var episodes = Convert.ToInt32(anime.Element("my_watched_episodes").Value);
                var score = Convert.ToInt32(anime.Element("my_score").Value);
                var animeId = Convert.ToInt32(anime.Element("series_animedb_id").Value);
                animeDbId.Add(animeId);

                if (dbControl.AnimeExists(animeId, true))
                {
                    myanimeObject.AnimeID = animeId;
                    myanimeObject.WatchedEpisodes = episodes;
                    myanimeObject.Score = score;
                    myanimeObject.Status = status;
                    dbControl.InsertAnime(myanimeObject);
                }
                else
                {
                    var myanime = dbControl.GetMyAnimeById(animeId);
                    var myStatus = myanime.GetStatus(status);
                    //change to just update specific items
                    if (Convert.ToInt32(myanime.Status) != myanime.GetStatus(status) ||
                        score != Convert.ToInt32(myanime.Score) || episodes != Convert.ToInt32(myanime.WatchedEpisodes))
                    {
                        dbControl.UpdateAnime(animeId, score, episodes, myStatus);
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
            var dbcontrol = new DatabaseControl();
            var animes = dbcontrol.GetAnime();
            var anime = animes.ToList();
            var urllist = anime.Select(id => string.Format("http://myanimelist.net/anime/{0}", id.ID)).ToList();

            RunAnime(urllist);
        }

        private void RunAnime(IList<string> animeList)
        {
            var check = new UrlChecker();
          var AnimeList =  check.Check(animeList);
            var failedList = new List<int>();
        }

        private static string GetTime()
        {
            return string.Format("{0}:{1}:{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
    }
}