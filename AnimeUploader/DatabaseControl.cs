using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Dapper;
using Newtonsoft.Json;

//CONVERT TO ASYNC
//change to read both tables at once if needed to reduce number of calls to database.

namespace AnimeUploader
{
    internal class DatabaseControl : IDisposable
    {
        private static readonly SqlConnection Connection;

        static DatabaseControl()
        {
            var json = LoadJson();
            Connection =
                new SqlConnection("Data Source=" + json[0].DataSource + ";Initial Catalog=" + json[0].InitialCatalog +
                                  ";Integrated Security=" + json[0].IntegratedSecurity);
        }

        public void Dispose()
        {
            Connection.Close();
        }

        private static List<Item> LoadJson()
        {
            List<Item> items;
            using (var r = new StreamReader("DatabaseSettings.json"))
            {
                var json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<Item>>(json);
            }
            return items;
        }

        public bool GenreExist(int animeId, int genreId)
        {
            var genre = Connection.Query("GetGenreByGenreId", new {AnimeId = animeId, GenreId = genreId},
                commandType: CommandType.StoredProcedure);
            return !genre.Any();
        }

        public static MyAnime GetMyAnimeById(int animeId)
        {
            return Connection.Query<MyAnime>("GetMyAnimeById", new {ID = animeId},
                commandType: CommandType.StoredProcedure).FirstOrDefault();
        }

        public static IEnumerable<Anime> GetAnime()
        {
            return Connection.Query<Anime>("select * from Anime").ToList();
        }

        public static Anime GetAnimeById(int animeId)
        {
            return Connection.Query<Anime>("GetAnimeById", new {ID = animeId},
                commandType: CommandType.StoredProcedure).FirstOrDefault();
        }

        public static void UpdateAnime(int animeId, int score = -1, int watchedEpisodes = -1, int status = -1)
        {
            Connection.Execute("UpdateMyAnime",
                new {ID = animeId, Score = score, WatchedEpisodes = watchedEpisodes, Status = status},
                commandType: CommandType.StoredProcedure);
        }

        public static void InsertAnime(MyAnime myAnime)
        {
            Connection.Execute("InsertMyAnime", myAnime, commandType: CommandType.StoredProcedure);
        }

        public static void InsertAnime(Anime anime)
        {
            Connection.Execute("InsertAnime", anime, commandType: CommandType.StoredProcedure);
        }

        public static void UpdateAnime(UpdateAnime anime)
        {
            Connection.Execute("UpdateAnime", anime, commandType: CommandType.StoredProcedure);
        }

        public static void InsertGenre(int animeId, string genres)
        {
            var genre = genres.Split(',');
            foreach (var g in genre)
            {
                Connection.Execute("InsertGenre", new {AnimeID = animeId, Genre = g},
                    commandType: CommandType.StoredProcedure);
            }
        }

        public List<GetAnime> GetAllMyListId()
        {
            return Connection.Query<GetAnime>("Select AnimeId from MyAnimeList").ToList();
        }

        public static bool AnimeExists(int animeDb, bool checkMyList)
        {
            bool doesExist;
            if (checkMyList)
            {
                var list = Connection.Query<MyAnime>("Select ID from MyAnimeList where AnimeID = " + animeDb).ToList();
                doesExist = list.Count == 0;
            }
            else
            {
                var list = Connection.Query<Anime>("Select ID from Anime where ID = " + animeDb).ToList();
                doesExist = list.Count == 0;
            }
            return doesExist;
        }

        private struct Item
        {
            public string DataSource;
            public string InitialCatalog;
            public string IntegratedSecurity;
        }
    }
}