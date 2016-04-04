using System;
using System.Windows;
using System.Xml.Linq;

namespace AnimeUploader
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

//Need to refactor and fix variable names
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            var dbControl = new DatabaseControl();
            var getAnime = new PageScraper();
            var animedoc = XElement.Load("http://myanimelist.net/malappinfo.php?u=CWarlord87&status=all&type=anime");
            var animeElements = animedoc.Elements("anime");
            foreach (var anime in animeElements)
            {
                var myanimeObject = new MyAnime();
                Anime animeObject;

                var status = anime.Element("my_status").Value;
                var episodes = Convert.ToInt32(anime.Element("my_watched_episodes").Value);
                var score = Convert.ToInt32(anime.Element("my_score").Value);
                var animeId = Convert.ToInt32(anime.Element("series_animedb_id").Value);
                var title = anime.Element("series_title").Value.Replace("'", "''");


                if (dbControl.AnimeExists(animeId))
                {
                    animeObject = getAnime.GetAnimeInfo(animeId);
                    animeObject.Title = title;
                    dbControl.InsertAnimeList(animeObject);
                }
                else
                {
                    animeObject = getAnime.GetAnimeInfo(animeId);
                    var oldAnime = dbControl.GetAnimeById(animeId);
                    var updateAnime = new UpdateAnime
                    {
                        Aired = animeObject.Aired == oldAnime.Aired ? null : animeObject.Aired,
                        Episodes = animeObject.Episodes == oldAnime.Episodes ? -1 : animeObject.Episodes,
                        Status = animeObject.Status == oldAnime.Status ? -1 : animeObject.Status,
                        Duration = animeObject.Duration == oldAnime.Duration ? null : animeObject.Duration,
                        Rating = animeObject.Rating == oldAnime.Rating ? -1 : animeObject.Rating,
                        Prequel = animeObject.Prequel == oldAnime.Prequel ? null : animeObject.Prequel,
                        Sequel = animeObject.Sequel == oldAnime.Sequel ? null : animeObject.Sequel,
                        ID = animeId
                    };

                    if (updateAnime.Status != -1 || updateAnime.Aired != null || updateAnime.Duration != null ||
                        updateAnime.Rating != -1 ||
                        updateAnime.Prequel != null || updateAnime.Sequel != null || updateAnime.Episodes != -1)
                    {

                        txtResults.Text += "\nAnimeID: " + animeId + "\n";
                        if (updateAnime.Status != -1)
                        {
                            txtResults.Text += "Status: " + animeObject.Status + " -> " + updateAnime.Status + "\n";
                        }
                        if (updateAnime.Aired != null)
                        {
                            txtResults.Text += "Aired: " + animeObject.Aired + " -> " + updateAnime.Aired + "\n";
                        }
                        if (updateAnime.Duration != null)
                        {
                            txtResults.Text += "Duration: " + animeObject.Duration + " -> " + updateAnime.Duration + "\n";
                        }
                        if (updateAnime.Rating != -1)
                        {
                            txtResults.Text += "Rating: " + animeObject.Rating + " -> " + updateAnime.Rating + "\n";
                        }
                        if (updateAnime.Prequel != null)
                        {
                            txtResults.Text += "Prequel: " + animeObject.Prequel + " -> " + updateAnime.Prequel + "\n";
                        }
                        if (updateAnime.Sequel != null)
                        {
                            txtResults.Text += "Sequel: " + animeObject.Sequel + " -> " + updateAnime.Sequel + "\n";
                        }
                        if (updateAnime.Episodes != -1)
                        {
                            txtResults.Text += "Episodes: " + animeObject.Episodes + " -> " + updateAnime.Episodes + "\n";
                        }
                        dbControl.UpdateAnimeList(updateAnime);
                    }
                }


                if (dbControl.MyAnimeExists(animeId))
                {
                    myanimeObject.AnimeID = animeId;
                    myanimeObject.WatchedEpisodes = episodes;
                    myanimeObject.Score = score;
                    myanimeObject.Status = status;
                    dbControl.InsertMyAnimeList(myanimeObject);
                }
                else
                {
                    var myanime = dbControl.GetMyAnimeById(animeId);
                    var myStatus = myanime.GetStatus(status);
                    //change to just update specific items
                    if (Convert.ToInt32(myanime.Status) != myanime.GetStatus(status) ||
                        score != Convert.ToInt32(myanime.Score) || episodes != Convert.ToInt32(myanime.WatchedEpisodes))
                        dbControl.UpdateMyAnimeList(animeId, score, episodes, myStatus);
                }
            }

            txtResults.Text += "DONE";
        }
    }
}