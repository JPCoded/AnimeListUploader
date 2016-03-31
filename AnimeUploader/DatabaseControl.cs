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

        public void UpdateMyAnimeList()
        { }

        public void InsertMyAnimeList(MyAnime myAnime)
        {
            _connection.Execute("InsertMyAnimeList", myAnime, commandType: CommandType.StoredProcedure);
        }

        public void UpdateAnimeList()
        { }
        public void InsertAnimeList(Anime anime)
        { _connection.Execute("InsertAnime", anime, commandType: CommandType.StoredProcedure); }

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