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
                _connection.Execute("InsertAnime", anime, commandType: CommandType.StoredProcedure);
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