using System.Data.SqlClient;
using System.Linq;
using System.Data;
using Dapper;

namespace AnimeUploader
{
    internal class DatabaseControl
    {
        private readonly SqlConnection _connection =
            new SqlConnection(
                "Data Source=DESKTOP-AQTJ6NL\\ANIMELIST;Initial Catalog=MyAnimeList;Integrated Security=True");

        public void MyAnimeList(MyAnime myAnime)
        {
            if (MyAnimeExists(myAnime.AnimeID))
            {
                _connection.Execute("InsertMyAnimeList", myAnime, commandType: CommandType.StoredProcedure);
            }
        }

        public void AnimeList(Anime anime)
        {
            if (AnimeExists(anime.ID))
            {
                _connection.Query(string.Format(
                    "Insert into Anime Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                    anime.ID, anime.Title, anime.Type, anime.Episodes, anime.Status, anime.Aired, anime.Duration,
                    anime.Rating, anime.Description, "", ""));
            }
        }

        public void UpdateMyAnimeList()
        { }
        public void InsertMyAnimeList()
        { }

        public void UpdateAnimeList()
        { }
        public void InsertAnimeList()
        { }
        public bool AnimeExists(int AnimeDB)
        {
            var list = _connection.Query<Anime>("Select ID from Anime where ID = " + AnimeDB).ToList();
            return list.Count == 0;
        }

        public bool MyAnimeExists(int AnimeDB)
        {
            var list = _connection.Query<MyAnime>("Select ID from MyAnimeList where AnimeID = " + AnimeDB).ToList();
            return list.Count == 0;
        }
    }
}