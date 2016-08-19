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
        private static List<Item> LoadJson()
        {
            List<Item> items = new List<Item>();
            using (var r = new StreamReader("DatabaseSettings.json"))
            {
                var json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<Item>>(json);
            }
            return items;
            
        }

        private class Item
        {
            public string DataSource;
            public string InitialCatalog;
            public string IntegratedSecurity;


        }

 
        //move to own settings file
        private SqlConnection _connection;
           

        public void PopulateConnection()
        {
            var json = LoadJson();
           _connection = new SqlConnection("Data Source=" + json[0].DataSource + ";Initial Catalog=" + json[0].InitialCatalog + ";Integrated Security="+json[0].IntegratedSecurity);
           
        }

        public void Dispose()
        {
            
            _connection.Close();
        }

        public bool GenreExist(int animeId, int genreId)
        {
            var genre = _connection.Query("GetGenreByGenreId", new {AnimeId = animeId, GenreId = genreId},
                commandType: CommandType.StoredProcedure);
            return !genre.Any();
           
        }

        public MyAnime GetMyAnimeById(int animeId)
        {
            return _connection.Query<MyAnime>("GetMyAnimeById", new {ID = animeId},
                commandType: CommandType.StoredProcedure).FirstOrDefault();
        }

        public IEnumerable<Anime> GetAnime()
        {
            return _connection.Query<Anime>("select * from Anime").ToList();
        }

        public Anime GetAnimeById(int animeId)
        {
            return _connection.Query<Anime>("GetAnimeById", new {ID = animeId},
                commandType: CommandType.StoredProcedure).FirstOrDefault();
        }

        public void UpdateAnime(int animeId, int score = -1, int watchedEpisodes = -1, int status = -1)
        {
            _connection.Execute("UpdateMyAnime",
                new {ID = animeId, Score = score, WatchedEpisodes = watchedEpisodes, Status = status},
                commandType: CommandType.StoredProcedure);
        }

        public void InsertAnime(MyAnime myAnime)
        {
            _connection.Execute("InsertMyAnime", myAnime, commandType: CommandType.StoredProcedure);
        }

        public void InsertAnime(Anime anime)
        {
            _connection.Execute("InsertAnime", anime, commandType: CommandType.StoredProcedure);
        }

        public void UpdateAnime(UpdateAnime anime)
        {
            _connection.Execute("UpdateAnime", anime, commandType: CommandType.StoredProcedure);
        }

        public void InsertGenre(int animeId, string genres)
        {
            var genre = genres.Split(',');
            foreach (var g in genre)
            {
                _connection.Execute("InsertGenre", new {AnimeID = animeId, Genre = g},
                    commandType: CommandType.StoredProcedure);
            }
        }

        public List<GetAnime> GetAllMyListId()
        {
           return _connection.Query<GetAnime>("Select AnimeId from MyAnimeList").ToList();
        }

        public bool AnimeExists(int animeDb, bool checkMyList)
        {
            bool doesExist;
            if (checkMyList)
            {
                var list = _connection.Query<MyAnime>("Select ID from MyAnimeList where AnimeID = " + animeDb).ToList();
                doesExist = list.Count == 0;
            }
            else
            {
                var list = _connection.Query<Anime>("Select ID from Anime where ID = " + animeDb).ToList();
                doesExist = list.Count == 0;
            }
            return doesExist;
        }

    }
}