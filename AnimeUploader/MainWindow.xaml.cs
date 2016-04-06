using System;
using System.Collections;
using System.Collections.Generic;
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
            var animeElements = getElements("http://myanimelist.net/malappinfo.php?u=CWarlord87&status=all&type=anime");
           
            txtResults.Text += "START: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second +
                               "\n";
            foreach (var anime in animeElements)
            {
                var myanimeObject = new MyAnime();
                Anime animeObject;

                var status = anime.Element("my_status").Value;
                var episodes = Convert.ToInt32(anime.Element("my_watched_episodes").Value);
                var score = Convert.ToInt32(anime.Element("my_score").Value);
                var animeId = Convert.ToInt32(anime.Element("series_animedb_id").Value);
                var title = anime.Element("series_title").Value.Replace("'", "''");


                if (dbControl.AnimeExists(animeId, false))
                {
                    animeObject = PageScraper.GetAnimeInfo(animeId);
                    animeObject.Title = title;
                    dbControl.InsertGenre(animeId,animeObject.Genre);
                    dbControl.InsertAnime(animeObject);
                }
                else
                {
                    animeObject = PageScraper.GetAnimeInfo(animeId);
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
                        updateAnime.Rating != null ||
                        updateAnime.Prequel != null || updateAnime.Sequel != null || updateAnime.Episodes != null)
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

            txtResults.Text += "DONE: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second +
                               "\n";
            ;
        }

        private IEnumerable<XElement> getElements(string url)
        {
            var animedoc = XElement.Load(url);
            return animedoc.Elements("anime");
        }
    }
}