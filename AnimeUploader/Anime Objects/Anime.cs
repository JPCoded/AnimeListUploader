﻿namespace AnimeUploader
{
    public class Anime
    {

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

        public static int GetRating(string rating)
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

        public static int GetType(string type)
        {
            var typeNumber = 0;

            switch (type)
            {
                case "TV":
                    typeNumber = 1;
                    break;
                case "OVA":
                    typeNumber = 2;
                    break;
                case "ONA":
                    typeNumber = 3;
                    break;
                case "Special":
                    typeNumber = 4;
                    break;
                case "Movie":
                    typeNumber = 5;
                    break;
            }
            return typeNumber;
        }

        public static int GetStatus(string status)
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
            }
            return statusNumber;
        }
    }

    internal static class AniRating
    {
        public const string G = "G - All Ages";
        public const string Pg = "PG - Children";
        public const string Pg13 = "PG-13 - Teens 13 or older";
        public const string R = "R - 17+ (violence &amp; profanity)";
        public const string X = "R+ - Mild Nudity";
    }

    internal static class AniAiring
    {
        public const string NotAired = "Not yet aired";
        public const string Airing = "Currently Airing";
        public const string Finished = "Finished Airing";
    }

    
}