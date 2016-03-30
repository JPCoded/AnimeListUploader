using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Linq;
using Dapper;


namespace AnimeUploader
{
    internal class DatabaseControl
    {
        private readonly SqlConnection _connection =
            new SqlConnection(
                "Data Source=DESKTOP-AQTJ6NL\\ANIMELIST;Initial Catalog=MyAnimeList;Integrated Security=True");

    public void InsertMyAnimeList(MyAnime myAnime)
        {
            if (MyAnimeExists(myAnime.AnimeID))
            {
                _connection.Query(string.Format(
                   "Insert into MyAnimeList Values({0},{1},{2},{3})", myAnime.Status, myAnime.Episodes, myAnime.Score, myAnime.AnimeID));
            }
        }

        public void InsertAnimeList(Anime anime)
        {
            if (AnimeExists(anime.ID))
            {
                _connection.Query(string.Format(
                    "Insert into Anime Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                    anime.ID, anime.Title, anime.Type, anime.Episodes, anime.Status,anime.Aired,anime.Duration,anime.Rating,anime.Description,"",""));
            }

        }

        public bool AnimeExists(int AnimeDB)
        {

            var list = _connection.Query<Anime>("Select ID from Anime where ID = " + AnimeDB).ToList();
            return list.Count == 0;
        }


    public bool MyAnimeExists(int AnimeDB)
        {
           var list =  _connection.Query<MyAnime>("Select ID from MyAnimeList where AnimeID = " + AnimeDB).ToList();
            return list.Count == 0;
        }
    }
}
