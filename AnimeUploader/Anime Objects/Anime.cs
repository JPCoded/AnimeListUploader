#region

using System.Collections.Generic;

#endregion

namespace AnimeUploader
{
    //get rid of switch statements
    public sealed class Anime : IAnime
    {
        private static readonly Dictionary<string, int> TypeDictionary = new Dictionary<string, int>
        {
            {"TV", 1},
            {"OVA", 2},
            {"ONA", 3},
            {"Special", 4},
            {"Movie", 5}
        };

        public int ID { get; set; }
        public string Title { get; set; }
        public int? Type { get; set; }
        public int? Status { get; set; }
        public int? Episodes { get; set; }
        public string Aired { get; set; }
        public string Duration { get; set; }
        public int? Rating { get; set; }
        public string Description { get; set; }
        public int? SequelID { get; set; }
        public string Sequel { get; set; }
        public int? PrequelID { get; set; }
        public string Prequel { get; set; }
        public string Genre { get; set; }

        public int GetRating(string rating)
        {
            int ratingNumber;

            switch (rating)
            {
                case AniRating.G:
                    ratingNumber = 1;
                    break;
                case AniRating.Pg:
                    ratingNumber = 2;
                    break;
                case AniRating.Pg13:
                    ratingNumber = 3;
                    break;
                case AniRating.R:
                    ratingNumber = 4;
                    break;
                case AniRating.X:
                    ratingNumber = 5;
                    break;
                default:
                    ratingNumber = 6;
                    break;
            }
            return ratingNumber;
        }

        public int GetType(string type) => TypeDictionary[type];

        public int GetStatus(string status)
        {
            var statusNumber = 0;

            switch (status)
            {
                case AniAiring.NotAired:
                    statusNumber = 1;
                    break;
                case AniAiring.Airing:
                    statusNumber = 2;
                    break;
                case AniAiring.Finished:
                    statusNumber = 3;
                    break;
                default:
                    break;
            }
            return statusNumber;
        }
    }
}
