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

//Need to refactor 
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            var dbControl = new DatabaseControl();
            var getAnime = new PageScraper();
            var animedoc = XElement.Load("http://myanimelist.net/malappinfo.php?u=CWarlord87&status=all&type=anime");
            var animeElements = animedoc.Elements("anime");
            foreach (var anime in animeElements)
            {
                var myanimeObject = new MyAnime();
                var animeObject = new Anime();

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
                    var myanime = dbControl.getMyAnimeById(animeId);
                    var myStatus = myanime.GetStatus(status);
                  if(Convert.ToInt32(myanime.Status) != myanime.GetStatus(status) || score != Convert.ToInt32(myanime.Score) || episodes != Convert.ToInt32(myanime.WatchedEpisodes))
                        dbControl.UpdateMyAnimeList(animeId,score,episodes,myStatus);
                }
             // txtResults.Text=  dbControl.GetAnime().ToString();
            }
          //  txtResults.Text = "DONE";
        }
    }
}