using System;
using System.Windows;
using System.Xml.Linq;
using HtmlAgilityPack;

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
            var animedoc = XElement.Load("animelist.xml");
            var animeElements = animedoc.Elements();
            foreach (var anime in animeElements)
            {
                var myanimeObject = new MyAnime();
                var animeObject = new Anime();

                var status = anime.Element("my_status").Value;
                var episodes = Convert.ToInt32(anime.Element("my_watched_episodes").Value);
                var score = Convert.ToInt32(anime.Element("my_score").Value);
                var animeId = Convert.ToInt32(anime.Element("series_animedb_id").Value);
                var title = anime.Element("series_title").Value;
                var newTitle = title.Replace("<![CDATA[","");
                newTitle = title.Replace("]]>","").Replace("'","''");
                animeObject = getAnime.GetAnimeInfo(animeId);
                animeObject.Title = newTitle;

             
                switch (status)
                {
                    case "Completed":
                        myanimeObject.Status = 1;
                        break;
                    case "Watching":
                        myanimeObject.Status = 2;
                        break;
                    case "Plan to Watch":
                        myanimeObject.Status = 3;
                        break;
                    default:
                        myanimeObject.Status = 4;
                        break;
                }
                myanimeObject.AnimeID = animeId;
                myanimeObject.Episodes = episodes;
                myanimeObject.Score = score;
                DBControl.AnimeList(animeObject);
                DBControl.MyAnimeList(myanimeObject);
               
            }
        }
    }
}