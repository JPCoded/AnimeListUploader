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
            var DBControl = new DatabaseControl();
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
                var title = anime.Element("series_title").Value;
                var newTitle = title.Replace("<![CDATA[", "").Replace("]]>", "").Replace("'", "''");

                if (DBControl.AnimeExists(animeId))
                {
                    animeObject = getAnime.GetAnimeInfo(animeId);
                    animeObject.Title = newTitle;

                    DBControl.AnimeList(animeObject);
                }

                myanimeObject.AnimeID = animeId;
                myanimeObject.Episodes = episodes;
                myanimeObject.Score = score;
                myanimeObject.Status = myanimeObject.GetStatus(status);

                DBControl.MyAnimeList(myanimeObject);
            }
        }
    }
}