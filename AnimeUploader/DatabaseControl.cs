using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
//CONVERT TO ASYNC
//change to read both tables at once if needed to reduce number of calls to database.
namespace AnimeUploader
{
    internal class DatabaseControl
    {
        private readonly SqlConnection _connection =
            new SqlConnection(
                "Data Source=DESKTOP-AQTJ6NL\\ANIMELIST;Initial Catalog=MyAnimeList;Integrated Security=True");

        public MyAnime GetMyAnimeById(int AnimeID)
        {
            var ani = _connection.Query<MyAnime>("GetMyAnimeById", new {ID = AnimeID},
                commandType: CommandType.StoredProcedure);
            return ani.FirstOrDefault();
        }

        public Anime GetAnimeById(int AnimeID)
        {
            var ani = _connection.Query<Anime>("GetAnimeById", new { ID = AnimeID },
                commandType: CommandType.StoredProcedure);
            return ani.FirstOrDefault();
        }

        public void UpdateMyAnimeList(int animeId, int score = -1, int watchedEpisodes = -1, int status = -1)
        {
            _connection.Execute("UpdateMyAnime",
                new {ID = animeId, Score = score, WatchedEpisodes = watchedEpisodes, Status = status},
                commandType: CommandType.StoredProcedure);
        }

        public void InsertMyAnimeList(MyAnime myAnime)
        {
            _connection.Execute("InsertMyAnimeList", myAnime, commandType: CommandType.StoredProcedure);
        }

        public void UpdateAnimeList(UpdateAnime anime)
        {
            _connection.Execute("UpdateAnime", anime, commandType: CommandType.StoredProcedure);
        }

        public void InsertAnimeList(Anime anime)
        {
            _connection.Execute("InsertAnime", anime, commandType: CommandType.StoredProcedure);
        }

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